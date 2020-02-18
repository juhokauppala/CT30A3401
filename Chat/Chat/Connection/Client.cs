
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
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;

namespace Chat.Connection
{
    class Client
    {
        private static Client instance = null;
        public IPAddress IP { get; private set; }
        public static int Port { get; } = 50000;

        private bool keepListening = true;
        public string Name { get; private set; }

        public static Client GetInstance()
        {
            if (instance == null)
                instance = new Client();

            return instance;
        }

        public static void Reset()
        {
            instance.Dispose();
            instance = null;
        }

        private TcpClient client;

        private Client()
        {
            
        }

        public void SendMessage(string message, string receiver, MessageReceiver receiverType)
        {
            NetworkStream stream = client.GetStream();
            Message msgObject = new Message() {
                MessageType = MessageType.ChatMessage,
                ReceiverType = receiverType,
                ReceiverName = receiver,
                SenderName = Name,
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
            DateTime lastHeartbeat = SendHeartbeat();
            while (client.Connected && keepListening)
            {
                if (client.Connected && stream.DataAvailable && stream.CanRead)
                {
                    newMessage = TcpIO.ReadStream(stream);
                    ChatData.GetInstance().AddMessage(newMessage);
                    UIController.GetInstance().Refresh();
                }

                if (DateTime.Now.Subtract(lastHeartbeat).TotalSeconds >= 2)
                {
                    lastHeartbeat = SendHeartbeat();
                }

                Thread.Yield();
            }
        }

        public bool Activate(string name, string address)
        {
            try
            {
                IP = IPAddress.Parse(address);
            } catch
            {
                return false;
            }
            if (LogInAs(name))
            {
                Name = name;
                UIController.GetInstance().UserName.Text = Name;
                StartListening();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool LogInAs(string name)
        {
            try
            {
                client = new TcpClient(IP.ToString(), Port);
            } catch
            {
                return false;
            }
            //client.LingerState.Enabled = true;
            //client.LingerState.LingerTime = 0;
            //Debug.WriteLine($"Connected: {client.Connected}");
            Message logInMessage = new Message()
            {
                MessageType = MessageType.LoginInformation,
                ReceiverType = MessageReceiver.User,
                ReceiverName = null,
                SenderName = name,
                Text = $"My port is: {client.Client.LocalEndPoint.ToString()}"
            };
            TcpIO.WriteStream(client.GetStream(), logInMessage);
            Message response = null;
            Debug.WriteLine("Started login...");
            Stopwatch waitingForResponse = Stopwatch.StartNew();
            while (response == null && waitingForResponse.Elapsed.TotalSeconds < 3)
            {
                response = TcpIO.ReadStream(client.GetStream());
                Thread.Yield();
            }
            waitingForResponse.Stop();
            Debug.WriteLine("Login-response received!");
            if (response.MessageType == MessageType.LoginInformation &&
                response.ReceiverName == name)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private DateTime SendHeartbeat()
        {
            TcpIO.WriteStream(client.GetStream(), new Message()
            {
                MessageType = MessageType.Heartbeat
            });
            return DateTime.Now;
        }

        public void Dispose()
        {
            keepListening = false;
            Debug.WriteLine("Disposing client...");
            if (client.Connected)
            {
                client.Client.Disconnect(false);
                client.Dispose();
                Debug.WriteLine("Closed!");
            }
        }
    }
}
