using System.Collections.Generic;
using System.Configuration;
using SampleSDK.CRM.Library.Common.ConfigFileHandler;

namespace SampleSDK.OAuth.Common
{
    public class ZohoOAuthUtil
    {
        public ZohoOAuthUtil() { }

        //TODO: Write a logger;


        //TODO: Implement exceptions and do throw necessary exceptions and log the exceptions;

        public static Dictionary<string, string> ConfigFileSectionToDict(string sectionName)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSection section = configuration.GetSection(sectionName) as ConfigFileSection;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (ConfigFileElement element in section.Settings)
            {
                keyValuePairs.Add(element.Key, element.Value);
            }

            return keyValuePairs;

        }

    }
}
