using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
            Chat.MainChat.SendButton.Click += SendButton_Click;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public void RefreshMainChat(string message)
        {
            
        }

        public void RefreshPersonalChat(string message)
        {
            
        }

        public event Action<string> IncomingMainChatMessage;
        public event Action<string> IncomingPersonalChatMessage;
    }
}
