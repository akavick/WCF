using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Ex13WindowsService
{
    [RunInstaller(true)]
    public partial class Ex13Installer : Installer
    {
        ServiceInstaller _serviceInstaller;
        ServiceProcessInstaller _processInstaller;
        public Ex13Installer()
        {
            InitializeComponent();
            _serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Manual,
                ServiceName = "Ex13DuplexWinSvc"
            };
            _processInstaller = new ServiceProcessInstaller {Account = ServiceAccount.LocalSystem};
            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}
