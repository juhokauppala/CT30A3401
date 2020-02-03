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
        public IReadOnlyList<Message> Messages => messages;

        private List<Message> messages;
        public string Name { get; private set; }
        public MessageReceiver ChannelType;

        public Channel(string name, MessageReceiver channelType)
        {
            Name = name;
            ChannelType = channelType;
            messages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            messages.Add(message);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
