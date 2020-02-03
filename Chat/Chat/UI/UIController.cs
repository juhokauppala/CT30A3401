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
                    SetSelected();
                });
        }

        private void SetChannels(ChatData data)
        {
            ItemCollection users = UserList.Items;
            users.Clear();
            foreach (Channel channel in data.Users)
            {
                users.Add(channel);
            }

            ItemCollection channels = ChannelList.Items;
            channels.Clear();
            foreach (Channel channel in data.Channels)
            {
                channels.Add(channel);
            }
        }

        private void SetSelected()
        {
            if (selected == null)
                return;

            ItemCollection messages = MessageBox.Items;
            messages.Clear();

            foreach(Message message in selected.Messages)
            {
                messages.Add(message);
            }

            ChannelType.Text = selected.ChannelType.ToString();
            TargetChannel.Text = selected.Name;

            bool isTargetChannelFrozen = selected.ChannelType == MessageReceiver.User;
            TargetChannel.IsReadOnly = isTargetChannelFrozen;
        }

        public void SelectChannel(Channel channel)
        {
            selected = channel;
        }

    }
}
