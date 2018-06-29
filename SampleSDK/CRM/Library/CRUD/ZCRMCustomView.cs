using System;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Common;


namespace SampleSDK.CRM.Library.CRUD
{
    public class ZCRMCustomView : ZCRMEntity
    {
        private string moduleAPIName;
        private string displayName;
        private string name;
        private string systemName;
        private long id;
        private string sortBy;
        private CommonUtil.SortOrder sortOrder;
        private string category;
        private List<string> fields = new List<string>();
        private int favourite;
        private bool isdefault;


        private ZCRMCustomView(string moduleAPIName, long customViewId)
        {
            ModuleAPIName = moduleAPIName;
            Id = customViewId;
        }

        public static ZCRMCustomView GetInstance(string moduleAPIName, long customViewId)
        {
            return new ZCRMCustomView(moduleAPIName, customViewId);
        }

        public string ModuleAPIName { get => moduleAPIName; private set => moduleAPIName = value; }
        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string SystemName { get => systemName; set => systemName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public List<string> Fields { get => fields; set => fields = value; }
        public CommonUtil.SortOrder SortOrder { get => sortOrder; set => sortOrder = value; }
        public string SortBy { get => sortBy; set => sortBy = value; }
        public string Category { get => category; set => category = value; }
        public bool Isdefault { get => isdefault; set => isdefault = value; }
        public int Favourite { get => favourite; set => favourite = value; }


        //TODO<IMPORTANT>: Test the methods for correct Generic parameters;
        public BulkAPIResponse<ZCRMRecord> GetRecords()
        {
            return GetRecords(null);
        }

        private BulkAPIResponse<ZCRMRecord> GetRecords(List<string> fields)
        {
            return GetRecords(1, 200, fields);
        }

        private BulkAPIResponse<ZCRMRecord> GetRecords(int page, int perPage, List<string> fields)
        {
            return GetRecords(null, null, page, perPage, null, fields);   
        }


        //TODO<IMPORTANT> : Got to handle the generic parameter;
        private BulkAPIResponse<ZCRMRecord> GetRecords(object p1, object p2, int page, int perPage, object p3, List<string> fields)
        {
            throw new NotImplementedException();
        }
    }
}
