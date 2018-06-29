using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.CRMException;


namespace SampleSDK.CRM.Library.Api.Response
{
    
    //TODO<DUE-TOMORROW>: Needs some learning to implement this class;
    public class BulkAPIResponse<T> : CommonAPIResponse where T : ZCRMEntity
    {

        private List<T> bulkData;
        private ResponseInfo info;
        private List<EntityResponse> bulkEntitiesResponse;

        public BulkAPIResponse() { }

        public BulkAPIResponse(HttpWebResponse response) : base(response)
        {
            SetInfo();
        }



        public List<T> BulkData { get => bulkData; set => bulkData = value; }
       
        public ResponseInfo Info { get => info; private set => info = value; }
     
        public List<EntityResponse> BulkEntitiesResponse { get => bulkEntitiesResponse; private set => bulkEntitiesResponse = value; }

        private void SetInfo()
        {
            if(ResponseJSON.ContainsKey("info"))
            {
                Info = new ResponseInfo((JObject)ResponseJSON.GetValue("info"));
            }
        }

        protected override void ProcessDataResponse()
        {
            BulkEntitiesResponse = new List<EntityResponse>();
            if(ResponseJSON.ContainsKey("data"))
            {
                JArray recordsArray = (JArray)ResponseJSON.GetValue("data");
                foreach(JObject recordJSON in recordsArray)
                {
                    if(recordJSON.ContainsKey("status"))
                    {
                        EntityResponse individualResponse = new EntityResponse(recordJSON);
                        BulkEntitiesResponse.Add(individualResponse);
                    }
                }
            }
        }


        //TODO: Handle exceptions appropriately;
        protected override void HandleFaultyResponse()
        {
            if((HttpStatusCode == APIConstants.ResponseCode.NO_CONTENT) || (HttpStatusCode == APIConstants.ResponseCode.NOT_MODIFIED))
            {
                ResponseJSON = new JObject();
                BulkData = new List<T>();
            }
            else
            {
                throw new ZCRMException(Convert.ToString(ResponseJSON.GetValue("code")), Convert.ToString(ResponseJSON.GetValue("message")));     
            }
        }

        public class EntityResponse
        {
            private JObject responseJSON;
            private ZCRMEntity data;
            private string status;
            private string message;
            private string code;
            private JObject errorDetails;
            private Dictionary<string, string> upsertedDetails = new Dictionary<string, string>();

            public EntityResponse(JObject entityResponseJSON)
            {
                ResponseJSON = entityResponseJSON;
                Status = Convert.ToString(entityResponseJSON.GetValue("status"));
                Code = Convert.ToString(entityResponseJSON.GetValue("code"));
                Message = Convert.ToString(entityResponseJSON.GetValue("message"));
                if((ResponseJSON.ContainsKey("details")) && (Status.Equals("error")))
                {
                    ErrorDetails = (JObject)ResponseJSON.GetValue("details");
                }
                if(entityResponseJSON.ContainsKey("action"))
                {
                    upsertedDetails.Add("action", Convert.ToString(entityResponseJSON.GetValue("action")));
                }
                if(entityResponseJSON.ContainsKey("duplicate_field"))
                {
                    upsertedDetails.Add("duplicate_field", Convert.ToString(entityResponseJSON.GetValue("duplciate_field")));   
                }
            }

            public JObject ResponseJSON { get => responseJSON; private set => responseJSON = value; }
            public ZCRMEntity Data { get => data; set => data = value; }
            public string Status { get => status; private set => status = value; }
            public string Message { get => message; private set => message = value; }
            public string Code { get => code; private set => code = value; }
            public JObject ErrorDetails { get => errorDetails; private set => errorDetails = value; }
        }

        //TODO: Inspect the usage of this class and learn about static classes and about access-modifiers;
        public class ResponseInfo
        {
            private bool moreRecords;
            private int recordCount;
            private int pageNo;
            private int perPage;


            internal ResponseInfo(JObject info)
            {
                MoreRecords = Convert.ToBoolean(info.GetValue("more_records"));
                RecordCount = Convert.ToInt32(info.GetValue("count"));
                PageNo = Convert.ToInt32(info.GetValue("page"));
                PerPage = Convert.ToInt32(info.GetValue("per_page"));
            }

            public bool MoreRecords { get => moreRecords; private set => moreRecords = value; }
            public int RecordCount { get => recordCount; private set => recordCount = value; }
            public int PageNo { get => pageNo; private set => pageNo = value; }
            public int PerPage { get => perPage; private set => perPage = value; }
        }

    }
}
