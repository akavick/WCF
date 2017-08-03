using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfChatClient.ChatServiceReference;
using WpfChatClient.Interfaces;
using MahApps.Metro.Controls;

namespace WpfChatClient.Classes
{
    public partial class TabChatControl : IChatContractCallback, IChat
    {
        private readonly object _listLocker = new object();
        private readonly object _chatLocker = new object();
        //private readonly ConcurrentDictionary<string, string> _histories = new ConcurrentDictionary<string, string>();
        private MetroAnimatedTabControl  TalksTabControl { get; }
        private IMainChatControl MainChat { get; }
        private HashSet<IChatControl> PrivateTalks { get; }


        public string UserName { get; set; }
        public IChatContract Server { get; set; }


        public TabChatControl()
        {
            InitializeComponent();
            TalksTabControl = _talksTabControl;
            PrivateTalks = new HashSet<IChatControl>();
            MainChat = _fullChatControl;

            MainChat.Tag = _mainChatTab;
            MainChat.UserTryingToSendMessage += MainChat_UserTryingToSendMessage;

            MainChat.ClientsListBox.SelectionMode = SelectionMode.Single;
            MainChat.ClientsListBox.MouseDoubleClick += ClientsListBox_MouseDoubleClick;

            MainChat.ClientsCountLabel.Content = 0;
        }

        private async Task MainChat_UserTryingToSendMessage(byte[] arr)
        {
            try
            {
                if (arr == null || arr.Length == 0)
                    return;
                await Server.SendToMainChatAsync(UserName, arr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private IChatControl GetOrCreateTab(string name)
        {
            try
            {
                var privateTalk = PrivateTalks.SingleOrDefault(t => ((t.Tag as TabItem)?.Header as PrivateTalkTabHeader)?.TabName.ToString() == name);
                if (privateTalk != null)
                    return privateTalk;
                privateTalk = new PanelChatControl();

                //if (_histories.ContainsKey(name))
                //    GetTextFromRichTextBox(privateTalk.ChatRichTextBox).Text = _histories[name];

                privateTalk.UserTryingToSendMessage += async arr =>
                {
                    try
                    {
                        if (arr == null || arr.Length == 0)
                            return;
                        await Server.SendToPersonalChatAsync(UserName, name, arr);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                };

                var header = new PrivateTalkTabHeader { TabName = name };
                var tabItem = new MetroTabItem { Header = header, Content = privateTalk };
                header.CloseButtonPressed += () =>
                {
                    try
                    {
                        //var text = GetTextFromRichTextBox(privateTalk.ChatRichTextBox).Text;
                        //_histories.AddOrUpdate(name, text, (oldT, newT) => newT);

                        PrivateTalks.Remove(privateTalk);
                        TalksTabControl.Items.Remove(tabItem);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                };

                privateTalk.Tag = tabItem;
                PrivateTalks.Add(privateTalk);
                TalksTabControl.Items.Add(tabItem);
                return privateTalk;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return null;
        }


        private void ClientsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var name = MainChat.ClientsListBox.SelectedItem.ToString();
                if (name == UserName)
                    return;
                var chat = GetOrCreateTab(name);
                TalksTabControl.SelectedItem = chat.Tag as TabItem;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }




        public void RefreshMainChat(string name, byte[] message)
        {
            try
            {
                lock (_chatLocker)
                {
                    MainChat.PushMessage(name, message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        public void RefreshPersonalChat(string sender, string reciever, byte[] message)
        {
            try
            {
                var talk = GetOrCreateTab(reciever);
                talk.PushMessage(sender, message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        public void RefreshClientList(string name, bool quitted = false)
        {
            try
            {
                lock (_listLocker)
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
                        MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count + 1;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        public void FullRefreshClientList(string[] names)
        {
            try
            {
                lock (_listLocker)
                {
                    MainChat.ClientsListBox.Items.Clear();
                    names = names.Distinct().OrderBy(n => n).ToArray();
                    foreach (var name in names)
                    {
                        if (name != UserName)
                            MainChat.ClientsListBox.Items.Add(name);
                    }
                    MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count + 1;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }




















    }
}
