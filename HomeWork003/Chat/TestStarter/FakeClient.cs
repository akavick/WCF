using System.ServiceModel;
using System.Text;
using TestStarter.ChatServiceReference;

namespace TestStarter
{
    internal class FakeClient : IChatContractCallback
    {
        #region Init

        public readonly IChatContract Server;
        public readonly string Name;

        public FakeClient(string name)
        {
            Name = name;
            var ic = new InstanceContext(this);
            Server = new ChatContractClient(ic);
            Server.IamIn(Name);
        }

        #endregion

        #region NotUsed

        public void RefreshMainChat(string name, byte[] message)
        {

        }


        public void RefreshClientList(string name, bool quitted)
        {

        }

        public void FullRefreshClientList(string[] names)
        {

        }

        #endregion

        public void RefreshPersonalChat(string sender, string reciever, byte[] message)
        {
            Server.SendToPersonalChat(Name, sender, Encoding.UTF8.GetBytes("ответь"));
        }
    }
}