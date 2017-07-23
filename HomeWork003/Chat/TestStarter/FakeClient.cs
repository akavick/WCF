using System.ServiceModel;
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
            Server.SendToPersonalChat(Name, name, "ответь", false);
        }
    }
}