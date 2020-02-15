using Chat.Data;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Core;

namespace Chat.UI
{
    public class UIController
    {
        public ListView UserList;
        public ListView ChannelList;
        public ListView MessageBox;
        public TextBlock ChannelType;
        public TextBox TargetChannel;
        public TextBox MessageField;
        public TextBlock UserName;

        private Channel selected = null;

        private static UIController singleton = null;

        public static UIController GetInstance()
        {
            if (singleton == null)
                singleton = new UIController();

            return singleton;
        }

        private UIController()
        {

        }

        public void Refresh()
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    ChatData data = ChatData.GetInstance();
                    SetChannels(data);
                    UpdateSelectedChannelMessages();
                });
        }

        private void SetChannels(ChatData data)
        {
            ItemCollection users = UserList.Items;
            IEnumerable<object> deletedUsers = users.Except(data.Users);
            IEnumerable<object> newUsers = data.Users.Except(users);
            
            foreach (object deletedUser in deletedUsers)
            {
                users.Remove(deletedUser);
            }
            foreach (object newUser in newUsers)
            {
                users.Add(newUser);
            }

            ItemCollection channels = ChannelList.Items;
            IEnumerable<object> newChannels = data.Channels.Except(channels);

            foreach (object newChannel in newChannels)
            {
                channels.Add(newChannel);
            }
        }

        private void SetSelectedChannel(Channel newSelected)
        {
            if (newSelected == null || newSelected == selected)
                return;

            UpdateSelectedChannelMessages();
        }

        private void UpdateSelectedChannelMessages()
        {
            ItemCollection messages = MessageBox.Items;
            if (selected == null || selected.Messages == null)
                return;

            object[] removedMessages = messages.Except(selected.Messages).ToArray();

            foreach (object removed in removedMessages)
            {
                messages.Remove(removed);
            }

            object[] newMessages = selected.Messages.Except(messages).ToArray();

            foreach (object newMessage in newMessages)
            {
                messages.Add(newMessage);
            }
        }

        public void SelectChannel(string channelName, MessageReceiver channelType)
        {
            Channel newSelected;
            if (channelType == MessageReceiver.Channel)
            {
                newSelected = ChatData.GetInstance().Channels.Where(channel => channel.Name == channelName).First();
            } else if (channelType == MessageReceiver.User)
            {
                newSelected = ChatData.GetInstance().Users.Where(channel => channel.Name == channelName).First();
            } else
            {
                throw new Exception($"Unknown MessageType: {channelType}");
            }
            SetSelectedChannel(newSelected);

            ChannelType.Text = newSelected.ChannelType.ToString();
            TargetChannel.Text = newSelected.Name;

            bool isTargetChannelFrozen = newSelected.ChannelType == MessageReceiver.User;
            TargetChannel.IsReadOnly = isTargetChannelFrozen;

            selected = newSelected;
        }

    }
}
