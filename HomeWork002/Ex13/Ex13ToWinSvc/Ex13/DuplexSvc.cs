using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;

namespace Ex13
{
    public class DuplexSvc : IDuplexSvc
    {
        /*метод ReturnTime() каждый раз запускается в новом потоке, для запуска потока используется делегат ParameterizedThreadStart, способный передать один параметер типа object, но поскольку нам надо передать в поток два параметра типа int, мы упаковываем их в List<int> и в таком виде передаем методу потока SendTimeToCLient() */
        public void ReturnTime(int period, int number)
        {
            try
            {
                DataValues src = new DataValues { Callback = OperationContext.Current.GetCallbackChannel<IClientCallback>() };
                Thread t = new Thread(src.SendTimeToCLient) { IsBackground = true };
                List<int> parameters = new List<int> { period, number };
                t.Start(parameters);
            }
            catch (Exception e)
            {
                using (var eventLog = new EventLog("Application") { Source = "Ex13DuplexSvc" })
                    eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error);
            }
        }
    }

}
