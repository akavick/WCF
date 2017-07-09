using System.Windows.Controls;

namespace WpfChatControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
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
    }
}
