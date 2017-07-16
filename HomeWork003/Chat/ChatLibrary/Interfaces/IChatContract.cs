using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChatContract
    {
        [OperationContract(IsOneWay = true, Name = "SendToMainChat")]
        void MessageFromClientToMainChat(string message);

        [OperationContract(IsOneWay = true, Name = "SendToPersonalChat")]
        void MessageFromClientToPersonalChat(string reciever, string message, bool sendToSender = true);

        [OperationContract(IsOneWay = true, Name = "IamIn")]
        void ClientIn(string name);
    }
}