﻿using Server.Logging;
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
    class Connection : IConnection
    {
        private TcpListener listener = null;
        private TcpClient client = null;
        private Task<TcpClient> waiter = null;
        private Thread thread = null;
        private List<Message> messages = new List<Message>();
        private Server server;
        private DateTime aliveUntil;

        public Connection(Server parent, IPAddress IP, int port)
        {
            listener = new TcpListener(IP, port);
            listener.Server.LingerState.Enabled = true;
            listener.Server.LingerState.LingerTime = 0;
            server = parent;
            aliveUntil = DateTime.Now.AddSeconds(5);
        }

        public bool HasClient => client != null;
        public bool ClientIsConnected { get; private set; } = false;

        public bool TestConnection()
        {
            return DateTime.Now <= aliveUntil;
        }

        public bool HasNewMessage => messages.Count > 0;

        public string Name { get; private set; }

        public void Dispose()
        {
            if (client.Connected)
                client.Close();

            listener.Stop();
            Logger.GetInstance().NewInfoLine($"Connection for {Name} closed!");
        }

        public Message[] GetMessages()
        {
            Message[] returned = messages.ToArray();
            messages.Clear();
            return returned;
        }

        public void StartListeningAsync()
        {
            listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
            Logger.GetInstance().NewInfoLine("Started accepting TCP client asynchronoysly");
            listener.Start();
            waiter = listener.AcceptTcpClientAsync();
            thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }

        public void WriteMessage(Message message)
        {
            if (HasClient && ClientIsConnected)
            {
                TcpIO.WriteStream(client.GetStream(), message);
            }
        }

        private void EndLoop()
        {
            if (thread.IsAlive)
                thread.Abort();
            Dispose();
        }

#region ThreadLoop

        protected void Loop()
        {
            bool hasFoundClient = false;
            while (ClientIsConnected || !hasFoundClient)
            {
                if (!hasFoundClient &&
                    waiter.IsCompleted &&
                    client == null)
                {
                    bool clientOK = AuthorizeClient(waiter.Result);
                    
                        hasFoundClient = true;
                        ClientIsConnected = true;
                        client = waiter.Result;
                        Logger.GetInstance().NewInfoLine("Client connected!");
                    if (!clientOK)
                    {
                        ClientIsConnected = false;
                        break;
                    }
                }

                if (client != null)
                {
                    Message message = ReadClient(client);
                    if (message != null)
                    {
                        if (message.MessageType == MessageType.Heartbeat)
                        {
                            aliveUntil = DateTime.Now.AddSeconds(5);
                        } else
                        {
                            messages.Add(message);
                        }
                    }
                      
                }

                if (client != null && !client.Connected)
                    ClientIsConnected = false;
                else
                    Thread.Yield();
            }
            EndLoop();
        }

        private Message ReadClient(TcpClient client)
        {
            return TcpIO.ReadStream(client.GetStream());
        }

        private bool AuthorizeClient(TcpClient client)
        {
            Logger.GetInstance().NewInfoLine("Started authenticating new client.");
            Message login = null;
            while (login == null)
            {
                login = TcpIO.ReadStream(client.GetStream());
            }
            bool loginOK = login.MessageType == MessageType.LoginInformation && !server.HasConnectionWithName(login.SenderName);

            Logger.GetInstance().NewInfoLine($"Authentication result: {loginOK}");

            if (loginOK)
            {
                Name = login.SenderName;
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
}
