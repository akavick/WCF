using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Ex13
{
    /*метод SendTimeToCLient() будет вызван после обращения клиента к нашей службе. Он подождет до начала следующей минуты и начнет отправлять клиенту сообщения с периодичностью period миллисекунд, всего таких сообщений он отправит number. Каждое из этих сообщений доставляется клиенту методом ReceiveTime() */

    public class DataValues
    {
        //член типа контракта обратного вызова
        public IClientCallback Callback = null;

        public void SendTimeToCLient(object data)
        {
            //по условию задачи надо подождать до начала следующей минуты
            int s = /*60 - DateTime.Now.Second*/ 0; // надоедает
            Thread.Sleep(s * 1000);
            DateTime start = DateTime.Now;
            //достать из object data наши два параметра типа int,
            List<int> parameters = (List<int>)data;
            int period = parameters[0];
            int number = parameters[1];
            //каждое сообщение клиенту готовится в цикле
            for (int i = 0;i < number;i++)
            {
                try
                {
                    //задержка между сообщениями period секунд
                    Thread.Sleep(period * 1000);
                    TimeSpan result = DateTime.Now - start;
                    TimeSpan r = result.Add(new TimeSpan(0, 0, s));
                    Callback.ReceiveTime(string.Format("{0} время работы со службой - {1}:{2}", DateTime.Now.ToLongTimeString(), r.Minutes, r.Seconds));
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