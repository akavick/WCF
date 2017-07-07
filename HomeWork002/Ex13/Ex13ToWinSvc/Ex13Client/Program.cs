using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Ex13;

namespace Ex13Client
{
    class Program
    {
        static void Main(string[] args)
        {
            DuplexChannelFactory<IDuplexSvc> factory = null;

            try
            {
                Console.Title = "ChannelClient";
                Task.Delay(1000).Wait();
                var client = new Ex13ClientCallback();
                factory = new DuplexChannelFactory<IDuplexSvc>(client, "MyEndPoint");
                var channel = factory.CreateChannel();

                Console.WriteLine("Ожидается ответ:");
                channel.ReturnTime(1, 10);
                Console.ReadKey(true);
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
