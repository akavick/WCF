using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfChatClient.Interfaces;

namespace WpfChatClient.Classes
{
    public partial class MainChatControl : IMainChatControl
    {
        public ListBox ClientsListBox { get; } //todo: fixit
        public Label ClientsCountLabel { get; set; } //todo: fixit

        public event Func<byte[], Task> UserTryingToSendMessage;

        private readonly IChatControl _chat;

        public MainChatControl()
        {
            InitializeComponent();

            ClientsListBox = _clientsListBox;
            ClientsCountLabel = _clientsCountLabel;
            _chat = _chatControl;
            _chat.UserTryingToSendMessage += _chat_UserTryingToSendMessage;
        }

        private async Task _chat_UserTryingToSendMessage(byte[] arr)
        {
            try
            {
                var task = UserTryingToSendMessage?.Invoke(arr);
                if (task != null)
                    await task;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void PushMessage(string userName, FlowDocument doc)
        {
            try
            {
                _chat.PushMessage(userName, doc);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void PushMessage(string userName, byte[] arr)
        {
            try
            {
                _chat.PushMessage(userName, arr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
