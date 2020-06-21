using GarminMtpDataBackupper.Config;
using System;
using System.Reflection;

namespace GarminMtpDataBackupper
{
    class Program
    {
        static void Main(string[] _)
        {
            Console.WriteLine($"GarminMtpDataBackupper version: {typeof(Program).Assembly.GetName().Version}");
            Console.WriteLine("Press Enter to start.");
            Console.ReadLine();

            var configReader = new ConfigReader();
            var config = configReader.GetConfig();

            var device = new Device(config);

            var dataBackuper = new DataBackupper(config, device);
            dataBackuper.BackupGarminData();

            Console.WriteLine("\nPress Enter to finish.");
            Console.ReadLine();
        }
    }
}
