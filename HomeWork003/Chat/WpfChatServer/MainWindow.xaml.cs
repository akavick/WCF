using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using ChatLibrary.Interfaces;

namespace WpfChatServer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChatView
    {
        public event Action<string> IncomingMainChatMessage;
        public event Action<string> IncomingPersonalChatMessage;

        public MainWindow()
        {
            InitializeComponent();
            Chat.MainChat.SendButton.Click += SendButton_Click;
            GetChatText().Text = "";
            GetMessageText().Text = "";
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => IncomingMainChatMessage?.Invoke(GetMessageText().Text);

        public void RefreshMainChat(string message) => GetChatText().Text += message;

        public void RefreshPersonalChat(string message)
        {
            
        }

        private TextRange GetChatText() => GetTextFromRichTextBox(Chat.MainChat.ChatRichTextBox);

        private TextRange GetMessageText() => GetTextFromRichTextBox(Chat.MainChat.MessageRichTextBox);

        private TextRange GetTextFromRichTextBox(RichTextBox rtb) => new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
    }
}
