using System;
using System.ServiceModel;
using System.Windows;
using WpfChatClient.Connected_Services.ChatServiceReference;

namespace WpfChatClient
{
    public partial class App
    {
        private MainWindow _window;
        private IChatContract _server;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var userName = "AKAVICK";
                if (e.Args.Length > 0)
                    userName = e.Args[0];
                _window = new MainWindow();
                var ic = new InstanceContext(_window);
                _server = new ChatContractClient(ic);
                _window.InitializeClient(userName, _server);
                _window.Show();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
    }
}
