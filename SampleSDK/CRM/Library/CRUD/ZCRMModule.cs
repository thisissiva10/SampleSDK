using System;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Api.Handler;
using SampleSDK.CRM.Library.Setup.Users;

namespace SampleSDK.CRM.Library.CRUD
{
    public class ZCRMModule : ZCRMEntity
    {
        private string systemName;
        private string apiName;
        private string singularLabel;
        private string pluralLabel;
        private long id;
        private Boolean customModule;
        private Boolean creatable;
        private Boolean viewable;
        private Boolean convertible;
        private Boolean deletable;
        private Boolean editable;
        private ZCRMUser modifiedBy;
        private string modifiedTime;

        //TODO: Inspect the usage of Collections;
        //TODO: Generate a ZCRMLayout and ZCRMModuleRelation classes and uncomment the below lines and implement getters and setters;
        private List<ZCRMLayout> layouts = new List<ZCRMLayout>();
        private List<ZCRMModuleRelation> relatedLists = new List<ZCRMModuleRelation>();

        private List<string> bussinessCardFields = new List<string>();
        private List<ZCRMProfile> accessibleProfiles = new List<ZCRMProfile>();

        protected ZCRMModule(string apiName)
        {
            ApiName = apiName;
        }


        public static ZCRMModule GetInstance(string apiName)
        {
            return new ZCRMModule(apiName);
        }

        public string ApiName { get => apiName; private set => apiName = value; }

        public long Id { get => id; set => id = value; }

        public string SystemName { get => systemName; set => systemName = value; }

        public string SingularLabel { get => singularLabel; set => singularLabel = value; }

        public string PluralLabel { get => pluralLabel; set => pluralLabel = value; }

        //TODO: Inspect the usage of the Convertible property name;

        public bool Convertible { get => convertible; set => convertible = value; }

        public bool Deletable { get => deletable; set => deletable = value; }

        public bool Editable { get => editable; set => editable = value; }

        public bool Viewable { get => viewable; set => viewable = value; }

        public bool CustomModule { get => customModule; set => customModule = value; }

        public ZCRMUser ModifiedBy { get => modifiedBy; set => modifiedBy = value; }

        public string ModifiedTime { get => modifiedTime; set => modifiedTime = value; }

        public List<string> BussinessCardFields { get => bussinessCardFields; set => bussinessCardFields = value; }

        //TODO:BulkAPIResponse getters for layouts needs to be implemented;
        public List<ZCRMLayout> Layouts { set => layouts = value; }
       
        public List<ZCRMModuleRelation> RelatedLists { get => relatedLists; set => relatedLists = value; }

        public bool Creatable { get => creatable; set => creatable = value; }

        public List<ZCRMProfile> AccessibleProfiles { get => accessibleProfiles; private set => accessibleProfiles = value; }

        public APIResponse GetRecord(long entityId)
        {
            ZCRMRecord record = ZCRMRecord.GetInstance(ApiName, entityId);
            return EntityAPIHandler.GetInstance(record).GetRecord();
        }

        public void AddAccessibleProfiles(ZCRMProfile profile)
        {
            AccessibleProfiles.Add(profile);
        }

        //TODO: Complete the remaining functions;

    }
}
