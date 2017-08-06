using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStarter
{
    class Program
    {
        static void Main()
        {
            Console.Title = "TESTRUNNER";

            var random = new Random();
            var namesMan = File.ReadAllLines(@"..\..\FirstNamesMan.txt");
            var namesWoman = File.ReadAllLines(@"..\..\FirstNamesWoman.txt");
            var names = namesMan
                .Concat(namesWoman)
                .OrderBy(n => random.Next())
                .Take(10)
                .ToArray();

            Task.Delay(2000).Wait();

            var clients = new List<FakeClient>();
            foreach (var name in names)
            {
                clients.Add(new FakeClient(name));
                //Console.WriteLine(name);
                //Task.Delay(1000).Wait();
            }

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(random.Next(50, 51));
                    var client = clients[random.Next(clients.Count)];
                    client.Server.SendToMainChat(client.Name, Encoding.UTF8.GetBytes("blah-blah"/*new string('a', 10000)*/));
                }
                // ReSharper disable once FunctionNeverReturns
            });

            //Process.Start(@"..\..\..\WpfChatClient\bin\Debug\WpfChatClient.exe", "anyUser");


            Console.ReadKey(true);
        }
    }
}
