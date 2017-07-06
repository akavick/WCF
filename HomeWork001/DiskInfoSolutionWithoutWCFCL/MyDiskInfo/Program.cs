using System;
using System.ServiceModel;
using Library;

namespace MyDiskInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = null;

            try
            {
                Console.Title = "SERVER";
                sh = new ServiceHost(typeof(MyDiskInfoServer));
                sh.Open();
                Console.Write("Для завершения нажмите <ENTER>\n");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (sh != null)
                    sh.Close();
            }
        }
    }
}
