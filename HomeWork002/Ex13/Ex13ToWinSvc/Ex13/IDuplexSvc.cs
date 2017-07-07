using System.ServiceModel;

namespace Ex13
{
    /*это контракт службы, метод ReturnTime() может вызываться клиентом, при вызове клиент передает службе period – периодичность, с которой клиент хочет получать сообщения службы и number – количество этих сообщений. Чтобы не блокировать клиента, метод объявлен как односторонний. Обратите внимание на атрибут, который указывает, что контракт имеет callback составляющую*/
    [ServiceContract(CallbackContract = typeof(IClientCallback))]
    public interface IDuplexSvc
    {
        [OperationContract(IsOneWay = true)]
        void ReturnTime(int period, int number);
    }

 }
