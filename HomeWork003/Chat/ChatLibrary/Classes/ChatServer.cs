using System.ServiceModel;
using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    public class ChatServer : IChat
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
                _view.IncomingMainChatMessage += _view_IncomingMessage;
                _view.IncomingPersonalChatMessage += _view_IncomingPersonalChatMessage;
            }
        }

        private void _view_IncomingPersonalChatMessage(string obj)
        {
            
        }

        private void _view_IncomingMessage(string obj)
        {
            
        }

        public void SendToMainChat(string message)
        {
            var channel = OperationContext.Current.Channel as IDuplexContextChannel;
            var callback = OperationContext.Current.GetCallbackChannel<IChat>();
            var clientChannel = callback as IClientChannel;
        }

        public void SendToPersonalChat(string message)
        {
            
        }
    }
}
