using System;
using System.Configuration;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Common.ConfigFileHandler;

using SampleSDK.CRM.Library;


namespace SampleSDK.CRM.Library.Common
{
    public class CommonUtil
    {
        public CommonUtil() { }

        //TODO: sortOrder enum;
        //TODO: photoSize enum;


        //TODO: Dictionary to Json Object method;




        //TODO: Implement exceptions;
        public static Dictionary<string, string> ConfigFileSectionToDict(string sectionName)
        {
            //Console.WriteLine("Section Name : "+sectionName);
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSection configFileSection = configuration.GetSection(sectionName) as ConfigFileSection;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            if (configFileSection != null)
            {
                foreach (ConfigFileElement element in configFileSection.Settings)
                {
                    keyValuePairs.Add(element.Key, element.Value);
                }
            }
            else
            {
                Console.WriteLine("Null Section");

            }
            return keyValuePairs;

        }

        //TODO: Time and date methods

        //TODO: PhotoSupported method

        //TODO: Check if validate file method is needed or not and implement if needed;

        //TODO: Collection to comma delimited string method;
    }
}
