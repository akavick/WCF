using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatLibrary
{
    public class ChatServer : IChatServer
    {
        public IClientsManager ClientsManager { private get; set; }

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
