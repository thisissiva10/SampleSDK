using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.CRMException;
using SampleSDK.CRM.Library.CRUD;
using SampleSDK.CRM.Library.Setup.Users;

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


        public BulkAPIResponse<ZCRMRecord> CreateRecords(List<ZCRMRecord> records)
        {
            if(records.Count > 100)
            {
                throw new ZCRMException("Cannot process more than 100 records at a time");
            }
            try{
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = module.ApiName;
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                foreach(ZCRMRecord record in records)
                {
                    if(record.EntityId == null)
                    {
                        dataArray.Add(EntityAPIHandler.GetInstance(record).GetZCRMRecordAsJSON());
                    }
                    else
                    {
                        throw new ZCRMException("Entity ID MUST be null for create operation.");
                    }
                }
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;

                BulkAPIResponse<ZCRMRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMRecord>();

                List<ZCRMRecord> createdRecords = new List<ZCRMRecord>();
                List<EntityResponse> responses = response.BulkEntitiesResponse;
                int responseSize = responses.Count;
                for (int i = 0; i < responseSize; i++)
                {
                    EntityResponse entityResponse = responses[i];
                    if(entityResponse.Status.Equals("success"))
                    {
                        JObject responseData = entityResponse.ResponseJSON;
                        JObject recordDetails = (JObject)responseData.GetValue("details");
                        ZCRMRecord newRecord = records[i];
                        EntityAPIHandler.GetInstance(newRecord).SetRecordProperties(recordDetails);
                        createdRecords.Add(newRecord);
                        entityResponse.Data = newRecord;
                    }
                    else{
                        entityResponse.Data = null;
                    }
                }
                response.BulkData = createdRecords;
                return response;
            }catch(Exception e){
                //TODO: Log the info;
                Console.WriteLine("Exception at createRecords");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        public BulkAPIResponse<ZCRMRecord> UpdateRecords(List<long> entityIds, string fieldAPIName, Object value)
        {
            if (entityIds.Count > 100)
            {
                throw new ZCRMException("Cannot process more than 100 records at a time");
            }
            //NOTE: null value is not converted to JObject of type null;
            try
            {
                requestMethod = APIConstants.RequestMethod.PUT;
                urlPath = module.ApiName;
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                foreach (long id in entityIds)
                {
                    JObject updateJSON = new JObject();
                    //TODO: Check the below line;
                    updateJSON.Add(fieldAPIName, (JToken)value);
                    updateJSON.Add("id", Convert.ToString("id"));
                    dataArray.Add(updateJSON);
                }
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;

                BulkAPIResponse<ZCRMRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMRecord>();

                List<ZCRMRecord> updatedRecords = new List<ZCRMRecord>();
                List<EntityResponse> responses = response.BulkEntitiesResponse;
                foreach(EntityResponse entityResponse in responses)
                {
                    if (entityResponse.Status.Equals("success"))
                    {
                        JObject responseData = entityResponse.ResponseJSON;
                        JObject recordDetails = (JObject)responseData.GetValue("details");
                        ZCRMRecord updatedRecord = ZCRMRecord.GetInstance(module.ApiName, Convert.ToInt64(recordDetails.GetValue("id")));
                        EntityAPIHandler.GetInstance(updatedRecord).SetRecordProperties(recordDetails);
                        updatedRecords.Add(updatedRecord);
                        entityResponse.Data = updatedRecord;
                    }
                    else
                    {
                        entityResponse.Data = null;
                    }
                }
                response.BulkData = updatedRecords;
                return response;
            }catch (Exception e)
            {
                //TODO: Log the info;
                Console.WriteLine("Exception at updateRecords");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        public BulkAPIResponse<ZCRMRecord> UpsertRecords(List<ZCRMRecord> records)
        {
            if (records.Count > 100)
            {
                throw new ZCRMException("Cannot process more than 100 records at a time");
            }
            try
            {
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = module.ApiName + "/upsert";
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                foreach (ZCRMRecord record in records)
                {
                    JObject recordJSON = EntityAPIHandler.GetInstance(record).GetZCRMRecordAsJSON();
                    recordJSON.Add("id", record.EntityId);
                    dataArray.Add(recordJSON);
                }
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;
                BulkAPIResponse<ZCRMRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMRecord>();

                List<ZCRMRecord> upsertedRecords = new List<ZCRMRecord>();
                List<EntityResponse> responses = response.BulkEntitiesResponse;
                int responseSize = responses.Count;
                for (int i = 0; i < responseSize; i++)
                {
                    EntityResponse entityResponse = responses[i];
                    if (entityResponse.Status.Equals("success"))
                    {
                        JObject responseData = entityResponse.ResponseJSON;
                        JObject recordDetails = (JObject)responseData.GetValue("details");
                        ZCRMRecord record = records[i];
                        EntityAPIHandler.GetInstance(record).SetRecordProperties(recordDetails);
                        upsertedRecords.Add(record);
                        entityResponse.Data = record;
                    }
                    else
                    {
                        entityResponse.Data = null;
                    }
                }
                response.BulkData = upsertedRecords;
                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the info;
                Console.WriteLine("Exception at UpsertedRecords");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }

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

                BulkAPIResponse<ZCRMRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMRecord>();

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
                Console.WriteLine("Exception in MassEntityAPIHandler.GetRecords");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNA_ERROR", e.ToString());
            }
        }

        //TODO:Handle exceptions;
        //TODO: Inspect the working of Generics;
        public BulkAPIResponse<ZCRMEntity> DeleteRecords(List<long> entityIds)
        {
            if(entityIds.Count > 100)
            {
                throw new ZCRMException("Cannot process more than 100 records at a time");
            }
            try{
                requestMethod = APIConstants.RequestMethod.DELETE;
                urlPath = module.ApiName;
                requestQueryParams.Add("ids", CommonUtil.CollectionToCommaDelimitedString(entityIds));

                BulkAPIResponse<ZCRMEntity> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMEntity>();

                List<EntityResponse> responses = response.BulkEntitiesResponse;
                foreach(EntityResponse entityResponse in responses)
                {
                    JObject entityResponseJSON = entityResponse.ResponseJSON;
                    JObject recordJSON = (JObject)entityResponseJSON.GetValue("details");
                    ZCRMRecord record = ZCRMRecord.GetInstance(module.ApiName, Convert.ToInt64(recordJSON.GetValue("id")));
                    entityResponse.Data = record;
                }
                return response;
            }catch(Exception){
                //TODO: Log the info;
                //TODO: Inspect the throw statement and compare the parameters with java sdk;
                Console.WriteLine("Exception occured in DeleteRecords");
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        public BulkAPIResponse<ZCRMTrashRecord> GetAllDeletedRecords()
        {
            return GetDeletedRecords("all");
        }

        public BulkAPIResponse<ZCRMTrashRecord> GetRecycleBinRecords()
        {
            return GetDeletedRecords("recycle");
        }

        public BulkAPIResponse<ZCRMTrashRecord> GetPermanentlyDeletedRecords()
        {
            return GetDeletedRecords("permanent");
        }

        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMTrashRecord> GetDeletedRecords(string type)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = module.ApiName + "/deleted";
                requestQueryParams.Add("type", type);

                BulkAPIResponse<ZCRMTrashRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMTrashRecord>();

                List<ZCRMTrashRecord> trashRecordList = new List<ZCRMTrashRecord>();
                JObject responseJSON = response.ResponseJSON;
                JArray trashRecordsArray = (JArray)responseJSON.GetValue("data");
                foreach (JObject trashRecordDetails in trashRecordsArray)
                {
                    trashRecord = ZCRMTrashRecord.GetInstance(Convert.ToString(trashRecordDetails.GetValue("type")), Convert.ToInt64(trashRecordDetails.GetValue("id")));
                    SetTrashRecordProperties(trashRecordDetails);
                    trashRecordList.Add(trashRecord);
                }
                response.BulkData = trashRecordList;
                return response;
            }catch(Exception e){

                //TODO: Log the exceptions;
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        //TODO: Handle Exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByText(string searchText, int page, int perPage)
        {
            return SearchRecords("word", searchText, page, perPage);
        }

        //TODO: Handle Exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByCriteria(string searchCriteria, int page, int perPage)
        {
            return SearchRecords("criteria", searchCriteria, page, perPage);
        }


        //TODO: Handle Exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByEmail(string searchValue, int page, int perPage)
        {
            return SearchRecords("email", searchValue, page, perPage);
        }


        //TODO: Handle Exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByPhone(string searchValue, int page, int perPage)
        {
            return SearchRecords("phone", searchValue, page, perPage);
        }

        private BulkAPIResponse<ZCRMRecord> SearchRecords(string searchKey, string searchValue, int page, int perPage)
        {
            try{
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = module.ApiName + "/search";
                requestQueryParams.Add(searchKey, searchValue);
                requestQueryParams.Add("page", page.ToString());
                requestQueryParams.Add("per_page", perPage.ToString());

                BulkAPIResponse<ZCRMRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMRecord>();

                List<ZCRMRecord> recordsList = new List<ZCRMRecord>();
                JObject responseJSON = response.ResponseJSON;
                JArray recordsArray = (JArray)responseJSON.GetValue("data");
                foreach(JObject recordDetails in recordsArray)
                {
                    ZCRMRecord record = ZCRMRecord.GetInstance(module.ApiName, Convert.ToInt64(recordDetails.GetValue("id")));
                    EntityAPIHandler.GetInstance(record).SetRecordProperties(recordDetails);
                    recordsList.Add(record);
                }

                response.BulkData = recordsList;
                return response;
            }catch(Exception e){
                //TODO: Log the exception;
                Console.WriteLine("Exception caught at searchRecords");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR"); 
            }
            
        }

        private void SetTrashRecordProperties(JObject trashRecordDetails)
        {
            foreach(KeyValuePair<string, JToken> trashRecordDetail in trashRecordDetails)
            {
                string fieldAPIName = Convert.ToString(trashRecordDetail.Value);
                if(fieldAPIName.Equals("Created_By"))
                {
                    JObject createdByObject = (JObject)trashRecordDetail.Value;
                    ZCRMUser createdUser = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject.GetValue("id")), Convert.ToString(createdByObject.GetValue("name")));
                    trashRecord.CreatedBy = createdUser;
                }
                else if(fieldAPIName.Equals("deleted_by"))
                {
                    JObject modifiedByObject = (JObject)trashRecordDetail.Value;
                    ZCRMUser DeletedByUser = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
                    trashRecord.CreatedBy = DeletedByUser;
                }
                else if(fieldAPIName.Equals("display_name"))
                {
                    trashRecord.DisplayName = Convert.ToString(trashRecordDetail.Value);
                }
                else if(fieldAPIName.Equals("deleted_time"))
                {
                    trashRecord.DeletedTime = Convert.ToString(trashRecordDetail.Value);
                }
            }
        }
    }
}
