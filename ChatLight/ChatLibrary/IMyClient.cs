using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceContract]
    public interface IMyClient
    {
        [OperationContract(IsOneWay = true)]
        void Refresh(string message);
    }
}
