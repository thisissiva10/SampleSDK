using System.Configuration;

namespace SampleSDK.CRM.Library.Common.ConfigFileHandler
{
    public class ConfigFileSection : ConfigurationSection
    {
    
        //TODO: Learn more about programmatic and static declerations;

        //TODO: inspect the use of this and base keywords;


        public ConfigFileSection(){
            
        }

        [ConfigurationProperty("settings", IsDefaultCollection = false)]
        public ConfigFileCollection Settings
        {
            get
            {
                ConfigFileCollection configFileCollection =
                    (ConfigFileCollection)base["settings"];

                return configFileCollection;
            }

            set
            {
                base["settings"] = value;
            }
        }
    }
}
