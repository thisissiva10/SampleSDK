using System;
using System.Collections.Generic;
using System.Configuration;
using SampleSDK.CRM.Library.Common.ConfigFileHandler;
using SampleSDK.OAuth.Client;
using SampleSDK.OAuth.Contract;
using SampleSDK.OAuth.Common;

namespace SampleSDK.OAuth.ClientApp
{
    public class ZohoOAuthFilePersistence : IZohoPersistenceHandler
    {
      
        public void DeleteOAuthTokens(string userMailId)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSection section = configuration.GetSection("oauthtokens") as ConfigFileSection;
            section.Settings.Clear();

            section.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Modified);

            //TODO: Log the exceptions and implement exception handling; 

        }

        public ZohoOAuthTokens GetOAuthTokens(string userMailId)
        {
            ZohoOAuthTokens tokens = new ZohoOAuthTokens();
            Dictionary<string, string> oauthTokens = ZohoOAuthUtil.ConfigFileSectionToDict("oauthtokens");
            if(!oauthTokens["useridentifier"].Equals(userMailId)){
                //TODO: Throw ZohoOAuthException and remove the console write statement;
                Console.WriteLine("Given user not found in persistence");
            }

            tokens.UserMaiilId = oauthTokens["useridentifier"];
            tokens.AccessToken = oauthTokens["accesstoken"];
            tokens.RefreshToken = oauthTokens["refreshtoken"];
            tokens.ExpiryTime = Convert.ToInt64(oauthTokens["expirytime"]);

            //TODO: Log the exceptions and implement try catching statements
            return tokens;

        }

        public void SaveOAuthData(ZohoOAuthTokens zohoOAuthTokens)
        {
            ForceDeleteOAuthData();
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSection section = configuration.GetSection("oauthtokens") as ConfigFileSection;
            if (section == null)
            {
                section = new ConfigFileSection();
                configuration.Sections.Add("oauthtokens", section);
            }
            section.Settings.Add(new ConfigFileElement("useridentifier", zohoOAuthTokens.UserMaiilId));
            section.Settings.Add(new ConfigFileElement("accesstoken", zohoOAuthTokens.AccessToken));
            section.Settings.Add(new ConfigFileElement("refreshtoken", zohoOAuthTokens.RefreshToken));
            section.Settings.Add(new ConfigFileElement("expirytime", zohoOAuthTokens.ExpiryTime.ToString()));

            section.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Modified);

            //TODO: Log the exceptions and implement exception handling;
        }


        //TODO: Think of a better idea about this concept;
        private void ForceDeleteOAuthData(){
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigFileSection section = configuration.GetSection("oauthtokens") as ConfigFileSection;
            if (section != null)
            {
                section.Settings.Clear();
                section.SectionInformation.ForceSave = true;
                configuration.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}
