using Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.Server
{
    interface IConnection
    {
        public void StartListeningAsync();
        public bool HasClient { get; }
        public bool TestConnection();

        public bool HasNewMessage { get; }
        public Message[] GetMessages();
        public void WriteMessage(Message messages);

        public void Dispose();

        public string Name { get; }
    }
}
