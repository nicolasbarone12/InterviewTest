using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class ConfigHelper
    {
        private static ConfigHelper _instance;
        private static IConfigurationRoot _configuration;
                
        private ConfigHelper()
        {
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appSettings.json");
            var builder = new ConfigurationBuilder().AddJsonFile(configFilePath);
            if (_configuration == null)
            {
                _configuration = builder.Build();
            }
        }

        public static ConfigHelper GetInstance()
        {
            if (_instance is null)
                _instance = new ConfigHelper();

            return _instance;
        }

        public string GetConnectionStrings(string connectionStringName)
        {
            return _configuration.GetConnectionString(connectionStringName);
        }
    }
}
