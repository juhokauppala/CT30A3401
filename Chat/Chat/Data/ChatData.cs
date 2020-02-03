using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Data
{
    public class ChatData
    {
        private static ChatData singleton = null;
        private Dictionary<string, Channel> channels;
        private Dictionary<string, Channel> directMessages;

        public static ChatData GetInstance()
        {
            if (singleton == null)
                singleton = new ChatData();

            return singleton;
        }

        private ChatData()
        {
            channels = new Dictionary<string, Channel>();
            directMessages = new Dictionary<string, Channel>();
        }

        private void AddMessageToDict(Message message, Dictionary<string, Channel> dictionary)
        {
            string key = message.ReceiverType == MessageReceiver.User ? message.SenderName : message.ReceiverName;
            bool channelExists = dictionary.ContainsKey(key);
            
            if (channelExists)
            {
                dictionary[message.ReceiverName].AddMessage(message);
            } else
            {
                Channel newChannel = new Channel(message.ReceiverName);
                newChannel.AddMessage(message);
                dictionary.Add(message.ReceiverName, newChannel);
            }
        }

        public void AddMessage(Message message)
        {
            switch (message.MessageType)
            {
                case MessageType.ChatMessage:
                    switch (message.ReceiverType)
                    {
                        case MessageReceiver.Channel:
                            AddMessageToDict(message, channels);
                            break;
                        case MessageReceiver.User:
                            AddMessageToDict(message, directMessages);
                            break;
                        default:
                            throw new Exception("Unknown ReceiverType");
                    }
                    break;
                case MessageType.UserInformation:
                    break;
                default:
                    throw new Exception($"Unknown Message type: {message.MessageType}");
            }
        }
    }
}
