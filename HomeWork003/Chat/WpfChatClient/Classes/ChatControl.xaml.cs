using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfChatClient.Interfaces;

namespace WpfChatClient.Classes
{
    public partial class ChatControl : IChatControl
    {
        public RichTextBox ChatRichTextBox { get; }
        public RichTextBox MessageRichTextBox { get; }
        public Button SendButton { get; }
        public TabItem TabItem { get; set; }

        public event Action<object> SendClick;

        public ChatControl()
        {
            InitializeComponent();
            ChatRichTextBox = _chatRichTextBox;
            MessageRichTextBox = _messageRichTextBox;
            SendButton = _sendMessageButton;
            TabItem = null;
            SendButton.Click += SendButton_Click;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            //endClick?.Invoke(sender, e);
        }

        private TextRange GetChatText()
            => GetTextFromRichTextBox(ChatRichTextBox);


        private TextRange GetMessageText()
            => GetTextFromRichTextBox(MessageRichTextBox);


        private TextRange GetTextFromRichTextBox(RichTextBox rtb)
            => new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
    }
}
