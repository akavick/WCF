using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceContract]
    public interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void Send(string message);
    }
}