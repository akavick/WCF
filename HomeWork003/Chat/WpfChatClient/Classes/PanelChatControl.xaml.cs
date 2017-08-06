using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using WpfChatClient.Interfaces;

namespace WpfChatClient.Classes
{
    public partial class PanelChatControl : IChatControl
    {
        private bool _clientNowScrolling;
        private readonly object _locker = new object();
        public event Func<byte[], Task> UserTryingToSendMessage;

        public PanelChatControl()
        {
            InitializeComponent();

            _sendMessageButton.Click += SendButton_Click;
            _chatScrollViewer.ScrollChanged += _chatScrollViewer_ScrollChanged;
            _chatScrollViewer.PreviewMouseWheel += _chatScrollViewer_PreviewMouseWheel;
            _chatScrollViewer.PreviewMouseDown += _chatScrollViewer_PreviewMouseDown;
            _chatScrollViewer.PreviewMouseUp += _chatScrollViewer_PreviewMouseUp;
            _textBox.PreviewKeyDown += _messageRichTextBox_PreviewKeyDown;
        }

        private void _messageRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            //try
            //{
            //    switch (e.Key)
            //    {
            //        case Key.Escape:
            //        {
            //            _messageRichTextBox.Selection.Select(_messageRichTextBox.CaretPosition, _messageRichTextBox.CaretPosition);
            //            break;
            //        }
            //        case Key.Enter:
            //        {
            //            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            //                _messageRichTextBox.CaretPosition.InsertLineBreak();
            //            else
            //                Work();
            //            e.Handled = true;
            //            break;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void _chatScrollViewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            DockBottomScroll();
        }

        private void _chatScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _clientNowScrolling = true;
        }

        private void _chatScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                _clientNowScrolling = true;
                _chatScrollViewer.ScrollToVerticalOffset(_chatScrollViewer.VerticalOffset - e.Delta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void _chatScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            DockBottomScroll();
        }

        private void DockBottomScroll()
        {
            try
            {
                if (Math.Abs(_chatScrollViewer.VerticalOffset - _chatScrollViewer.ScrollableHeight) > 10.0)
                    return;
                _clientNowScrolling = false;
                _chatScrollViewer.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void PushMessage(string userName, string message)
        {
            try
            {
                lock (_locker)
                {
                    var flowChatMessage = new ChatMessage(message, userName, DateTime.Now);
                    DockPanel.SetDock(flowChatMessage, Dock.Top);
                    _chatDockPanel.Children.Add(flowChatMessage);
                    if (_chatDockPanel.Children.Count > 500)
                    {
                        _chatDockPanel.Children.RemoveRange(0, 300);
                        GC.Collect();
                    }
                }

                if (!_clientNowScrolling)
                    _chatScrollViewer.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public async void PushMessage(string userName, byte[] arr)
        {
            await await Dispatcher.InvokeAsync(async () =>
            {
                await Task.Yield();
                try
                {
                    var message = Encoding.UTF8.GetString(arr);
                    PushMessage(userName, message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
        }



        private async void Work()
        {
            try
            {
                var messageText = _textBox.Text;

                if (string.IsNullOrWhiteSpace(messageText))
                    return;

                _sendMessageButton.IsEnabled = false;

                await Task.Run(() =>
                {

                }).ContinueWith(t =>
                {

                }, TaskScheduler.FromCurrentSynchronizationContext());


                byte[] arr = null;

                await await Dispatcher.InvokeAsync(async () =>
                {
                    await Task.Yield();
                    arr = Encoding.UTF8.GetBytes(_textBox.Text);
                });

                if (arr.Length > 100000000)
                    MessageBox.Show("Сообщение имеет слишком большую длину");
                else
                {
                    _textBox.Text = "";
                    UserTryingToSendMessage?.Invoke(arr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                _sendMessageButton.IsEnabled = true;
            }
        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //await Task.Yield();
            Work();
        }

    }
}
