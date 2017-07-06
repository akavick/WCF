using System;
using System.Collections.Generic;
using ChatLibrary;
using System.ServiceModel;
using System.Threading.Tasks;

namespace MyServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MyServer : IMyServer
    {
        private static readonly Dictionary<IMyClient, string> Clients = new Dictionary<IMyClient, string>();
        private static readonly object Locker = new object();

        public void Say(string message)
        {
            try
            {
                var sender = OperationContext.Current.GetCallbackChannel<IMyClient>();

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
                MassSend(sender, str);
                Console.WriteLine(str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void MassSend(IMyClient sender, string message)
        {
            lock (Locker)
            {
                foreach (var client in Clients)
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            if (sender == null || client.Key == sender)
                                return;
                            await SendMessage(client.Key, message);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    });
                }
            }
        }

        public void Init(string name)
        {
            try
            {
                IMyClient callback = OperationContext.Current.GetCallbackChannel<IMyClient>();

                lock (Locker)
                {
                    if (callback != null && !Clients.ContainsKey(callback))
                        Clients.Add(callback, name);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task SendMessage(IMyClient client, string message)
        {
            await Task.Run(() =>
            {
                try
                {
                    client.Refresh(message);
                }
                catch
                {
                    try
                    {
                        string name = null;

                        lock (Locker)
                        {
                            name = Clients[client];
                            Clients.Remove(client);
                        }

                        var str = string.Format("{0,-10}{1} отсоединился", DateTime.Now.ToLongTimeString(), name);
                        Console.WriteLine(str);
                        MassSend(null, str);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                }
            });
        }
    }
}
