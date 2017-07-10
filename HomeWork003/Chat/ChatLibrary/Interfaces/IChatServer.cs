using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IChatClient))]
    public interface IChatServer
    {
        [OperationContract(IsOneWay = true)]
        void SendToMainChat(string message);

        [OperationContract(IsOneWay = true)]
        void SendToPerson(string message);
    }
}
