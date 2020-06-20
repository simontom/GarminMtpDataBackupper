using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GarminMtpDataBackupper.Config
{
    public class Config
    {
#pragma warning disable 0649
        [JsonProperty(PropertyName = "garminDeviceName")]
        private readonly string _garminDeviceName;

        [JsonProperty(PropertyName = "garminDiskName")]
        private readonly string _garminDiskName;

        [JsonProperty(PropertyName = "garminRoot")]
        private readonly string _garminRoot;

        [JsonProperty(PropertyName = "garminFolders")]
        private readonly ISet<string> _garminFolders;
        private IList<string> _garminFoldersAsList = null;

        [JsonProperty(PropertyName = "targetPath")]
        private readonly string _targetPath;
#pragma warning restore 0649

        public string GarminDeviceName => string.IsNullOrEmpty(_garminDeviceName) ? throw new ConfigException("Field 'garminDeviceName' is missing or empty.") : _garminDeviceName;

        public string GarminDiskName => string.IsNullOrEmpty(_garminDiskName) ? throw new ConfigException("Field 'garminDiskName' is missing or empty.") : _garminDiskName;

        public string GarminRoot => string.IsNullOrEmpty(_garminRoot) ? throw new ConfigException("Field 'garminRoot' is missing or empty.") : _garminRoot;

        public IList<string> GarminFolders
        {
            get
            {
                if (IsNullOrEmpty(_garminFolders))
                {
                    throw new ConfigException("Field 'garminFolders' is missing or empty.");
                }

                if (_garminFoldersAsList == null)
                {
                    _garminFoldersAsList = _garminFolders.ToList();
                }

                return _garminFoldersAsList;
            }
        }

        public string TargetPath => string.IsNullOrEmpty(_targetPath) ? throw new ConfigException("Field 'targetPath' is missing or empty.") : _targetPath;

        private bool IsNullOrEmpty<T>(ISet<T> collection)
        {
            return (collection == null) || (collection.Count == 0);
        }
    }
}
