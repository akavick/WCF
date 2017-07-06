using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Library;

namespace MyDiskInfoChannelClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IMyDiskInfoServer> factory = null;

            try
            {
                Console.Title = "ChannelClient";
                Task.Delay(1000).Wait();
                factory = new ChannelFactory<IMyDiskInfoServer>("MyDiskInfoServerEndPoint");
                IMyDiskInfoServer channel = factory.CreateChannel();

                while (true)
                {
                    Console.WriteLine("Выберите действие или CTRL+C для выхода");
                    Console.WriteLine("\t1 - получить объём свободного места на диске");
                    Console.WriteLine("\t2 - получить объём всего места на диске");
                    var choice = Console.ReadKey(true).KeyChar;
                    if (choice == '1' || choice == '2')
                    {
                        Console.WriteLine("Введите запрос или CTRL+C для выхода");
                        var query = Console.ReadLine();
                        var answer = choice == '1' ? channel.GetFreeSpace(query) : channel.GetTotalSpace(query);
                        Console.WriteLine(answer);
                    }
                    else Console.WriteLine("Некорректный ввод");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (factory != null)
                    factory.Close();
            }
        }
    }
}
