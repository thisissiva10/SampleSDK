using System;
using SampleSDK.CRM.Library.Common;

namespace SampleSDK.CRM.Library.CRUD
{
    public class ZCRMModuleRelation : ZCRMEntity
    {

        private string label;
        private string apiName;
        private string module;
        private string name;
        private long id;
        private string parentModuleAPIName;
        private bool visible;
        private string type;

        //TODO: Generate ZCRMRecord and ZCRMJunctionRecord classes and uncomment the below lines;
        private ZCRMRecord parentRecord;
        private ZCRMJunctionRecord junctionRecord;

        private ZCRMModuleRelation(string parentModuleAPIName, string relatedListAPIName)
        {
            ParentModuleAPIName = parentModuleAPIName;
            ApiName = relatedListAPIName;
        }

        private ZCRMModuleRelation(string parentModuleAPIName, long relatedListId)
        {
            ParentModuleAPIName = parentModuleAPIName;
            Id = relatedListId;
        }

        private ZCRMModuleRelation(ZCRMRecord parentRecord, string relatedListAPIName)
        {
            ParentRecord = parentRecord;
            ApiName = relatedListAPIName;
        }

        private ZCRMModuleRelation(ZCRMRecord parentRecord, ZCRMJunctionRecord junctionRecord)
        {
            ParentRecord = parentRecord;
            JunctionRecord = junctionRecord;
        }
       

        public static ZCRMModuleRelation GetInstance(string parentModuleAPIName, string relatedListAPIName)
        {
            return new ZCRMModuleRelation(parentModuleAPIName, relatedListAPIName);
        }

        public static ZCRMModuleRelation GetInstance(string parentModuleAPIName, long relatedListId)
        {
            return new ZCRMModuleRelation(parentModuleAPIName, relatedListId);
        }

        public static ZCRMModuleRelation GetInstance(ZCRMRecord parentRecord, string relatedListAPIName)
        {
            return new ZCRMModuleRelation(parentRecord, relatedListAPIName);
        }

        public static ZCRMModuleRelation GetInstance(ZCRMRecord parentRecord, ZCRMJunctionRecord junctionRecord)
        {
            return new ZCRMModuleRelation(parentRecord, junctionRecord);
        }

        public string ApiName { get => apiName; set => apiName = value; }

        public long Id { get => id; set => id = value; }

        public string ParentModuleAPIName { get => parentModuleAPIName; private set => parentModuleAPIName = value; }

        public ZCRMRecord ParentRecord { get => parentRecord; private set => parentRecord = value; }

        private ZCRMJunctionRecord JunctionRecord { get => junctionRecord; set => junctionRecord = value; }

        public string Module { get => module; set => module = value; }

        public string Label { get => label; set => label = value; }

        public string Name { get => name; set => name = value; }

        public bool Visible { get => visible; set => visible = value; }

        public string Type { get => type; set => type = value; }


        //TODO: Complete the remaining methods;

    }
}
