using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{   
    [Serializable]
    public class Message
    {
        public string Text { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public MessageReceiver ReceiverType { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        ChatMessage,
        LoginInformation
    }

    public enum MessageReceiver
    {
        Channel,
        User
    }
}
