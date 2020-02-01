
using Chat.Data;
using Chat.UI;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
                MessageType = MessageType.ChatMessage,
                ReceiverType = MessageReceiver.Channel,
                ReceiverName = "PublicChannel",
                SenderName = "Client1",
                Text = message
            };
            TcpIO.WriteStream(stream, msgObject);
        }

        private void StartListening()
        {
            Thread thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }

        private void Loop()
        {
            NetworkStream stream = client.GetStream();
            Message newMessage;
            while (client.Connected)
            {
                if (stream.DataAvailable && stream.CanRead)
                {
                    newMessage = TcpIO.ReadStream(stream);
                    ChatData.GetInstance().AddMessage(newMessage);
                    UIController.GetInstance().Refresh();
                }

                Thread.Yield();
            }
        }

        public void Activate(string name)
        {
            LogInAs(name);
            StartListening();
        }

        private void LogInAs(string name)
        {
            Message logInMessage = new Message()
            {
                MessageType = MessageType.LoginInformation,
                ReceiverType = MessageReceiver.User,
                ReceiverName = null,
                SenderName = name,
                Text = null
            };
            TcpIO.WriteStream(client.GetStream(), logInMessage);
        }
    }
}
