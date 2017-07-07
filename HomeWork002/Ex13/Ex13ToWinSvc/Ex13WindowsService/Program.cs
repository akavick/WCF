using System.ServiceProcess;

namespace Ex13WindowsService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase.Run(new Ex13Service());
        }
    }
}
