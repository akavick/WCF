using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceContract(CallbackContract = typeof(IChatClient))]
    public interface IChatServer
    {
        IClientsManager ConnectionManager { get; set; }

        [OperationContract(IsOneWay = true)]
        void Send(string message);
    }
}
