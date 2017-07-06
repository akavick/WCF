using System;

namespace DuplexxxWcfServiceLibrary
{
    class FolderContentCallback : IFolderContentCallback
    {
        public void SendContent(string content)
        {
            Console.WriteLine(content);
        }
    }
}