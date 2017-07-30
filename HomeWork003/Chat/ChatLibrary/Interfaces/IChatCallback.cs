using System.ServiceModel;

namespace ChatLibrary.Interfaces
{
    [ServiceContract]
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshMainChat(string name, byte[] message);

        [OperationContract(IsOneWay = true)]
        void RefreshPersonalChat(string sender, string reciever, byte[] message);

        [OperationContract(IsOneWay = true)]
        void RefreshClientList(string name, bool quitted = false);

        [OperationContract(IsOneWay = true)]
        void FullRefreshClientList(string[] names);
    }
}