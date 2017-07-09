using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceContract(CallbackContract = typeof(IChatClient))]
    public interface IChatServer
    {
        [OperationContract(IsOneWay = true)]
        void Say(string message);

        [OperationContract(IsOneWay = true)]
        void Init(string name);
    }
}
