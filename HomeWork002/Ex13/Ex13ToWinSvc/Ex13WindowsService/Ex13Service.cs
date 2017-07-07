using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Ex13;

namespace Ex13WindowsService
{
    public partial class Ex13Service : ServiceBase
    {
        internal static ServiceHost MyServiceHost = null;
        public Ex13Service()
        {
            InitializeComponent();
            this.ServiceName = "Ex13DuplexWinSvc";
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (MyServiceHost != null)
                    MyServiceHost.Close();

                MyServiceHost = new ServiceHost(typeof(DuplexSvc));
                MyServiceHost.Open();
            }
            catch (Exception e)
            {
                this.EventLog.WriteEntry(e.ToString(), EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (MyServiceHost != null)
                {
                    MyServiceHost.Close();
                    MyServiceHost = null;
                }
            }
            catch (Exception e)
            {
                this.EventLog.WriteEntry(e.ToString(), EventLogEntryType.Error);
            }
        }
    }
}
