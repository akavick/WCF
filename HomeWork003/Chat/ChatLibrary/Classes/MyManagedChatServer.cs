using System.ServiceModel;
using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    public class MyManagedChatServer : IManagedChat
    {
        private IChatClientsManager _clientsManager;

        public IChatClientsManager ClientsManager
        {
            get => _clientsManager;
            set => _clientsManager = value;
        }

        public void SendToMainChat(string message)
        {
            var channel = OperationContext.Current.Channel as IDuplexContextChannel;
            var callback = OperationContext.Current.GetCallbackChannel<IChatContract>();
            var clientChannel = callback as IClientChannel;
        }

        public void SendToPersonalChat(string message)
        {
            
        }
    }
}
