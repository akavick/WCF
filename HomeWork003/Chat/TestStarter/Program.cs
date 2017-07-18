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
        private readonly IChatContract _server;
        private readonly string _name;

        public FakeClient(string name)
        {
            _name = name;
            var ic = new InstanceContext(this);
            _server = new ChatContractClient(ic);
            _server.IamIn(_name);
        }

        public void RefreshMainChat(string message)
        {
            //Console.WriteLine($"{_name}: {message}");
        }

        public void RefreshPersonalChat(string name, string message, bool finished)
        {
            //Console.WriteLine($"{name}: {message}");
        }

        public void RefreshClientList(string name, bool quitted)
        {
            //lock (Locker)
            //{
            //    Console.WriteLine($"RefreshLIST for {_name}: {name}");
            //}
        }

        public void FullRefreshClientList(string[] names)
        {
            //lock (Locker)
            //{
            //    Console.WriteLine($"LIST for {_name}");
            //    foreach (var name in names)
            //    {
            //        Console.WriteLine(name);
            //    }
            //    Console.WriteLine();
            //}
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
                .ToArray();

            Process.Start(@"..\..\..\WpfChatClient\bin\Debug\WpfChatClient.exe", "AKAVICK");
            Task.Delay(1000).Wait();

            List<FakeClient> clients = new List<FakeClient>();
            foreach (var name in names)
            {
                clients.Add(new FakeClient(name));
                //Console.WriteLine(name);
                //Process.Start(@"..\..\..\WpfChatClient\bin\Debug\WpfChatClient.exe", name);
                //Task.Delay(1000).Wait();
            }
            


            Console.ReadKey(true);
        }
    }
}
