using System;
using System.ServiceModel;
using System.Windows;
using MahApps.Metro.Controls;
using WpfChatClient.ChatServiceReference;
using WpfChatClient.Interfaces;

namespace WpfChatClient
{
    public partial class MainWindow : MetroWindow
    {
        private readonly IChat _chat;

        public MainWindow()
        {
            InitializeComponent();
            _chat = Chat;
        }

        public void InitializeClient(string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    return;

                _chat.Server = new ChatContractClient(new InstanceContext(_chat));
                _chat.UserName = Title = userName;
                _chat.Server.IamIn(userName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


    }
}
