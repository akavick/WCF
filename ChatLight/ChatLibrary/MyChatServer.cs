using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;

namespace ChatLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MyChatServer : IChatServer
    {
        private static readonly Dictionary<IChatClient, string> Clients = new Dictionary<IChatClient, string>();
        private static readonly object Locker = new object();
        private static readonly object ChatLogLocker = new object();
        private static readonly object ErrorLogLocker = new object();
        private static readonly Timer Refresher = new Timer { Enabled = true, Interval = 100 };

        public MyChatServer()
        {
            Refresher.Elapsed += Refresher_Elapsed;
        }

        void Refresher_Elapsed(object sender, ElapsedEventArgs e)
        {
            Refresher.Stop();
            Console.Clear();
            try
            {
                var clientsToRemove = new List<IChatClient>();

                lock (Locker)
                {
                    foreach (var client in Clients)
                    {
                        var current = client.Key as IClientChannel;
                        if (current == null || current.State == CommunicationState.Faulted || current.State == CommunicationState.Closed)
                            clientsToRemove.Add(client.Key);
                        Console.WriteLine(client.Value);
                    }
                }

                foreach (var client in clientsToRemove)
                {
                    string name = null;
                    lock (Locker)
                    {
                        name = Clients[client];
                        Clients.Remove(client);
                    }
                    var str = string.Format("----------{0,-10}{1} отсоединился----------", DateTime.Now.ToLongTimeString(), name);
                    MassSend(null, str);
                    lock (ChatLogLocker)
                        File.AppendAllText("ChatLog.txt", str + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                Refresher.Start();
            }
        }

        public void Say(string message)
        {
            try
            {
                var sender = OperationContext.Current.GetCallbackChannel<IChatClient>();

                if (sender == null)
                    return;

                lock (Locker)
                {
                    if (!Clients.ContainsKey(sender))
                    {
                        Task.Run(() =>
                        {
                            sender.Refresh(string.Format("{0,-10}SERVER: пшёл вон, мерзкий хацкер", DateTime.Now.ToLongTimeString()));
                            var s = sender as IClientChannel;
                            if (s != null)
                                s.Close();
                            lock (ErrorLogLocker)
                                File.AppendAllText("ChatErrorLog.txt", "Пресечена попытка несанкционированного доступа" + Environment.NewLine);
                        });
                        return;
                    }
                }

                string name = null;

                lock (Locker)
                {
                    if (Clients.ContainsKey(sender))
                        name = Clients[sender];
                }

                var str = string.Format("{0,-10}{1,-15}{2}", DateTime.Now.ToLongTimeString(), name + ":", message);
                lock (ChatLogLocker)
                    File.AppendAllText("ChatLog.txt", str + Environment.NewLine);
                MassSend(sender, str);
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        private void MassSend(IChatClient sender, string message)
        {
            lock (Locker)
            {
                foreach (var client in Clients)
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            if (client.Key == sender)
                                return;
                            await SendMessage(client.Key, message);
                        }
                        catch (Exception e)
                        {
                            LogError(e);
                        }
                    });
                }
            }
        }

        public void Init(string name)
        {
            try
            {
                IChatClient callback = OperationContext.Current.GetCallbackChannel<IChatClient>();

                lock (Locker)
                {
                    if (callback != null && !Clients.ContainsKey(callback))
                        Clients.Add(callback, name);
                }

                var str = string.Format("----------{0,-10}{1} присоединился----------", DateTime.Now.ToLongTimeString(), name);
                MassSend(callback, str);
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        private async Task SendMessage(IChatClient client, string message)
        {
            await Task.Run(() =>
            {
                try
                {
                    var current = client as IClientChannel;
                    if (current != null && current.State == CommunicationState.Opened)
                        client.Refresh(message);
                }
                catch (Exception e)
                {
                    LogError(e);
                }
            });
        }

        private void LogError(Exception e)
        {
            //using (var eventLog = new EventLog("Application") { Source = "LefikoffChat" })
            //    eventLog.WriteEntry(e.ToString(), EventLogEntryType.Error);
            lock (ErrorLogLocker)
                File.AppendAllText("ChatErrorLog.txt", e + Environment.NewLine);
        }
    }
}
