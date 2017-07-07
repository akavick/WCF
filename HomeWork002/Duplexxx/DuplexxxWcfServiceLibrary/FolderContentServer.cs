using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace DuplexxxWcfServiceLibrary
{
    public class FolderContentServer : IFolderContentServer
    {
        public void RequestContent(string path)
        {
            //проверка блокировки клиента
            Task.Delay(1000).Wait();

            IFolderContentCallback callback = null;

            try
            {
                callback = OperationContext.Current.GetCallbackChannel<IFolderContentCallback>();
                var notFound = string.Format("заданного пути не существует: {0}", path);

                if (!string.IsNullOrEmpty(path))
                {
                    if (path.Last() != '/' || path.Last() != '\\')
                        path += "/";
                }
                else
                {
                    callback.SendContent(notFound);
                    return;
                }

                var request = new DirectoryInfo(path);

                if (!request.Exists)
                {
                    callback.SendContent(notFound);
                    return;
                }

                var sb = new StringBuilder();

                foreach (var di in request.GetDirectories())
                    sb.AppendLine(di.Name);

                foreach (var fi in request.GetFiles())
                    sb.AppendLine(fi.Name);

                callback.SendContent(sb.ToString());
            }
            catch (Exception e)
            {
                try
                {
                    if (callback != null && ((IClientChannel)callback).State == CommunicationState.Opened)
                        callback.SendContent(e.Message);
                }
                catch { }
            }
        }
    }
}
