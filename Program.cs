using GarminMtpDataBackupper.Config;
using System;

namespace GarminMtpDataBackupper
{
    class Program
    {
        static void Main(string[] _)
        {
            var configReader = new ConfigReader();
            var config = configReader.GetConfig();

            var device = new Device(config);

            var dataBackuper = new DataBackupper(config, device);
            dataBackuper.BackupGarminData();

            Console.WriteLine("Please, press Enter to finish.");
            Console.ReadLine();
        }
    }
}
