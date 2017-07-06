using System.ServiceModel;

namespace Library
{
    [ServiceContract]
    public interface IMyDiskInfoServer
    {
        [OperationContract]
        string GetFreeSpace(string diskName);

        [OperationContract]
        string GetTotalSpace(string diskName);
    }
}
