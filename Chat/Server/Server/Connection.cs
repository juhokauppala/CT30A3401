using Server.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Server
{
    class Connection
    {
        public ConnectionState State;

        private TcpListener listener = null;
        private TcpClient client = null;
        private Task<TcpClient> waiter = null;
        private Thread thread = null;
        private List<Message> messages = new List<Message>();
        private Server server;
        private DateTime aliveUntil;

        private object aliveLock = new object();

        public Connection(Server parent, IPAddress IP, int port)
        {
            listener = new TcpListener(IP, port);
            listener.Server.LingerState.Enabled = true;
            listener.Server.LingerState.LingerTime = 0;
            server = parent;
            State = ConnectionState.Waiting;
        }

        public bool IsAlive()
        {
            lock (aliveLock)
            {
                if (DateTime.Now > aliveUntil)
                    Logger.GetInstance().NewInfoLine($"No heartbeat! (AliveUntil: {aliveUntil.ToLongTimeString()}) (Time now: {DateTime.Now.ToLongTimeString()})");

                return DateTime.Now <= aliveUntil;
            }
        }

        public bool HasNewMessage => messages.Count > 0;

        public string Name { get; private set; }

        public void Dispose()
        {
            if (State != ConnectionState.Disposed)
            {
                State = ConnectionState.Disposed;
                if (client.Connected)
                    client.Close();

                Logger.GetInstance().NewInfoLine($"Disposing {Name}");
            }
        }

        public Message[] GetMessages()
        {
            Message[] returned = messages.ToArray();
            messages.Clear();
            return returned;
        }

        public void StartListeningAsync()
        {
            //listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            Logger.GetInstance().NewInfoLine("Started accepting TCP client asynchronoysly");
            listener.Start();
            waiter = listener.AcceptTcpClientAsync();
            thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }

        public void WriteMessage(Message message)
        {
            if (State == ConnectionState.Alive)
            {
                TcpIO.WriteStream(client.GetStream(), message);
            }
        }

#region ThreadLoop

        protected void Loop()
        {
            while (State != ConnectionState.Disposed)
            {
                if (waiter.IsCompleted &&
                    State == ConnectionState.Waiting)
                {
                    listener.Stop();
                    State = ConnectionState.Authenticating;
                    bool clientOK = AuthenticateClient(waiter.Result);
                    client = waiter.Result;
                    Logger.GetInstance().NewInfoLine("Client connected!");
                    if (!clientOK)
                    {
                        break;
                    }
                    State = ConnectionState.Alive;
                }

                if (State == ConnectionState.Alive)
                {
                    Message message = ReadClient(client);
                    if (message != null)
                    {
                        if (message.MessageType == MessageType.Heartbeat)
                        {
                            lock (aliveLock)
                            {
                                aliveUntil = DateTime.Now.AddSeconds(5);
                            }
                        } else
                        {
                            messages.Add(message);
                        }
                    }
                      
                }

                Thread.Yield();
            }
            Dispose();
        }

        private Message ReadClient(TcpClient client)
        {
            Message message = TcpIO.ReadStream(client.GetStream());
            //if (message != null)
            //    Logger.GetInstance().NewInfoLine(message.ToDebugString());
            return message;
        }

        private bool AuthenticateClient(TcpClient client)
        {
            Logger.GetInstance().NewInfoLine("Started authenticating new client.");
            Message login = null;
            while (login == null)
            {
                login = TcpIO.ReadStream(client.GetStream());
                Thread.Yield();
            }
            bool loginOK = (login.MessageType == MessageType.LoginInformation) && (!server.HasConnectionWithName(login.SenderName));
            Logger.GetInstance().NewInfoLine(login.ToDebugString());

            Logger.GetInstance().NewInfoLine($"Authentication result: {loginOK}");

            if (loginOK)
            {
                Name = login.SenderName;
                aliveUntil = DateTime.Now.AddSeconds(5);
                server.AddUser(this);
                return true;
            } else
            {
                Message response = new Message()
                {
                    MessageType = MessageType.UserInformation,
                    ReceiverName = null
                };
                TcpIO.WriteStream(client.GetStream(), response);
                return false;
            }
        }

#endregion
    }

    public enum ConnectionState
    {
        Waiting,
        Authenticating,
        Alive,
        Disposed
    }
}
