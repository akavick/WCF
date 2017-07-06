using System.ServiceModel;

namespace DuplexxxWcfServiceLibrary
{
    [ServiceContract(CallbackContract = typeof(IFolderContentCallback))]
    public interface IFolderContentServer
    {
        [OperationContract(IsOneWay = true)]
        void RequestContent(string path);
    }
}
