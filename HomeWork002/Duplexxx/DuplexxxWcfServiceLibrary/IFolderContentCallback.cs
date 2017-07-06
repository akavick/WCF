using System.ServiceModel;

namespace DuplexxxWcfServiceLibrary
{
    [ServiceContract]
    public interface IFolderContentCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendContent(string content);
    }
}