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
        static void Main(string[] args)
        {
            Task.Delay(2000).Wait();

            var random = new Random();
            var namesMan = File.ReadAllLines(@"..\..\FirstNamesMan.txt");
            var namesWoman = File.ReadAllLines(@"..\..\FirstNamesWoman.txt");
            var names = namesMan
                .Concat(namesWoman)
                .OrderBy(n => random.Next())
                .Take(3)
                .ToArray();

            foreach (var name in names)
            {
                Process.Start(@"..\..\..\WpfChatClient\bin\Debug\WpfChatClient.exe", name);
                Task.Delay(100).Wait();
            }
        }
    }
}
