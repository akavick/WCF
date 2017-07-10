using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IChat))]
    public interface IChat
    {
        [OperationContract(IsOneWay = true)]
        void SendToMainChat(string message);

        [OperationContract(IsOneWay = true)]
        void SendToPersonalChat(string message);
    }
}