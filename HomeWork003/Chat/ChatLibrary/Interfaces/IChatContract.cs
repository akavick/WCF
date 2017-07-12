using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IChatContract))]
    public interface IChatContract
    {
        [OperationContract(IsOneWay = true)]
        void SendToMainChat(string message);

        [OperationContract(IsOneWay = true)]
        void SendToPersonalChat(string message);
    }
}