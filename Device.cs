using MediaDevices;
using System;
using System.IO;
using System.Linq;

namespace GarminMtpDataBackupper
{
    public class Device : IDisposable
    {
        private readonly string _deviceName;
        private MediaDevice _connectedDevice;

        public Device(Config.Config config)
        {
            _deviceName = config.GarminDeviceName;
        }

        public void Dispose()
        {
            _connectedDevice?.Dispose();
        }

        public void Connect()
        {
            var devices = MediaDevice.GetDevices();
            _connectedDevice = devices.FirstOrDefault((device) => (device.FriendlyName == _deviceName) || (device.Description == _deviceName));

            if (_connectedDevice == null)
            {
                var exception = new IOException($"Unable to connect to Garmin device: {_deviceName}");
                Logger.Error("Connecting to the device problem.", exception);
                throw exception;
            }

            _connectedDevice.Connect();
        }

        public void Disconnect()
        {
            _connectedDevice?.Disconnect();
            _connectedDevice = null;
        }

        public void DownloadFile(MediaFileInfo sourceFile, string destinationFolderPath)
        {
            MemoryStream memoryStream = new MemoryStream();
            _connectedDevice.DownloadFile(sourceFile.FullName, memoryStream);
            memoryStream.Position = 0;

            using (FileStream file = new FileStream($@"{destinationFolderPath}\{sourceFile.Name}", FileMode.Create, FileAccess.Write))
            {
                byte[] bytes = new byte[memoryStream.Length];
                memoryStream.Read(bytes, 0, (int)memoryStream.Length);
                file.Write(bytes, 0, bytes.Length);
                memoryStream.Close();
            }
        }

        public MediaDirectoryInfo GetDirectoryInfo(string path)
        {
            return _connectedDevice.GetDirectoryInfo(path);
        }

        public bool FolderExists(string path)
        {
            return _connectedDevice.DirectoryExists(path);
        }
    }
}
