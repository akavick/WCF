using System.ServiceModel;
using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MyChatServer : IChatContract
    {
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
