using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceContract]
    public interface IChatClient
    {
        [OperationContract(IsOneWay = true)]
        void Refresh(string message);
    }
}
