using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ChatLibrary;

namespace HumanConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите имя:");
            var name = Console.ReadLine();
            Console.Title = name ?? "User";

            var address = new Uri("net.tcp://localhost:8003/MyChatWindow");
            var binding = new NetTcpBinding();
            var endpoint = new EndpointAddress(address);
            var factory = new ChannelFactory<IHumanChatWindow>(binding, endpoint);
            var channel = factory.CreateChannel();
            channel.Init(name);

            Console.WriteLine("Приложение готово к работе, введите сообщение:");

            while (true)
            {
                try
                {
                    var message = Console.ReadLine();
                    channel.Say(message);
                    Console.Clear();
                    Console.WriteLine("Введите сообщение:");
                }
                catch
                {
                    Console.WriteLine("Сервер разорвал соединение...");
                    break;
                }
            }

            Console.ReadKey(true);
        }
    }
}
