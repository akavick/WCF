using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using ChatLibrary.Interfaces;

namespace ChatLibrary.Classes
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MyChatServer : IChatContract
    {
        //public void SendToMainChat(string message)
        //{
        //    var channel = OperationContext.Current.Channel as IDuplexContextChannel;
        //    var callback = OperationContext.Current.GetCallbackChannel<IChatContract>();
        //    var clientChannel = callback as IClientChannel;
        //}

        #region Static

        private static readonly ConcurrentDictionary<string, IChatCallback> Clients; //todo: разделить
        private static readonly object Locker = new object();

        static MyChatServer()
        {
            if (Clients == null)
                Clients = new ConcurrentDictionary<string, IChatCallback>();
        }

        private static IChatCallback GetCallback()
        {
            try
            {
                return OperationContext.Current.GetCallbackChannel<IChatCallback>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static string GetFullMessage(IChatCallback sender, string message)
        {
            try
            {
                return $"{DateTime.Now.ToLongTimeString(),-10}{Clients.Single(c => c.Value == sender).Key + ":",-15}{message}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        private static string GetFullMessage(string name, string message)
        {
            try
            {
                return $"{DateTime.Now.ToLongTimeString(),-10}{name + ":",-15}{message}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static async Task SendMassMessage(IChatCallback sender, string message)
        {
            await Task.Run(() =>
            {
                foreach (var client in Clients.Values.ToArray())
                {
                    try
                    {
                        client.RefreshMainChat(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        try
                        {
                            Console.WriteLine(Clients.Single(c => c.Value == client).Key);
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
            });
        }


        private static async Task SendSingleMessage(IChatCallback sender, string message)
        {
            await Task.Run(() =>
            {

                //client.RefreshMainChat(message);

            });
        }

        #endregion


        #region IChatContract

        public void MessageFromClientToMainChat(string sender, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return;
                var sen= GetCallback();
                var msg = GetFullMessage(sender, message);
                var task = SendMassMessage(sen, msg);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public void MessageFromClientToPersonalChat(string sender, string reciever, string message, bool sendToSender = true)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return;
                var sen = GetCallback();
                var rec = Clients[reciever];
                var msg = GetFullMessage(sender, message);
                rec.RefreshPersonalChat(sender, msg, !sendToSender);
                if (reciever == sender)
                    return;
                if (sendToSender)
                    sen.RefreshPersonalChat(reciever, msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public void ClientIn(string name)
        {
            //lock (Locker)
            try
            {
                Console.WriteLine(Clients.Count);

                if (Clients.ContainsKey(name))
                {
                    Console.WriteLine("уже есть");
                    return;
                }

                var newClient = GetCallback();
                if (!Clients.TryAdd(name, newClient))
                {
                    Console.WriteLine("Не удалось добавить клиента");
                    return;
                }
                //if (!success)
                //{
                //    throw new Exception("Не удалось добавить клиента " + name);
                //    //закрыть?
                //}
                //var clientChannel = newClient as IClientChannel;

                //if (clientChannel != null)
                //{
                //    clientChannel.Closed += ClientChannel_Closed;
                //    clientChannel.Closing += ClientChannel_Closing;
                //    clientChannel.Faulted += ClientChannel_Faulted;
                //}

                foreach (var client in Clients.Values.ToArray())
                {
                    try
                    {
                        if (client == newClient)
                            client.FullRefreshClientList(Clients.Keys.ToArray());
                        else
                        {
                            client.RefreshClientList(name);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        try
                        {
                            Console.WriteLine(Clients.Single(c => c.Value == client).Key);
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }

                var enterMessage = $"***** {name} входит в чат. *****";
                MessageFromClientToMainChat(name, enterMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        //private void ClientChannel_Faulted(object sender, EventArgs e)
        //{
        //    //ClientOut();
        //}


        //private void ClientChannel_Closing(object sender, EventArgs e)
        //{
        //    //ClientOut();
        //}


        //private void ClientChannel_Closed(object sender, EventArgs e)
        //{
        //    //ClientOut();
        //}


        //public void ClientOut()
        //{
        //    try
        //    {
        //        var exitingClient = GetCallback();
        //        if (!Clients.Values.ToArray().Contains(exitingClient))
        //            return;
        //        var quitMessage = $"***** {Clients[exitingClient]} покидает чат. *****";
        //        MessageFromClientToMainChat(quitMessage);

        //        string exitedClientName;
        //        var success = Clients.TryRemove(exitingClient, out exitedClientName);

        //        var clientChannel = exitingClient as IClientChannel;
        //       // clientChannel?.Close();

        //        if (!success)
        //            throw new Exception($"Не удалось удалить клиента {exitedClientName}");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

        #endregion

    }
}
