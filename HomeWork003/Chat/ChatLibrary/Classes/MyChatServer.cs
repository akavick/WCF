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

        private static readonly ConcurrentDictionary<IChatCallback, string> Clients; //todo: разделить
        private static readonly ConcurrentDictionary<string, IChatCallback> Names;

        static MyChatServer()
        {
            if (Clients == null)
                Clients = new ConcurrentDictionary<IChatCallback, string>();
            if (Names == null)
                Names = new ConcurrentDictionary<string, IChatCallback>();
        }

        private static IChatCallback GetCallback()
        {
            return OperationContext.Current.GetCallbackChannel<IChatCallback>();
        }

        private static string GetFullMessage(IChatCallback sender, string message)
        {
            var time = DateTime.Now.ToLongTimeString();
            var name = Clients[sender];
            return $"{time,-10}{name + ":",-15}{message}";
        }

        private static string GetFullMessage(string name, string message)
        {
            var time = DateTime.Now.ToLongTimeString();
            return $"{time,-10}{name + ":",-15}{message}";
        }




        #region IChatContract

        public void MessageFromClientToMainChat(string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return;
                var sender = GetCallback();
                var msg = GetFullMessage(sender, message);

                foreach (var client in Clients)
                {
                    try
                    {
                        client.Key.RefreshMainChat(msg);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void MessageFromClientToPersonalChat(string reciever, string message, bool sendToSender = true)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    return;
                var rec = Names[reciever];
                var sender = GetCallback();
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

            }
        }

        public void ClientIn(string name)
        {
            try
            {
                var newClient = GetCallback();
                if (Clients.ContainsKey(newClient) || Names.ContainsKey(name))
                    return;
                var success = Clients.TryAdd(newClient, name) && Names.TryAdd(name, newClient);
                if (!success)
                {
                    IChatCallback client;
                    Names.TryRemove(name, out client);
                    string errorClientName;
                    Clients.TryRemove(newClient, out errorClientName);
                    throw new Exception("Не удалось добавить клиента " + name);
                    //закрыть?
                }
                var clientChannel = newClient as IClientChannel;
                if (clientChannel != null)
                {
                    clientChannel.Closed += ClientChannel_Closed;
                    clientChannel.Closing += ClientChannel_Closing;
                    clientChannel.Faulted += ClientChannel_Faulted;
                }

                foreach (var client in Clients)
                {
                    try
                    {
                        if (client.Key == newClient)
                            client.Key.FullRefreshClientList(Names.Keys.ToArray());
                        else
                            client.Key.RefreshClientList(name);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception e)
            {

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
                var exitingClient = GetCallback();
                if (!Clients.ContainsKey(exitingClient))
                    return;
                var quitMessage = $"***** {Clients[exitingClient]} покинул беседу. *****";
                MessageFromClientToMainChat(quitMessage);

                IChatCallback client;
                string exitedClientName;
                var success = Clients.TryRemove(exitingClient, out exitedClientName) && Names.TryRemove(exitedClientName, out client);
                //закрыть?
                if (!success)
                    throw new Exception($"Не удалось удалить клиента {exitedClientName}");
            }
            catch (Exception e)
            {

            }
        }

        #endregion

    }
}
