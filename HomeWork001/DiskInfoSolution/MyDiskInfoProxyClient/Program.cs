using System;
using System.Threading.Tasks;
using MyDiskInfoProxyClient.MyDiskInfoServiceReference;

namespace MyDiskInfoProxyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDiskInfoServerClient client = null;

            try
            {
                Console.Title = "ProxyClient";
                Task.Delay(1000).Wait();
                client = new MyDiskInfoServerClient();

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
                        var answer = choice == '1' ? client.GetFreeSpace(query) : client.GetTotalSpace(query);
                        Console.WriteLine(answer);
                    }
                    else
                        Console.WriteLine("Некорректный ввод");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (client != null)
                    client.Close();
            }
        }
    }
}
