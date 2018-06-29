using System;
using System.Configuration;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Common.ConfigFileHandler;
using Newtonsoft.Json.Linq;


namespace SampleSDK.CRM.Library.Common
{




    public class CommonUtil
    {
        public CommonUtil() { }

        public enum SortOrder { asc, desc }

        public enum PhotoSize { stamp, thumb, original, favicon, medium }

        //TODO: sortOrder enum;
        //TODO: photoSize enum;


        //TODO: Implement exception handling;
        public static Dictionary<string, string> ConvertJObjectToDict(JObject json)
        {
            Dictionary<string, string> returnMap = new Dictionary<string, string>();
            foreach(var keyValuePair in json)
            {
                returnMap.Add(keyValuePair.Key, keyValuePair.Value.ToString());
            }

            return returnMap;
        }



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

        internal static string CollectionToCommaDelimitedString(List<string> fields)
        {
            string retString = "";
            foreach(string field in fields)
            {
                retString += field + ",";
            }
            return retString.TrimEnd(',');
        }

        //TODO: Time and date methods

        //TODO: PhotoSupported method

        //TODO: Check if validate file method is needed or not and implement if needed;

        //TODO: Collection to comma delimited string method;
    }
}
