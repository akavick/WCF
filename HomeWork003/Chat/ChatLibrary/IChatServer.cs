using System.ServiceModel;

namespace ChatLibrary
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
