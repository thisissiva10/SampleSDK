using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.CRUD;
using SampleSDK.CRM.Library.Api.Response;


namespace SampleSDK.CRM.Library.Api.Handler
{
    public class EntityAPIHandler : CommonAPIHandler, IAPIHandler
    {
        //NOTE:Property not used;
        protected ZCRMRecord record = null;


        protected EntityAPIHandler(ZCRMRecord zcrmRecord)
        {
            record = zcrmRecord;
        }

        public static EntityAPIHandler GetInstance(ZCRMRecord zcrmRecord)
        {
            return new EntityAPIHandler(zcrmRecord);
        }

        //TODO: Handle Exceptions;
        public APIResponse GetRecord()
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = $"{record.ModuleAPIName}/{record.EntityId}";

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject recordDetails = (JObject)responseDataArray[0];
                SetRecordProperties(recordDetails);

                return null;
            }
        }

        public APIResponse CreateRecord()
        {
            return null;
        }

        public APIResponse UpdateRecord()
        {
            return null;
        }


        public APIResponse DeleteRecord()
        {
            return null;
        }


        public void SetRecordProperties(JObject recordDetails)
        {
            SetRecordProperties(recordDetails, record);
        }

        public void SetRecordProperties(JObject recordJSON, ZCRMRecord record)
        {
            JObject recordDetails = new JObject(recordJSON);
            foreach(KeyValuePair<string, JToken> token in recordDetails)
            {
                
            }
        }
    }
}
