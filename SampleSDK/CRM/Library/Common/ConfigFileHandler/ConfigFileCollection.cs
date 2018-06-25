using System.Configuration;

namespace SampleSDK.CRM.Library.Common.ConfigFileHandler
{
    [ConfigurationCollection(itemType: typeof(ConfigFileElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ConfigFileCollection : ConfigurationElementCollection
    {

        static ConfigFileCollection()
        {

            collectionProperties = new ConfigurationPropertyCollection();

        }

        //TODO: Check for public constructors;

        private static ConfigurationPropertyCollection collectionProperties;

        //TODO: Inspect the use of indexers;
        protected ConfigurationPropertyCollection CollectionProperties
        {
            get { return collectionProperties; }
        }

        //TODO: Check for hiding base class members by the inherited member and the new keyword;
        protected new ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }


        //Indexer
        public ConfigFileElement this[int index]
        {
            get { return (ConfigFileElement)base.BaseGet(index); }

            //TODO: Check for the neccessity of set element and decide on implementing the set method;
            set
            {
                if (base.BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }


        //TODO: Check for hiding base class members by the inherited member and the new keyword;
        public new ConfigFileElement this[string key]
        {
            get { return (ConfigFileElement)base.BaseGet(key); }
        }

        //Overridden Methods;
        protected override ConfigurationElement CreateNewElement()
        {
            //throw new NotImplementedException();
            return new ConfigFileElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //throw new NotImplementedException();
            return (element as ConfigFileElement).Key;
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            //TODO: Find out BaseAdd method(ConfigurationElement);
        }

        public void Clear(){
            BaseClear();
        }

        public void Add(ConfigFileElement element){
                base.BaseAdd(element);
        }
    }
}
