using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceContract(CallbackContract = typeof(IMyClient))]
    public interface IMyServer
    {
        [OperationContract(IsOneWay = true)]
        void Say(string message);

        [OperationContract(IsOneWay = true)]
        void Init(string name);
    }
}
