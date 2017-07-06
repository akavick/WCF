using System;
using DuplexxxWcfServiceLibrary;

namespace FolderContentClient
{
    public class FolderContentClient : IFolderContentCallback
    {
        public void SendContent(string content)
        {
            Console.WriteLine(content);
        }
    }
}