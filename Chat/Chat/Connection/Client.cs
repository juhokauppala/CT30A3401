
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Connection
{
    class Client
    {
        public static IPAddress IP { get; } = IPAddress.Parse("127.0.0.1");
        public static int Port { get; } = 3401;


        private TcpClient client;

        public Client()
        {
            client = new TcpClient(IP.ToString(), Port);
            Debug.WriteLine($"Connected: {client.Connected}");
        }

        public void SendMessage(string message)
        {
            Debug.WriteLine($"Sending message. Connected: {client.Connected}");
            NetworkStream stream = client.GetStream();
            Message msgObject = new Message() {
                IsReceiverUser = false,
                ReceiverName = "PublicChannel",
                SenderName = "Client1",
                Text = message
            };
            TcpIO.WriteStream(stream, msgObject);
        }
    }
}
