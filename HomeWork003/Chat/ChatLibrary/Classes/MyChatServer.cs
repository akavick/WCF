using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel;
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

        private static readonly ConcurrentDictionary<IChatCallback, string> Clients; //todo: разделить
        private static readonly object Locker = new object();

        static MyChatServer()
        {
            if (Clients == null)
                Clients = new ConcurrentDictionary<IChatCallback, string>();
        }


        private static IChatCallback Callback => OperationContext.Current.GetCallbackChannel<IChatCallback>();


        private static string GetFullMessage(IChatCallback sender, string message) 
            => $"{DateTime.Now.ToLongTimeString(),-10}{Clients[sender] + ":",-15}{message}";


        private static string GetFullMessage(string name, string message) 
            => $"{DateTime.Now.ToLongTimeString(),-10}{name + ":",-15}{message}";

        #endregion


        #region IChatContract

        public void MessageFromClientToMainChat(string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return;
                var sender = Callback;
                var msg = GetFullMessage(sender, message);

                foreach (var client in Clients.Keys)
                {
                    try
                    {
                        client.RefreshMainChat(msg);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public void MessageFromClientToPersonalChat(string reciever, string message, bool sendToSender = true)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return;
                var rec = Clients.SingleOrDefault(c => c.Value == reciever).Key;
                var sender = Callback;
                var senderName = Clients[sender];
                var msg = GetFullMessage(sender, message);
                rec.RefreshPersonalChat(senderName, msg, !sendToSender);
                if (reciever == senderName)
                    return;
                if (sendToSender)
                    sender.RefreshPersonalChat(reciever, msg);
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public void ClientIn(string name)
        {
            lock (Locker)
            {
                try
                {
                    var newClient = Callback;
                    if (Clients.ContainsKey(newClient))
                        return;
                    var success = Clients.TryAdd(newClient, name);
                    //if (!success)
                    //{
                    //    throw new Exception("Не удалось добавить клиента " + name);
                    //    //закрыть?
                    //}
                    var clientChannel = newClient as IClientChannel;
                    if (clientChannel != null)
                    {
                        clientChannel.Closed += ClientChannel_Closed;
                        clientChannel.Closing += ClientChannel_Closing;
                        clientChannel.Faulted += ClientChannel_Faulted;
                    }

                    foreach (var client in Clients.Keys)
                    {
                        try
                        {
                            if (client == newClient)
                                client.FullRefreshClientList(Clients.Values.ToArray());
                            else
                            {
                                client.RefreshClientList(name);
                            }
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }

                    var enterMessage = $"***** {name} входит в чат. *****";
                    MessageFromClientToMainChat(enterMessage);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }


        private void ClientChannel_Faulted(object sender, EventArgs e)
        {
            ClientOut();
        }


        private void ClientChannel_Closing(object sender, EventArgs e)
        {
            ClientOut();
        }


        private void ClientChannel_Closed(object sender, EventArgs e)
        {
            ClientOut();
        }


        public void ClientOut()
        {
            try
            {
                var exitingClient = Callback;
                if (!Clients.ContainsKey(exitingClient))
                    return;
                var quitMessage = $"***** {Clients[exitingClient]} покидает чат. *****";
                MessageFromClientToMainChat(quitMessage);

                string exitedClientName;
                var success = Clients.TryRemove(exitingClient, out exitedClientName);
                //закрыть?
                if (!success)
                    throw new Exception($"Не удалось удалить клиента {exitedClientName}");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

    }
}
