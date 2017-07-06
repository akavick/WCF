using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ChatLibrary;

namespace MyServer
{
    class Program
    {
        static void Main()
        {
            Console.Title = "CHAT-SERVER";
            
            //var address = new Uri("soap.udp://localhost:8080/");
            //var binding = new UdpBinding();

            var address = new Uri("net.tcp://localhost:8002/MyService");
            var binding = new NetTcpBinding();

            var contract = typeof(IMyServer);
            var host = new ServiceHost(typeof(MyServer));
            host.AddServiceEndpoint(contract, binding, address);
            host.Open();
            Console.WriteLine("Server started...");

            Random random = new Random();
            string[] namesMan = File.ReadAllLines(@"FirstNamesMan.txt");
            string[] namesWoman = File.ReadAllLines(@"FirstNamesWoman.txt");
            var names = namesMan
                .Concat(namesWoman)
                .OrderBy(n => random.Next())
                .Take(10)
                .ToArray();

            foreach (var name in names)
            {
                Process.Start(@"..\..\..\MyClient\bin\Debug\MyClient.exe", name);
                Task.Delay(100).Wait();
            }

            Console.ReadKey(true);
            host.Close();
        }
    }
}
