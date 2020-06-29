using MediaDevices;
using System;
using System.IO;

namespace GarminMtpDataBackupper
{
    public class DataBackupper
    {
        private readonly Config.Config _config;
        private readonly Device _device;

        private string GarminRootPath => Path.Combine(_config.GarminDiskName, _config.GarminRoot);
        private string FormattedCurrentDate => DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        private string DestinationRootFolderPath => Path.Combine(_config.TargetPath, $"{_config.GarminDeviceName}_{FormattedCurrentDate}");

        private int _directoriesCount;
        private int _filesCount;

        public DataBackupper(Config.Config config, Device device)
        {
            _config = config;
            _device = device;

            _directoriesCount = 0;
            _filesCount = 0;
        }

        public void BackupGarminData()
        {
            var garminRootPath = GarminRootPath;
            var destinationRootPath = DestinationRootFolderPath;

            using (_device)
            {
                _device.Connect();

                foreach (var folderName in _config.GarminFolders)
                {
                    string sourcePath = Path.Combine(garminRootPath, folderName);
                    string destinationPath = Path.Combine(destinationRootPath, folderName);
                    Logger.Info($@"Copying folder FROM: {sourcePath} TO: {destinationPath}");

                    CopyDirectoryRecursively(sourcePath, destinationPath);
                }

                _device.Disconnect();
            }

            PrintCopiesCount();
        }

        private void CopyDirectoryRecursively(string sourcePath, string destinationPath)
        {
            if (!_device.FolderExists(sourcePath))
            {
                Logger.Error($"Missing source: {sourcePath}");
                return;
            }

            MediaDirectoryInfo sourceDirectoryInfo;
            try
            {
                sourceDirectoryInfo = _device.GetDirectoryInfo(sourcePath);
            }
            catch (Exception exception)
            {
                Logger.Error($"Missing source or is not accessible: {sourcePath}", exception);
                return;
            }

            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            var files = sourceDirectoryInfo.EnumerateFiles();
            foreach (var file in files)
            {
                string temppath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(temppath, false);
                _filesCount++;
            }

            var folders = sourceDirectoryInfo.EnumerateDirectories();
            foreach (var subdir in folders)
            {
                string temppath = Path.Combine(destinationPath, subdir.Name);
                Logger.Warning($@"Copying SUBfolder FROM: {sourcePath} TO: {destinationPath}");
                CopyDirectoryRecursively(subdir.FullName, temppath);
                _directoriesCount++;
            }
        }

        private void PrintCopiesCount()
        {
            Logger.Info($"Copied\n\t- Directories: {_directoriesCount}\n\t- Files: {_filesCount}");
        }
    }
}
