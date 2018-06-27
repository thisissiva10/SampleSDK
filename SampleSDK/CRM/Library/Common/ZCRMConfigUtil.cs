using System;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Setup.Restclient;
using SampleSDK.OAuth.Client;
using SampleSDK.CRM.Library.CRMException;
using SampleSDK.OAuth.Common;

namespace SampleSDK.CRM.Library.Common
{
    public class ZCRMConfigUtil
    {


        private static Dictionary<string, string> configProperties = new Dictionary<string, string>();
        private static Boolean handleAuthentication = false;

        //TODO: Check whether set accessor is necessary;
        public static Boolean HandleAuthentication { get => handleAuthentication; private set => handleAuthentication = value; }

        public static Dictionary<string, string> ConfigProperties { get => configProperties; private set => configProperties = value; }


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


        public static string GetAccessToken()
        {

            //TODO: Inspect the usage of catch claues;
            string userMailId = ZCRMRestClient.GetCurrentUserEmail();

            if((userMailId == null) && (ConfigProperties["currentUserEmail"] == null))
            {
                throw new ZCRMException("Current user must be either set in ZCRMRestClient or zcrm_configuration Config section");
            }
            if(userMailId == null)
            {
                userMailId = ConfigProperties["currentUserEmail"];
            }
            try
            {
                ZohoOAuthClient client = ZohoOAuthClient.GetInstance();
                return client.GetAccessToken(userMailId);
            }catch(ZohoOAuthException) { throw; }
        }


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

        //NOTE: Omitted getAllConfig(). The property ConfigProperty can be called for such request;

    }

}
