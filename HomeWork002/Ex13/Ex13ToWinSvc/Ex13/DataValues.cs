using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Ex13
{
    /*����� SendTimeToCLient() ����� ������ ����� ��������� ������� � ����� ������. �� �������� �� ������ ��������� ������ � ������ ���������� ������� ��������� � �������������� period �����������, ����� ����� ��������� �� �������� number. ������ �� ���� ��������� ������������ ������� ������� ReceiveTime() */

    public class DataValues
    {
        //���� ���� ��������� ��������� ������
        public IClientCallback Callback = null;

        public void SendTimeToCLient(object data)
        {
            //�� ������� ������ ���� ��������� �� ������ ��������� ������
            int s = /*60 - DateTime.Now.Second*/ 0; // ���������
            Thread.Sleep(s * 1000);
            DateTime start = DateTime.Now;
            //������� �� object data ���� ��� ��������� ���� int,
            List<int> parameters = (List<int>)data;
            int period = parameters[0];
            int number = parameters[1];
            //������ ��������� ������� ��������� � �����
            for (int i = 0;i < number;i++)
            {
                try
                {
                    //�������� ����� ����������� period ������
                    Thread.Sleep(period * 1000);
                    TimeSpan result = DateTime.Now - start;
                    TimeSpan r = result.Add(new TimeSpan(0, 0, s));
                    Callback.ReceiveTime(string.Format("{0} ����� ������ �� ������� - {1}:{2}", DateTime.Now.ToLongTimeString(), r.Minutes, r.Seconds));
                }
                catch(Exception e)
                {
                    using (var eventLog = new EventLog("Application") {Source = "Ex13DuplexSvc" })
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error);
                }
            }
        }
    }
}