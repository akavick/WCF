using System;
using System.IO;
using System.Linq;

namespace Library
{
    public class MyDiskInfoServer : IMyDiskInfoServer
    {
        public string GetFreeSpace(string diskName)
        {
            string toReturn = GetSomeDriveInfo(diskName, "AvailableFreeSpace");
            return toReturn;
        }

        public string GetTotalSpace(string diskName)
        {
            string toReturn = GetSomeDriveInfo(diskName, "TotalSize");
            return toReturn;
        }

        private static string GetSomeDriveInfo(string diskName, string propertyName)
        {
            try
            {
                if (string.IsNullOrEmpty(diskName) || !char.IsLetter(diskName[0]))
                    return "Введите корректное имя диска";

                var driveLetter = char.ToLower(diskName[0]);
                var drives = DriveInfo.GetDrives();
                var targetDrive = drives.SingleOrDefault(d => char.ToLower(d.Name[0]) == driveLetter);

                if (targetDrive == null)
                    return "Запрошенный диск не найден";

                var property = targetDrive.GetType().GetProperty(propertyName);
                var result = property.GetValue(targetDrive);

                return string.Format("{0} байт", result);
            }
            catch (Exception e)
            {
                var message = e.InnerException != null ? e.InnerException.Message : e.Message;
                Console.WriteLine("diskName: {0}{1}propertyName: {2}{1}errorBody: {3}{1}", diskName, Environment.NewLine, propertyName, message);
                return string.Format("Произошла внутренняя ошибка: {0}", message);
            }
        }
    }
}
