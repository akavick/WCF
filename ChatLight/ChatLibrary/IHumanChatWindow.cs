using System.ServiceModel;

namespace ChatLibrary
{
    [ServiceContract]
    public interface IHumanChatWindow
    {
        [OperationContract]
        void Say(string message);

        [OperationContract]
        void Init(string name);
    }
}
