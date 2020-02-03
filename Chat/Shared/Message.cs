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

        public string ToDebugString()
        {
            string str = "";
            str += $"SenderName: {SenderName}\n";
            str += $"Receivername: {ReceiverName}\n";
            str += $"Text: {Text}\n";
            str += $"ReceiverType: {ReceiverType}\n";
            str += $"MessageType: {MessageType}\n";
            return str;
        }

        public string Display()
        {
            return $"{SenderName}: {Text}";
        }

        public override string ToString()
        {
            return Display();
        }
    }

    public enum MessageType
    {
        ChatMessage,
        LoginInformation,
        UserInformation,
        Heartbeat
    }

    public enum MessageReceiver
    {
        Channel,
        User
    }
}
