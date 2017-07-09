using System;
using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HumanChatWindow : IHumanChatWindow
    {
        private readonly IChatServer _server;
        private string _name;

        public HumanChatWindow(IChatServer server)
        {
            _server = server;
        }

        public void Say(string message)
        {
            try
            {
                _server.Say(message);
                Console.WriteLine("{0,-10}ME: {1}", DateTime.Now.ToLongTimeString(), message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Init(string name)
        {
            try
            {
                _name = name;
                Console.Title = (_name ?? "User") + " Chat Window";
                _server.Init(_name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}