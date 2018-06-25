using System;
using System.Collections.Generic;
using SampleSDK.OAuth.Common;
using SampleSDK.OAuth.Contract;

namespace SampleSDK.OAuth.Client
{
    public class ZohoOAuth
    {
        //TODO: Write a  logger;

        private static Dictionary<string, string> configProperties = new Dictionary<string, string>();

        public ZohoOAuth() { }

        //TODO: Write Exceptions;
        public static void Initialize()
        {
            Initialize(null);
        }

        //TODO: Write Exceptions
        public static void Initialize(string domainSuffix)
        {
            Initialize(domainSuffix, null);
        }


        //TODO: Throw exceptions
        public static void Initialize(string domainSuffix, string configFileName)
        {

            SetIAMUrl(domainSuffix);
            AddConfigurationData("oauth_configuration");
            if (configFileName != null)
            {
                AddConfigurationData(configFileName);
            }

            //TODO:Delete the following code after completion;
            Console.WriteLine("OAuth Inititalization Configurations:");
            foreach (KeyValuePair<string, string> configuration in configProperties)
            {
                Console.WriteLine(configuration.Key + " : " + configuration.Value);
            }

            //End region;

            ZohoOAuthParams oAuthParams = new ZohoOAuthParams() {    
                ClientId = GetConfigValue("client_id"),
                ClientSecret = GetConfigValue("client_secret"),
                AccessType = GetConfigValue("access_type"),
                RedirectURL = GetConfigValue("redirect_uri"),
                Scopes = GetConfigValue("scope")    
            };


            ZohoOAuthClient.GetInstance(oAuthParams);
            //TODO: Log the configuration properties;
            //TODO: Refer Java SDK for exceptions;

        }


        private static void SetIAMUrl(string domainSuffix)
        {
            domainSuffix = domainSuffix ?? "com";
            switch (domainSuffix)
            {
                case "eu":
                    configProperties.Add("iamURL", "https://accounts.zoho.eu");
                    break;
                case "cn":
                    configProperties.Add("iamURL", "https://accounts.zoho.cn");
                    break;
                default:
                    configProperties.Add("iamURL", "https://accounts.zoho.com");
                    break;
            }
        }

        //Adds Configuration key value pairs specified by the argument to configProperties;
        private static void AddConfigurationData(string configFileName)
        {
            Dictionary<string, string> keyValuePairs = ZohoOAuthUtil.ConfigFileSectionToDict(configFileName);
            foreach (KeyValuePair<string, string> keyValues in keyValuePairs)
            {
                configProperties.Add(keyValues.Key, keyValues.Value);
            }
        }

        public static string GetConfigValue(string key){
            return configProperties[key];
        }

        public static Dictionary<string, string> ConfigProperties => configProperties;


        public static string GetIAMUrl()
        {
            return GetConfigValue("iamURL");
        }

        public static string GetLoginWithZohoUrl()
        {
            return GetIAMUrl() + "/oauth/v2/auth?scope=" + GetCRMScope() + "&client_id=" + GetClientID() + "&client_secret=" + GetClientSecret() + "&response_type=code&access_type=" + GetAccessType() + "&redirect_uri=" + GetRedirectURL();
        }

        public static string GetTokenURL()
        {
            return GetIAMUrl() + "/oauth/v2/token";
        }

        public static string GetRefreshTokenURL()
        {
            return GetIAMUrl() + "/oauth/v2/token";
        }

        public static string GetUserInfoURL()
        {
            return GetIAMUrl() + "/oauth/user/info";
        }

        public static string GetRevokeTokenURL()
        {
            return GetIAMUrl() + "/oauth/v2/token/revoke";
        }

        public static string GetCRMScope()
        {
            return GetConfigValue("scope");
        }

        public static string GetClientID()
        {
            return GetConfigValue("client_id");
        }

        public static string GetClientSecret()
        {
            return GetConfigValue("client_secret");
        }

        public static string GetRedirectURL()
        {
            return GetConfigValue("redirect_uri");
        }

        public static string GetAccessType()
        {
            return GetConfigValue("access_type");
        }

        //TODO: Get Persistence Handler class with class not found exception;

        public static IZohoPersistenceHandler GetPersistenceHandlerInstance(){

            //TODO: Implement try and catch all possible exceptions;
            try
            {
                string classAssemblyName = GetConfigValue("persistence_handler_class");
                return (IZohoPersistenceHandler)Activator.CreateInstance(type: Type.GetType(classAssemblyName));
            }catch(Exception e) when (e is NullReferenceException || e is ArgumentNullException){
                throw new ZohoOAuthException(e);
            }
        }
    }
}
