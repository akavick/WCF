using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;

namespace DuplexxxWcfServiceLibrary
{
    public class FolderContentServer : IFolderContentServer
    {
        public void RequestContent(string path)
        {
            try
            {
                var callback = OperationContext.Current.GetCallbackChannel<IFolderContentCallback>();
                var requestedDirectoryInfo = new DirectoryInfo(path);

                if (!requestedDirectoryInfo.Exists)
                {
                    callback.SendContent(string.Format("заданного пути не существует: {0}", path));
                    return;
                }

                var sb = new StringBuilder();

                foreach (var directoryInfo in requestedDirectoryInfo.GetDirectories())
                {
                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
