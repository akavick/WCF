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
        private readonly HashSet<TabItem> _finishedTalks = new HashSet<TabItem>();
        private readonly object _listLocker = new object();
        private readonly object _chatLocker = new object();

        private IChatContract _server;
        private string _userName = null;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Chat.MainChat.SendButton.Click += SendButton_Click;
            Chat.MainChat.ClientsListBox.SelectionMode = SelectionMode.Single;
            Chat.MainChat.ClientsListBox.MouseDoubleClick += ClientsListBox_MouseDoubleClick;
            GetChatText().Text = GetMessageText().Text = "";
            Chat.MainChat.ClientsCountLabel.Content = 0;
            Closing += MainWindow_Closing;
        }

        public void InitializeClient(string userName, IChatContract server)
        {
            try
            {
                if (string.IsNullOrEmpty(userName))
                    return;
                Title = _userName = userName;
                if (server == null)
                    return;
                _server = server;
                _server.IamIn(_userName);
            }
            catch (Exception e)
            {
                GetChatText().Text += e.ToString();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //var c = _server as IChatContractChannel;
                //var a = c.LocalAddress.ToString();
                //MessageBox.Show(Chat.MainChat.ClientsListBox.Items.CanSort.ToString());
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void ClientsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var name = Chat.MainChat.ClientsListBox.SelectedItem.ToString();
                if (name == _userName)
                    return;
                var chat = GetOrCreateTab(name);
                Chat.TalksTabControl.SelectedItem = chat.TabItem;
            }
            catch (Exception exception)
            {
                GetChatText().Text += exception.ToString();
            }
        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var rtfMessage = GetMessageText();
                var text = rtfMessage.Text;
                if (string.IsNullOrEmpty(text))
                    return;
                rtfMessage.Text = "";
                _server.SendToMainChat(_userName, text);
            }
            catch (Exception exception)
            {
                GetChatText().Text += exception.ToString();
            }
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
            try
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
                    _server.SendToPersonalChat(_userName, name, text, true);
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
                            _server.SendToPersonalChat(_userName, name, quitMessage, false);
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
            catch (Exception e)
            {
                GetChatText().Text += e.ToString();
            }
            return null;
        }

        #region IChatCallback

        public void RefreshMainChat(string message)
        {
            lock (_chatLocker)
            {
                GetChatText().Text += message;
            }
        }


        public void RefreshPersonalChat(string name, string message, bool talkFinished)
        {
            try
            {
                var talk = GetOrCreateTab(name);
                if (talkFinished)
                    _finishedTalks.Add(talk.TabItem);
                GetTextFromRichTextBox(talk.ChatRichTextBox).Text += message;
            }
            catch (Exception e)
            {
                GetChatText().Text += e.ToString();
            }
        }


        public void RefreshClientList(string name, bool quitted = false)
        {
            lock (_listLocker)
            {
                try
                {
                    if (quitted)
                        Chat.MainChat.ClientsListBox.Items.Remove(name);
                    else
                    {
                        var tempList = new SortedSet<string>(Chat.MainChat.ClientsListBox.Items.Cast<string>());
                        if (!tempList.Contains(name))
                            tempList.Add(name);
                        Chat.MainChat.ClientsListBox.Items.Clear();
                        foreach (var element in tempList)
                        {
                            if (element != _userName)
                                Chat.MainChat.ClientsListBox.Items.Add(element);
                        }
                        Chat.MainChat.ClientsCountLabel.Content = Chat.MainChat.ClientsListBox.Items.Count.ToString();
                    }
                }
                catch (Exception e)
                {
                    GetChatText().Text += e.ToString();
                }
            }
        }


        public void FullRefreshClientList(string[] names)
        {
            lock (_listLocker)
            {
                try
                {
                    Chat.MainChat.ClientsListBox.Items.Clear();
                    foreach (var name in names.Distinct().OrderBy(n => n).ToArray())
                    {
                        if (name != _userName)
                            Chat.MainChat.ClientsListBox.Items.Add(name);
                    }
                    Chat.MainChat.ClientsCountLabel.Content = Chat.MainChat.ClientsListBox.Items.Count;
                }
                catch (Exception e)
                {
                    GetChatText().Text += e.ToString();
                }
            }
        }

        #endregion
    }
}
