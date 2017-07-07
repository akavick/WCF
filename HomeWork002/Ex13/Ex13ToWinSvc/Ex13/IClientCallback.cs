using System.ServiceModel;

namespace Ex13
{

    /*��� ������ ����� ��������� ������ � �������� ��������� ������, ����� ReceiveTime() ������ �� ������� ������� � ����� ���������� �������, ��� ������ ������ �������� ������� ���������� � string str*/
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveTime(string str);
    }
}