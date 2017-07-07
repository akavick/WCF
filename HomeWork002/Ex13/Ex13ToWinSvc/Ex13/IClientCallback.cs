using System.ServiceModel;

namespace Ex13
{

    /*это втора€ часть контракта службы Ц контракт обратного вызова, метод ReceiveTime() описан на стороне клиента и может вызыватьс€ службой, при вызове служба передает клиенту информацию в string str*/
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveTime(string str);
    }
}