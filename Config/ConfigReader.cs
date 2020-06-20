using Newtonsoft.Json;
using System.IO;

namespace GarminMtpDataBackupper.Config
{
    public class ConfigReader
    {
        private readonly string _configFileName;

        public ConfigReader(string configFileName = "config.json")
        {
            _configFileName = configFileName;
        }

        public Config GetConfig()
        {
            if (!File.Exists(_configFileName))
            {
                throw new ConfigException($"Missing config file: {_configFileName}");
            }

            var configFileContent = File.ReadAllText(_configFileName);
            var config = JsonConvert.DeserializeObject<Config>(configFileContent);

            return config;
        }
    }
}
