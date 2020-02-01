using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data
{
    public class Channel
    {
        private List<Message> messages;
        public string Name { get; private set; }

        public Channel(string name)
        {
            Name = name;
            messages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            messages.Add(message);
        }
    }
}
