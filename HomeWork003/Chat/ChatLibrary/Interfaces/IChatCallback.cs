using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract]
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshMainChat(string message);

        [OperationContract(IsOneWay = true)]
        void RefreshPersonalChat(string name, string message, bool finished = false);

        [OperationContract(IsOneWay = true)]
        void RefreshClientList(string name, bool quitted = false);

        [OperationContract(IsOneWay = true)]
        void FullRefreshClientList(string[] names);
    }
}