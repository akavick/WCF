using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfChatServer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
            this.Exit += App_Exit;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            InitializeComponent();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {

        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            this.MainWindow = new MainWindow();
        }
    }
}
