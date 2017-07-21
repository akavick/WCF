using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TestStarter.ChatServiceReference;

namespace TestStarter
{
    class FakeClient : IChatContractCallback
    {
        #region Init

        public readonly IChatContract _server;
        public readonly string _name;

        public FakeClient(string name)
        {
            _name = name;
            var ic = new InstanceContext(this);
            _server = new ChatContractClient(ic);
            _server.IamIn(_name);
        }

        #endregion

        #region NotUsed

        public void RefreshMainChat(string message)
        {

        }

        public void RefreshClientList(string name, bool quitted)
        {

        }

        public void FullRefreshClientList(string[] names)
        {

        }

        #endregion

        public void RefreshPersonalChat(string name, string message, bool finished)
        {
            _server.SendToPersonalChat(_name, name, "ответь", false);
        }
    }



    class Program
    {
        static void Main(string[] args)
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

            List<FakeClient> clients = new List<FakeClient>();
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
                    await Task.Delay(random.Next(50, 5001));
                    var client = clients[random.Next(clients.Count)];
                    client._server.SendToMainChat(client._name, "blah-blah");
                }
            });

            Process.Start(@"..\..\..\WpfChatClient\bin\Debug\WpfChatClient.exe", "anyUser");


            Console.ReadKey(true);
        }
    }
}
