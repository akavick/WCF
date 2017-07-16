using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfChatClient.ChatServiceReference;
using WpfChatControlLibrary;

namespace WpfChatClient
{
    public partial class MainWindow : IChatContractCallback /*IChatView*/
    {
        private readonly string _userName;
        private readonly IChatContract _server;
        private readonly HashSet<TabItem> _finishedTalks = new HashSet<TabItem>();


        public MainWindow(string userName)
        {
            Title = _userName = userName;
            InitializeComponent();
            Chat.MainChat.SendButton.Click += SendButton_Click;
            Chat.MainChat.ClientsListBox.SelectionMode = SelectionMode.Single;
            Chat.MainChat.ClientsListBox.MouseDoubleClick += ClientsListBox_MouseDoubleClick;
            GetChatText().Text = GetMessageText().Text = "";
            InstanceContext ic = new InstanceContext(this);
            _server = new ChatContractClient(ic); //todo: разделить
            _server.IamIn(_userName);
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ClientsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var name = Chat.MainChat.ClientsListBox.SelectedItem.ToString();
            if (name == _userName)
                return;
            var chat = GetOrCreateTab(name);
            Chat.TalksTabControl.SelectedItem = chat.TabItem;
        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var rtfMessage = GetMessageText();
            var text = rtfMessage.Text;
            if (string.IsNullOrEmpty(text))
                return;
            rtfMessage.Text = "";
            _server.SendToMainChat(text);
        }


        private TextRange GetChatText()
            => GetTextFromRichTextBox(Chat.MainChat.ChatRichTextBox);


        private TextRange GetMessageText()
            => GetTextFromRichTextBox(Chat.MainChat.MessageRichTextBox);


        private TextRange GetTextFromRichTextBox(RichTextBox rtb)
            => new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);


        private void ClearChat(ChatControl chat) 
            => GetTextFromRichTextBox(chat.MessageRichTextBox).Text = GetTextFromRichTextBox(chat.ChatRichTextBox).Text = "";


        private ChatControl GetOrCreateTab(string name)
        {
            var privateTalk = Chat.PrivateTalks.SingleOrDefault(t => t.TabItem.Header.ToString() == name);
            if (privateTalk != null)
                return privateTalk;
            privateTalk = new ChatControl();
            ClearChat(privateTalk);
            void OnSendButtonOnClick(object sender, RoutedEventArgs eventArgs)
            {
                var rtfMessage = GetTextFromRichTextBox(privateTalk.MessageRichTextBox);
                var text = rtfMessage.Text;
                if (string.IsNullOrEmpty(text))
                    return;
                rtfMessage.Text = "";
                _server.SendToPersonalChat(name, text, true);
            }
            privateTalk.SendButton.Click += OnSendButtonOnClick;
            var tabItem = new TabItem { Header = name, Content = privateTalk };
            void OnTabItemOnMouseDoubleClick(object sender, MouseButtonEventArgs eventArgs)
            {
                var text = GetTextFromRichTextBox(privateTalk.ChatRichTextBox).Text;
                if (!string.IsNullOrEmpty(text))
                {
                    var quitMessage = $"***** {_userName} покинул беседу. *****";
                    if (!_finishedTalks.Contains(tabItem))
                        _server.SendToPersonalChat(name, quitMessage, false);
                    else
                        _finishedTalks.Remove(tabItem);
                }
                Chat.PrivateTalks.Remove(privateTalk);
                Chat.TalksTabControl.Items.Remove(tabItem);
            }
            tabItem.MouseDoubleClick += OnTabItemOnMouseDoubleClick;
            privateTalk.TabItem = tabItem;
            Chat.PrivateTalks.Add(privateTalk);
            Chat.TalksTabControl.Items.Add(tabItem);
            return privateTalk;
        }

        #region IChatCallback

        public void RefreshMainChat(string message) 
            => GetChatText().Text += message;


        public void RefreshPersonalChat(string name, string message, bool talkFinished)
        {
            var talk = GetOrCreateTab(name);
            if (talkFinished)
                _finishedTalks.Add(talk.TabItem);
            GetTextFromRichTextBox(talk.ChatRichTextBox).Text += message;
        }


        public void RefreshClientList(string name, bool quitted = false)
        {
            if (quitted)
                Chat.MainChat.ClientsListBox.Items.Remove(name);
            else
                Chat.MainChat.ClientsListBox.Items.Add(name);
        }


        public void FullRefreshClientList(string[] names)
        {
            Chat.MainChat.ClientsListBox.Items.Clear();
            foreach (var name in names)
                Chat.MainChat.ClientsListBox.Items.Add(name);
        }

        #endregion
    }
}
