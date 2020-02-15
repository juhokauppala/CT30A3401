using Chat.Connection;
using Chat.UI;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ICollection<Channel> Channels { get => channels.Values; }
        public ICollection<Channel> Users { get => directMessages.Values; }

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

            channels.Add("Default", new Channel("Default", MessageReceiver.Channel));
        }

        private void AddMessageToDict(Message message, Dictionary<string, Channel> dictionary)
        {
            string key = message.ReceiverType == MessageReceiver.User ? GetChannelKeyFromPrivateMessage(message) : message.ReceiverName;
            bool channelExists = dictionary.ContainsKey(key);
            
            if (channelExists)
            {
                dictionary[key].AddMessage(message);
            } else
            {
                Channel newChannel = new Channel(key, message.ReceiverType);
                newChannel.AddMessage(message);
                dictionary.Add(key, newChannel);
            }
            UIController.GetInstance().Refresh();
        }

        private string GetChannelKeyFromPrivateMessage(Message message)
        {
            if (message.SenderName != Client.GetInstance().Name)
            {
                return message.SenderName;
            }
            return message.ReceiverName;
        }

        public void AddMessage(Message message)
        {
            switch (message.MessageType)
            {
                case MessageType.ChatMessage:
                    Debug.WriteLine("Received Message!");
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
                case MessageType.UserList:
                    UpdateUserList(message.Text);
                    break;
                default:
                    throw new Exception($"Unknown Message type: {message.MessageType}");
            }
        }

        public void UpdateUserList(string userString)
        {
            string[] users = userString.Split(Message.UserListDelimiter);
            string[] newUsers = users.Except(directMessages.Keys).ToArray();
            string[] deletedUsers = directMessages.Keys.Except(users).ToArray();

            foreach (string deleted in deletedUsers)
            {
                directMessages.Remove(deleted);
            }
            foreach (string newUser in newUsers)
            {
                directMessages.Add(newUser, new Channel(newUser, MessageReceiver.User));
            }
        }
    }
}
