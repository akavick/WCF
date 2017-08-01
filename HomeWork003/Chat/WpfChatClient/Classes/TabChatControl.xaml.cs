//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Input;
//using WpfChatClient.ChatServiceReference;
//using WpfChatClient.Interfaces;

//namespace WpfChatClient.Classes
//{
//    public partial class TabChatControl : IChatContractCallback, IChat
//    {
//        private readonly object _listLocker = new object();
//        private readonly object _chatLocker = new object();
//        private readonly ConcurrentDictionary<string, string> _histories = new ConcurrentDictionary<string, string>();
//        private TabControl TalksTabControl { get; }
//        private IMainChatControl MainChat { get; }
//        private HashSet<IChatControl> PrivateTalks { get; }


//        public string UserName { get; set; }
//        public IChatContract Server { get; set; }


//        public TabChatControl()
//        {
//            InitializeComponent();
//            TalksTabControl = _talksTabControl;
//            PrivateTalks = new HashSet<IChatControl>();
//            MainChat = _fullChatControl;

//            MainChat.Tag = _mainChatTab;
//            MainChat.UserTryingToSendMessage += MainChat_UserTryingToSendMessage;

//            MainChat.ClientsListBox.SelectionMode = SelectionMode.Single;
//            MainChat.ClientsListBox.MouseDoubleClick += ClientsListBox_MouseDoubleClick;

//            MainChat.ClientsCountLabel.Content = 0;
//        }

//        private async void MainChat_UserTryingToSendMessage(byte[] arr)
//        {
//            await Dispatcher.InvokeAsync(() =>
//            {
//                try
//                {
//                    if (arr == null || arr.Length == 0)
//                        return;
//                    Server.SendToMainChat(UserName, arr);
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.ToString());
//                }
//            });
//        }

//        private IChatControl GetOrCreateTab(string name)
//        {
//            try
//            {
//                var privateTalk = PrivateTalks.SingleOrDefault(t => (t.Tag as TabItem)?.Header.ToString() == name);
//                if (privateTalk != null)
//                    return privateTalk;
//                privateTalk = new PanelChatControl();

//                //if (_histories.ContainsKey(name))
//                //    GetTextFromRichTextBox(privateTalk.ChatRichTextBox).Text = _histories[name];

//                void OnUserTryingToSendMessage(byte[] arr)
//                {
//                    try
//                    {
//                        if (arr == null || arr.Length == 0)
//                            return;
//                        Server.SendToPersonalChat(UserName, name, arr);
//                    }
//                    catch (Exception e)
//                    {
//                        MessageBox.Show(e.ToString());
//                    }
//                }
//                privateTalk.UserTryingToSendMessage += OnUserTryingToSendMessage;

//                var tabItem = new TabItem { Header = name, Content = privateTalk };

//                void OnTabItemOnMouseDoubleClick(object sender, MouseButtonEventArgs eventArgs)
//                {
//                    try
//                    {
//                        //var text = GetTextFromRichTextBox(privateTalk.ChatRichTextBox).Text;
//                        //_histories.AddOrUpdate(name, text, (oldT, newT) => newT);

//                        PrivateTalks.Remove(privateTalk);
//                        TalksTabControl.Items.Remove(tabItem);
//                    }
//                    catch (Exception e)
//                    {
//                        MessageBox.Show(e.ToString());
//                    }
//                }
//                tabItem.MouseDoubleClick += OnTabItemOnMouseDoubleClick;

//                privateTalk.Tag = tabItem;
//                PrivateTalks.Add(privateTalk);
//                TalksTabControl.Items.Add(tabItem);
//                return privateTalk;
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show(e.ToString());
//            }
//            return null;
//        }


//        private void ClientsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//        {
//            try
//            {
//                var name = MainChat.ClientsListBox.SelectedItem.ToString();
//                if (name == UserName)
//                    return;
//                var chat = GetOrCreateTab(name);
//                TalksTabControl.SelectedItem = chat.Tag as TabItem;
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(exception.ToString());
//            }
//        }




//        public async void RefreshMainChat(string name, byte[] message)
//        {
//            await Dispatcher.InvokeAsync(() =>
//            {
//                try
//                {
//                    lock (_chatLocker)
//                    {
//                        MainChat.PushMessage(name, message);
//                    }
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.ToString());
//                }
//            });
//        }


//        public async void RefreshPersonalChat(string sender, string reciever, byte[] message)
//        {
//            await Task.Yield();

//            await Dispatcher.InvokeAsync(() =>
//            {
//                try
//                {
//                    var talk = GetOrCreateTab(reciever);
//                    talk.PushMessage(sender, message);
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.ToString());
//                }
//            });
//        }


//        public async void RefreshClientList(string name, bool quitted = false)
//        {
//            await Dispatcher.InvokeAsync(() =>
//                           {
//                               try
//                               {
//                                   lock (_listLocker)
//                                   {
//                                       if (quitted)
//                                           MainChat.ClientsListBox.Items.Remove(name);
//                                       else
//                                       {
//                                           var tempList = new SortedSet<string>(MainChat.ClientsListBox.Items.Cast<string>());
//                                           if (!tempList.Contains(name))
//                                               tempList.Add(name);
//                                           MainChat.ClientsListBox.Items.Clear();
//                                           foreach (var element in tempList)
//                                           {
//                                               if (element != UserName)
//                                                   MainChat.ClientsListBox.Items.Add(element);
//                                           }
//                                           MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count.ToString();
//                                       }
//                                   }
//                               }
//                               catch (Exception e)
//                               {
//                                   MessageBox.Show(e.ToString());
//                               }
//                           });
//        }


//        public async void FullRefreshClientList(string[] names)
//        {
//            await Dispatcher.InvokeAsync(() =>
//            {
//                try
//                {
//                    lock (_listLocker)
//                    {
//                        MainChat.ClientsListBox.Items.Clear();
//                        names = names.Distinct().OrderBy(n => n).ToArray();
//                        foreach (var name in names)
//                        {
//                            if (name != UserName)
//                                MainChat.ClientsListBox.Items.Add(name);
//                        }
//                        MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count;
//                    }
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.ToString());
//                }
//            });
//        }




















//    }
//}


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using WpfChatClient.ChatServiceReference;
using WpfChatClient.Interfaces;

namespace WpfChatClient.Classes
{
    public partial class TabChatControl : IChatContractCallback, IChat
    {
        private readonly object _listLocker = new object();
        private readonly object _chatLocker = new object();
        private readonly ConcurrentDictionary<string, string> _histories = new ConcurrentDictionary<string, string>();
        private TabControl TalksTabControl { get; }
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
            await Task.Yield();

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

                async Task OnUserTryingToSendMessage(byte[] arr)
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
                }
                privateTalk.UserTryingToSendMessage += OnUserTryingToSendMessage;

                var header = new PrivateTalkTabHeader { TabName = name };
                var tabItem = new TabItem { Header = header, Content = privateTalk };
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


        private async void ClientsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await Task.Yield();

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




        public async void RefreshMainChat(string name, byte[] message)
        {
            await Task.Yield();

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


        public async void RefreshPersonalChat(string sender, string reciever, byte[] message)
        {
            await Task.Yield();

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


        public async void RefreshClientList(string name, bool quitted = false)
        {
            await Task.Yield();

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
                        MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        public async void FullRefreshClientList(string[] names)
        {
            await Task.Yield();

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
                    MainChat.ClientsCountLabel.Content = MainChat.ClientsListBox.Items.Count;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }




















    }
}
