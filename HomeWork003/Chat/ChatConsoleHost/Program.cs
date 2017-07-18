using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ChatLibrary.Classes;
using ChatLibrary.Interfaces;

namespace ChatConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SERVER";
            ServiceHost sh = new ServiceHost(typeof(MyChatServer));
            sh.Open();
            Console.ReadKey(true);
            sh.Close();
        }
    }
}
