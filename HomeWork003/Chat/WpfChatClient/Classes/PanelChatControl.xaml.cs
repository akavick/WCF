﻿using System;
using System.IO;
using System.Text;
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
        //private object _locker = new object();

        public event Action<byte[]> UserTryingToSendMessage;

        public PanelChatControl()
        {
            InitializeComponent();
            try
            {
                GetMessageText().Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            _sendMessageButton.Click += SendButton_Click;
            _chatScrollViewer.ScrollChanged += _chatScrollViewer_ScrollChanged;
            _chatScrollViewer.PreviewMouseWheel += _chatScrollViewer_PreviewMouseWheel;
            _chatScrollViewer.PreviewMouseDown += _chatScrollViewer_PreviewMouseDown;
            _chatScrollViewer.PreviewMouseUp += _chatScrollViewer_PreviewMouseUp;
            _messageRichTextBox.PreviewKeyDown += _messageRichTextBox_PreviewKeyDown;
        }

        private TextRange GetMessageText()
            => new TextRange(_messageRichTextBox.Document.ContentStart, _messageRichTextBox.Document.ContentEnd);


        private void _messageRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.Escape:
                    {
                        _messageRichTextBox.Selection.Select(_messageRichTextBox.CaretPosition, _messageRichTextBox.CaretPosition);
                        break;
                    }
                    case Key.Enter:
                    {
                        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                            _messageRichTextBox.CaretPosition.InsertLineBreak();
                        else
                            Work();
                        e.Handled = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void _chatScrollViewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
            => DockBottomScroll();

        private void _chatScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
            => _clientNowScrolling = true;

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
            => DockBottomScroll();

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


        public void PushMessage(string userName, FlowDocument doc)
        {
            try
            {
                var flowChatMessage = new FlowChatMessage(doc, userName, DateTime.Now);
                DockPanel.SetDock(flowChatMessage, Dock.Top);
                _chatDockPanel.Children.Add(flowChatMessage);

                if (!_clientNowScrolling)
                    _chatScrollViewer.ScrollToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public void PushMessage(string userName, byte[] arr)
        {
            try
            {
                FlowDocument document;
                try
                {
                    using (var stream = new MemoryStream(arr))
                    {
                        document = XamlReader.Load(stream) as FlowDocument;
                    }

                    if (document != null)
                        PushMessage(userName, document);
                }
                catch (XamlParseException)
                {
                    document = new FlowDocument();
                    var message = Encoding.UTF8.GetString(arr);
                    var range = new TextRange(document.ContentStart, document.ContentEnd);
                    range.Text = message;
                    PushMessage(userName, document);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void Work()
        {
            try
            {
                if (GetMessageText().IsEmpty)
                    return;

                var flowDocument = _messageRichTextBox.Document;
                _messageRichTextBox.Document = new FlowDocument();
                byte[] arr;

                using (var stream = new MemoryStream())
                {
                    if (flowDocument != null)
                        XamlWriter.Save(flowDocument, stream);
                    arr = stream.ToArray();
                }

                UserTryingToSendMessage?.Invoke(arr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            Work();
        }

    }
}