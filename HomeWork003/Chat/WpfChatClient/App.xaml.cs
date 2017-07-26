using System;
using System.Windows;

namespace WpfChatClient
{
    public partial class App
    {
        private MainWindow _window;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var userName = "AKAVICK";
                if (e.Args.Length > 0)
                    userName = e.Args[0];
                _window = new MainWindow();
                _window.InitializeClient(userName);
                _window.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
    }
}
