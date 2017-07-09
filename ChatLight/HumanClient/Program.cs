using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ChatLibrary;

namespace HumanClient
{
    class Program
    {
        static void Main()
        {
            Console.Title = "UserChatWindow";

            var client = new MyChatClient();
            var address = new Uri("net.tcp://localhost:8002/MyChat");
            var binding = new NetTcpBinding();
            var endpoint = new EndpointAddress(address);
            var factory = new DuplexChannelFactory<IChatServer>(client, binding, endpoint);
            var channel = factory.CreateChannel();
            //((IDuplexContextChannel) channel).Open();

            var window = new HumanChatWindow(channel);
            var chatWindowAddress = new Uri("net.tcp://localhost:8003/MyChatWindow");
            var chatWindowBinding = new NetTcpBinding();
            var chatWindowContract = typeof(IHumanChatWindow);
            var chatWindowhost = new ServiceHost(window);
            chatWindowhost.AddServiceEndpoint(chatWindowContract, chatWindowBinding, chatWindowAddress);
            chatWindowhost.Open();

            Process.Start(@"..\..\..\HumanConsole\bin\Debug\HumanConsole.exe");

            Console.ReadKey(true);

            chatWindowhost.Close();
            factory.Close();
        }
    }
}
