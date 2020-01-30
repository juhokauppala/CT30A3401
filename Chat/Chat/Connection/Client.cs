
using System;
using System.Collections.Generic;
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
        }

        public void SendMessage(byte[] message)
        {
            NetworkStream stream = client.GetStream();
            stream.Write(message, 0, message.Length);
        }
    }
}
