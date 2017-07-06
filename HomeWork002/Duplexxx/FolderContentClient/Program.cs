using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DuplexxxWcfServiceLibrary;

namespace FolderContentClient
{
    class Program
    {
        static void Main(string[] args)
        {
            DuplexChannelFactory<IFolderContentServer> factory = null;
            
            try
            {
                Console.Title = "ChannelClient";
                Task.Delay(1000).Wait();
                var client = new FolderContentClient();
                factory = new DuplexChannelFactory<IFolderContentServer>(client, "FolderContentServerEndPoint");
                var channel = factory.CreateChannel();

                Console.WriteLine("Вводите путь:");
                while (true)
                {
                    var path = Console.ReadLine();
                    channel.RequestContent(path);
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
