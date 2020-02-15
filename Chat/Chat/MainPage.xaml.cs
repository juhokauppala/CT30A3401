using Chat.Connection;
using Chat.Data;
using Chat.UI;
using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Chat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitUIControls(UIController.GetInstance());
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
        }

        private void ClickChannel(object sender, ItemClickEventArgs e)
        {
            UIController.GetInstance().SelectChannel(((Channel)e.ClickedItem).Name, MessageReceiver.Channel);
        }

        private void ClickUser(object sender, ItemClickEventArgs e)
        {
            UIController.GetInstance().SelectChannel(((Channel)e.ClickedItem).Name, MessageReceiver.User);
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            
            bool success = Client.GetInstance().Activate(UserNameField.Text);
            if (success)
            {
                NameScreen.Visibility = Visibility.Collapsed;
            }
        }

        private void InitUIControls(UIController controller)
        {
            controller.ChannelList = ChannelList;
            controller.UserList = UserList;
            controller.MessageBox = Messages;
            controller.MessageField = TextField;
            controller.TargetChannel = SendTarget;
            controller.ChannelType = SendingTo;
        }
    }
}
