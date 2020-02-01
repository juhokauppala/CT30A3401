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
    class Connection : IConnection
    {
        private TcpListener listener = null;
        private TcpClient client = null;
        private Task<TcpClient> waiter = null;
        private Thread thread = null;
        private List<Message> messages = new List<Message>();

        public Connection(IPAddress IP, int port)
        {
            listener = new TcpListener(IP, port);
        }

        public bool HasClient => client != null;
        public bool ClientIsConnected { get; private set; } = false;

        public bool HasNewMessage => messages.Count > 0;

        public string Name => throw new NotImplementedException();

        public void Dispose()
        {
            if (client.Connected)
                client.Close();

            listener.Stop();
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
                    hasFoundClient = true;
                    ClientIsConnected = true;
                    client = waiter.Result;
                    Logger.GetInstance().NewInfoLine("Client connected!");
                }

                if (client != null)
                {
                    Message message = ReadClient(client);
                    if (message != null)
                        messages.Add(message);
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

#endregion
    }
}
