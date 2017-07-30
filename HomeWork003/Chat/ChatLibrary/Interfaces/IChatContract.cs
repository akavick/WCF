using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IChatCallback))]
    public interface IChatContract
    {
        [OperationContract(IsOneWay = true, Name = "SendToMainChat")]
        void MessageFromClientToMainChat(string sender, byte[] message);

        [OperationContract(IsOneWay = true, Name = "SendToPersonalChat")]
        void MessageFromClientToPersonalChat(string sender, string reciever, byte[] message);

        [OperationContract(IsOneWay = true, Name = "IamIn")]
        void ClientIn(string name);
    }
}