using System;
using System.Windows.Controls;
using WpfChatControlLibrary.Interfaces;

namespace WpfChatControlLibrary.Classes
{
    public partial class ChatControl : IChatControl
    {
        public RichTextBox ChatRichTextBox { get; }
        public RichTextBox MessageRichTextBox { get; }
        public Button SendButton { get; }
        public TabItem TabItem { get; set; }

        public ChatControl()
        {
            InitializeComponent();
            ChatRichTextBox = _chatRichTextBox;
            MessageRichTextBox = _messageRichTextBox;
            SendButton = _sendMessageButton;
            TabItem = null;
        }

        public IChatMessage GetWrittenMessage()
        {
            throw new NotImplementedException();
        }

        public IChatHistory GetHistory()
        {
            throw new NotImplementedException();
        }

        public void PushNewMessage(IChatMessage message)
        {
            
        }
    }
}
