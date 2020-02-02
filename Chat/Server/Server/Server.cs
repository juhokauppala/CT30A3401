using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Server.Logging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Linq;

namespace Server.Server
{
    class Server
    {
        public static IPAddress IP { get; } = IPAddress.Parse("127.0.0.1");

        private List<IConnection> connections;
        private List<IConnection> pendingConnections;
        private bool run = true;
        private bool hasStopped = false;

        private object connectionListLock = new object();

        public Server()
        {
            connections = new List<IConnection>();
            pendingConnections = new List<IConnection>();
        }

        public void Start(int port)
        {
            IConnection waitingConnection = new Connection(this, IP, port);
            waitingConnection.StartListeningAsync();
            List<Message> newMessages = new List<Message>();
            List<IConnection> closed = new List<IConnection>();
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timeToExecute;
            Logger logger = Logger.GetInstance();

            while (run)
            {
                stopwatch.Restart();

                // Check for a new client
                if (waitingConnection.HasClient)
                {
                    waitingConnection = new Connection(this, IP, port);
                    waitingConnection.StartListeningAsync();
                }

                // Add new users
                if (pendingConnections.Count >= 1)
                {
                    lock (connectionListLock)
                    {
                        connections.AddRange(pendingConnections);

                        foreach (IConnection pending in pendingConnections)
                        {
                            Message newUserInfo = new Message()
                            {
                                MessageType = MessageType.UserInformation,
                                ReceiverName = pending.Name
                            };

                            foreach (IConnection user in connections)
                            {
                                user.WriteMessage(newUserInfo);
                            }
                        }

                        pendingConnections.Clear();
                    }
                }

                // Update messages and closed sockets
                foreach (IConnection connection in connections)
                {
                    if (connection.HasNewMessage)
                    {
                        Message[] messages = connection.GetMessages();
                        foreach (Message message in messages)
                        {
                            Logger.GetInstance().NewInfoLine($"New message from {message.SenderName} to {message.ReceiverName}: {message.Text}");
                            newMessages.Add(message);
                        }
                    } else if (!connection.TestConnection())
                    {
                        closed.Add(connection);
                    }
                }

                // Remove closed sockets
                foreach (IConnection closedConnection in closed)
                {
                    Logger.GetInstance().NewInfoLine($"Removing: {closedConnection.Name}");
                    closedConnection.Dispose();
                    connections.Remove(closedConnection);
                }
                closed.Clear();

                // Deliver messages
                foreach (Message newMessage in newMessages)
                {
                    if (newMessage.ReceiverType == MessageReceiver.User)
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

                
                logger.UpdateStaticLine(connections.Count, timeToExecute, -1);

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
            Console.CursorVisible = true;
            while(!hasStopped)
            {
                Thread.Yield();
            }
        }

        public bool HasConnectionWithName(string name)
        {
            return connections.Select(conn => conn.Name == name).ToList().Count >= 1;
        }

        public void AddUser(Connection connection)
        {
            lock (connectionListLock)
            {
                pendingConnections.Add(connection);
            }
        }
    }
}
