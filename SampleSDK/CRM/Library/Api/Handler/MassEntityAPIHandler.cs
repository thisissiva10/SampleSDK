using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.CRMException;
using SampleSDK.CRM.Library.CRUD;

namespace SampleSDK.CRM.Library.Api.Handler
{
    public class MassEntityAPIHandler : CommonAPIHandler, IAPIHandler
    {
        private ZCRMModule module;
        private ZCRMTrashRecord trashRecord = null;


        private MassEntityAPIHandler(ZCRMModule zcrmModule)
        {
            module = zcrmModule;
        }

        public static MassEntityAPIHandler GetInstance(ZCRMModule zcrmModule)
        {
            return new MassEntityAPIHandler(zcrmModule);   
        }


        public BulkAPIResponse<ZCRMRecord> GetRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince, string isConverted, string isApproved, List<string> fields)
        {
            try{
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = module.ApiName;
                if(cvId != null)
                {
                    requestQueryParams.Add("cvid", cvId.ToString());
                }
                requestQueryParams.Add("sort_by", sortByField);
                if(sortOrder != null)
                {
                    requestQueryParams.Add("sort_order", sortOrder.ToString());
                }
                requestQueryParams.Add("page", page.ToString());
                requestQueryParams.Add("per_page", perPage.ToString());
                requestQueryParams.Add("coneverted", isConverted);
                requestQueryParams.Add("approved", isApproved);
                if(fields != null)
                {
                    requestQueryParams.Add("fields", CommonUtil.CollectionToCommaDelimitedString(fields));
                }
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse();

                List<ZCRMRecord> records = new List<ZCRMRecord>();
                JObject responseJSON = response.ResponseJSON;
                JArray recordsArray = (JArray)responseJSON.GetValue("data");
                foreach(JObject recordDetails in recordsArray)
                {
                    ZCRMRecord record = ZCRMRecord.GetInstance(module.ApiName, Convert.ToInt64(recordDetails.GetValue("id")));
                    EntityAPIHandler.GetInstance(record).SetRecordProperties(recordDetails);
                    records.Add(record);
                }
                response.BulkData = records;
                return response;
            }catch(Exception e)
            {
                //TODO: Log the info;
                throw new ZCRMException("ZCRM_INTERNA_ERROR", e.ToString());
            }
        }


        public MassEntityAPIHandler()
        {
        }
    }
}
