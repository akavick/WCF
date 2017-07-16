using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfChatClient
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string userName = "User";
            if (e.Args.Length > 0)
            {
                userName = e.Args[0];
            }
            var mainWindow = new MainWindow(userName);
            mainWindow.Show();
        }
    }
}
