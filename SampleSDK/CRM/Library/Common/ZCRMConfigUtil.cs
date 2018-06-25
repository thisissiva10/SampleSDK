using System;
using System.Collections.Generic;
using SampleSDK.OAuth.Client;

namespace SampleSDK.CRM.Library.Common
{
    public class ZCRMConfigUtil
    {



        //TODO: Refer public naming conventions;
        private static Dictionary<string, string> configProperties = new Dictionary<string, string>();

        public static Dictionary<string, string> ConfigProperties{
            get { return configProperties; }
            set { configProperties = value; }
        }
        private static Boolean handleAuthentication = false;

        //TODO: Check whether set accessor is necessary;
        public static Boolean HandleAuthentication {
            get { return handleAuthentication;  }
            set { handleAuthentication = value; }
        }


        public ZCRMConfigUtil() { }

        //TODO: Initialize method for mobile;

        public static void Initialize(Boolean initOAuth) {

            //TODO: Get configProperties from configProperties from configFile;
            ConfigProperties = CommonUtil.ConfigFileSectionToDict("configuration");

            Dictionary<string, string> keyValuePairs = CommonUtil.ConfigFileSectionToDict("zcrm_configuration");
            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
            { 
                ConfigProperties.Add(keyValues.Key, keyValues.Value); 
            }

            if(initOAuth){
                HandleAuthentication = true;
                //TODO: Use try catch or corresponding exception handling;
                ZohoOAuth.Initialize(ConfigProperties.ContainsKey("domainSuffix")?(string)ConfigProperties["domainSuffix"]:null);
            }
            if(ConfigProperties.ContainsKey("domainSuffix")){
                UpdateConfigBaseUrl((string)ConfigProperties["domainSuffix"]);
            }
            //TODO: Log the information along with ConfigProperties;

        }


        //TODO: Write LoadConfigProperties() method;
        //TODO: Write GetAccessToken() method;


        //TODO: Throw exception and complete the method;
        public static void UpdateConfigBaseUrl(string location){
            string apiBaseUrl = "apiBaseUrl";
            string accessType = GetAccessType();
            string domain = "www";
            //TODO: Get domain from api constants and correspondingly do the update in the local field;
            /*if(){
                
            }*/

            switch (location)
            {
                case "eu":
                    SetConfigValue(apiBaseUrl, "https://" + domain + ".zohoapis.eu");
                    break;
                case "cn":
                    SetConfigValue(apiBaseUrl, "https://" + domain + ".zohoapis.com.cn");
                    break;
                default:
                    SetConfigValue(apiBaseUrl, "https://" + domain + ".zohoapis.com");
                    break;
            }



        }


        public static void SetConfigValue(string config, string value){
            ConfigProperties.Add(config, value);
        }

        public static string GetConfigValue(string config){
            return (string)ConfigProperties[config];
        }

        public static string GetApiBaseURL(){
            return GetConfigValue("apiBaseURL");
        }

        public static string GetApiVersion(){
            return GetConfigValue("apiVersion");
        }

        public static string GetAuthenticationClass(){
            return GetConfigValue("loginAuthClass");
        }

        public static string GetAccessType(){
            return GetConfigValue("accessType");
        }

        public static string GetPhotoUrl(){
            return GetConfigValue("photoUrl");
        }

        public static Boolean ShouldHandleAuthentication(){
            return HandleAuthentication;
        }

        //Omitted getAllConfig(). The property ConfigProperty can be called for such request;

    }

}
