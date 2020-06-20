using System;

namespace GarminMtpDataBackupper.Config
{
    public class ConfigException : Exception
    {
        public ConfigException(string message): base(message)
        {
        }
    }
}
