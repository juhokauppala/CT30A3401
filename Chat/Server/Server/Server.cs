using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Server
{
    class Server
    {
        public static IPAddress IP { get; } = IPAddress.Parse("127.0.0.1");

        private List<IConnection> connections;
        private bool run = true;
        private bool hasStopped = false;

        public Server()
        {
            connections = new List<IConnection>();
        }

        public void Start(int port)
        {
            IConnection waitingConnection = new Connection(IP, port);
            waitingConnection.StartListeningAsync();
            List<Message> newMessages = new List<Message>();
            List<IConnection> closed = new List<IConnection>();
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timeToExecute;

            while (run)
            {
                stopwatch.Restart();

                // Check for a new client
                if (waitingConnection.HasClient)
                {
                    connections.Add(waitingConnection);
                    waitingConnection = new Connection(IP, port);
                    waitingConnection.StartListeningAsync();
                }

                // Update messages and closed sockets
                foreach (IConnection connection in connections)
                {
                    if (connection.HasNewMessage)
                    {
                        Message[] messages = connection.GetMessages();
                        foreach (Message message in messages)
                        {
                            Console.WriteLine($"New message from {message.SenderName} to {message.ReceiverName}: {message.Text}");
                            newMessages.Add(message);
                        }
                    } else if (!connection.HasClient)
                    {
                        closed.Add(connection);
                    }
                }

                // Remove closed sockets
                foreach (IConnection closedConnection in closed)
                {
                    closedConnection.Dispose();
                    connections.Remove(closedConnection);
                }
                closed.Clear();

                // Deliver messages
                foreach (Message newMessage in newMessages)
                {
                    if (newMessage.IsReceiverUser)
                    {
                        IConnection receiver = connections.Find(conn => conn.Name == newMessage.ReceiverName);
                        IConnection sender = connections.Find(conn => conn.Name == newMessage.SenderName);

                        if (receiver != null)
                            receiver.WriteMessage(newMessage);
                        
                        if (sender != null)
                            sender.WriteMessage(newMessage);

                    } else
                    {
                        foreach (IConnection receiver in connections)
                        {
                            receiver.WriteMessage(newMessage);
                        }
                    }
                }
                newMessages.Clear();

                timeToExecute = stopwatch.Elapsed;

                //Console.WriteLine($"One round of actions took: {timeToExecute.Seconds}.{timeToExecute.Milliseconds}s");

                Thread.Yield();
            }
            foreach (IConnection connection in connections)
            {
                connection.Dispose();
            }
            hasStopped = true;
        }

        public void Stop()
        {
            run = false;
            while(!hasStopped)
            {
                Thread.Yield();
            }
        }
    }
}
