using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using ChatLibrary.Classes;
using ChatLibrary.Interfaces;

namespace WpfChatServer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        MyChatPresenter _presenter = null;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow { Title = "Chat SERVER" };
            var _presenter = new MyChatPresenter(mainWindow);
            mainWindow.Show();
        }



    }
}
