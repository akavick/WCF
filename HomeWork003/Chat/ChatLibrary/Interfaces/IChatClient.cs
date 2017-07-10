using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract]
    public interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void Send(string message);
    }
}