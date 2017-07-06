using System;
using System.Collections.Generic;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
