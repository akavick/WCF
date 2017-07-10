using System.ServiceModel;
using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    public class ChatServer : IChatServer
    {
        private IView _view;
        private IClientsManager _clientsManager;

        public IClientsManager ClientsManager
        {
            set => _clientsManager = value;
        }

        public IView View
        {
            set
            {
                _view = value;
                _view.IncomingMessage += _view_IncomingMessage;
            }
        }

        private void _view_IncomingMessage(string obj)
        {
            
        }

        public void SendToMainChat(string message)
        {
            var channel = OperationContext.Current.Channel as IDuplexContextChannel;
            var callback = OperationContext.Current.GetCallbackChannel<IChatClient>();
            var clientChannel = callback as IClientChannel;
        }

        public void SendToPerson(string message)
        {
            
        }
    }
}
