using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WpfChatClient.ChatServiceReference;
using WpfChatClient.Interfaces;

namespace WpfChatClient.Classes
{
    public partial class TabFullChatControl : IChatContractCallback, IChat
    {
        private readonly object _listLocker = new object();
        private readonly object _chatLocker = new object();
        private readonly ConcurrentDictionary<string, string> _histories = new ConcurrentDictionary<string, string>();
        private TabControl TalksTabControl { get; }
        private FullChatControl MainChat { get; }
        private HashSet<ChatControl> PrivateTalks { get; }


        public string UserName { get; set; }
        public IChatContract Server { get; set; }


        public TabFullChatControl()
        {
            InitializeComponent();
            TalksTabControl = _talksTabControl;
            PrivateTalks = new HashSet<ChatControl>();
            MainChat = _fullChatControl;
            MainChat.TabItem = _mainChatTab;

            MainChat.SendButton.Click += SendButton_Click;
            MainChat.ClientsListBox.SelectionMode = SelectionMode.Single;
            MainChat.ClientsListBox.MouseDoubleClick += ClientsListBox_MouseDoubleClick;
            GetChatText().Text = GetMessageText().Text = "";
            MainChat.ClientsCountLabel.Content = 0;
        }






        private ChatControl GetOrCreateTab(string name)
        {
            try
            {
                var privateTalk = PrivateTalks.SingleOrDefault(t => t.TabItem.Header.ToString() == name);
                if (privateTalk != null)
                    return privateTalk;
                privateTalk = new ChatControl();
                ClearChat(privateTalk);
                if (_histories.ContainsKey(name))
                    GetTextFromRichTextBox(privateTalk.ChatRichTextBox).Text = _histories[name];

                void OnSendButtonOnClick(object sender, RoutedEventArgs eventArgs)
                {
                    var rtfMessage = GetTextFromRichTextBox(privateTalk.MessageRichTextBox);
                    var text = rtfMessage.Text;
                    if (string.IsNullOrEmpty(text))
                        return;
                    rtfMessage.Text = "";
                    Server.SendToPersonalChat(UserName, name, text);
                }
                privateTalk.SendButton.Click += OnSendButtonOnClick;

                var tabItem = new TabItem { Header = name, Content = privateTalk };

                void OnTabItemOnMouseDoubleClick(object sender, MouseButtonEventArgs eventArgs)
                {
                    var text = GetTextFromRichTextBox(privateTalk.ChatRichTextBox).Text;
                    _histories.AddOrUpdate(name, text, (oldT, newT) => newT);
                    PrivateTalks.Remove(privateTalk);
                    TalksTabControl.Items.Remove(tabItem);
                }
                tabItem.MouseDoubleClick += OnTabItemOnMouseDoubleClick;

                privateTalk.TabItem = tabItem;
                PrivateTalks.Add(privateTalk);
                TalksTabControl.Items.Add(tabItem);
                return privateTalk;
            }
            catch (Exception e)
            {
                GetChatText().Text += e.ToString();
            }
            return null;
        }



        private TextRange GetChatText()
            => GetTextFromRichTextBox(MainChat.ChatRichTextBox);


        private TextRange GetMessageText()
            => GetTextFromRichTextBox(MainChat.MessageRichTextBox);


        private TextRange GetTextFromRichTextBox(RichTextBox rtb)
            => new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);


        private void ClearChat(ChatControl chat)
            => GetTextFromRichTextBox(chat.MessageRichTextBox).Text = GetTextFromRichTextBox(chat.ChatRichTextBox).Text = "";


        private void ClientsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var name = MainChat.ClientsListBox.SelectedItem.ToString();
                if (name == UserName)
                    return;
                var chat = GetOrCreateTab(name);
                TalksTabControl.SelectedItem = chat.TabItem;
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
                Server.SendToMainChat(UserName, text);
            }
            catch (Exception exception)
            {
                GetChatText().Text += exception.ToString();
            }
        }


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
                        MainChat.ClientsListBox.Items.Remove(name);
                    else
                    {
                        var tempList = new SortedSet<string>(MainChat.ClientsListBox.Items.Cast<string>());
                        if (!tempList.Contains(name))
                            tempList.Add(name);
                        MainChat.ClientsListBox.Items.Clear();
                        foreach (var element in tempList)
                        {
                            if (element != UserName)
                                MainChat.ClientsListBox.Items.Add(element);
                        }
                        MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count.ToString();
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
                    MainChat.ClientsListBox.Items.Clear();
                    names = names.Distinct().OrderBy(n => n).ToArray();
                    foreach (var name in names)
                    {
                        if (name != UserName)
                            MainChat.ClientsListBox.Items.Add(name);
                    }
                    MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count;
                }
                catch (Exception e)
                {
                    GetChatText().Text += e.ToString();
                }
            }
        }























    }
}
