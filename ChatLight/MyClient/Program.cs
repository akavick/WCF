using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ChatLibrary;

namespace MyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = args.Any() ? args[0] : "MyClient";
            Console.Title = name;
            var client = new MyClient();

            //var address = new Uri("soap.udp://localhost:8080/");
            //var binding = new UdpBinding();

            var address = new Uri("net.tcp://localhost:8002/MyService");
            var binding = new NetTcpBinding();

            var endpoint = new EndpointAddress(address);
            var factory = new DuplexChannelFactory<IMyServer>(client, binding, endpoint);
            var channel = factory.CreateChannel();

            channel.Init(name);

            Random random = new Random();

            while (true)
            {
                try
                {
                    Task.Delay(random.Next(1000, 5001)).Wait();
                    var message = new StringBuilder("blah");
                    var count = random.Next(6);
                    for (int i = 0; i < count; i++)
                        message.Append("-blah");
                    Console.WriteLine("{0,-10}ME: {1}", DateTime.Now.ToLongTimeString(), message);
                    channel.Say(message.ToString());
                }
                catch
                {
                    Console.WriteLine("Сервер разорвал соединение...");
                    break;
                }
            }
            //Console.ReadKey(true);
        }
    }
}
