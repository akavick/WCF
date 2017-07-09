using System.Windows.Controls;

namespace WpfChatControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для FullChatControl.xaml
    /// </summary>
    public partial class FullChatControl : UserControl
    {
        public RichTextBox ChatRichTextBox { get; }
        public RichTextBox MessageRichTextBox { get; }
        public Button SendButton { get; }
        public ListBox ClientsListBox { get; }
        public TabItem TabItem { get; set; }

        public FullChatControl()
        {
            InitializeComponent();
            ChatRichTextBox = _chatControl.ChatRichTextBox;
            MessageRichTextBox = _chatControl.MessageRichTextBox;
            SendButton = _chatControl.SendButton;
            ClientsListBox = _clientsListBox;
            TabItem = null;
        }
    }
}
