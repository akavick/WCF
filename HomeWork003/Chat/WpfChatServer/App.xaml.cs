using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ChatLibrary.Classes;

namespace WpfChatServer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow { Title = "Chat SERVER" };
            var manager = new MyChatClientsManager();
            var chat = new MyManagedChatServer{ ClientsManager = manager };
            var presenter = new MyChatPresenter { View = mainWindow, Chat = chat };
            mainWindow.Show();
        }



    }
}
