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
    public class RelatedListAPIHandler : CommonAPIHandler, IAPIHandler
    {

        //NOTE: Properties are not used. Instead fields are used since publicly they are not accessed;

        private ZCRMRecord parentRecord;
        private ZCRMModuleRelation relatedList;
        private ZCRMJunctionRecord junctionRecord;

        private RelatedListAPIHandler(ZCRMRecord parentRecord, ZCRMModuleRelation relatedList)
        {
            this.parentRecord = parentRecord;
            this.relatedList = relatedList;
        }

        public static RelatedListAPIHandler GetInstance(ZCRMRecord parentRecord, ZCRMModuleRelation relatedList)
        {
            return new RelatedListAPIHandler(parentRecord, relatedList);
        }

        private RelatedListAPIHandler(ZCRMRecord parentRecord, ZCRMJunctionRecord junctionRecord)
        {
            this.parentRecord = parentRecord;
            this.junctionRecord = junctionRecord;
        }

        public static RelatedListAPIHandler GetInstance(ZCRMRecord parentRecord, ZCRMJunctionRecord junctionRecord)
        {
            return new RelatedListAPIHandler(parentRecord, junctionRecord);
        }

        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> GetRecords(string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}";
                requestQueryParams.Add("sort_by", sortByField);
                if (sortOrder != null)
                {
                    requestQueryParams.Add("sort_order", sortOrder.ToString());
                }
                requestQueryParams.Add("page", page.ToString());
                requestQueryParams.Add("per_page", perPage.ToString());
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMRecord> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMRecord>();

                List<ZCRMRecord> recordsList = new List<ZCRMRecord>();
                JObject responseJSON = response.ResponseJSON;
                JArray recordsArray = (JArray)responseJSON.GetValue("data");
                foreach (JObject recordDetails in recordsArray)
                {
                    ZCRMRecord record = ZCRMRecord.GetInstance(relatedList.ApiName, Convert.ToInt64(recordDetails.GetValue("id")));
                    EntityAPIHandler.GetInstance(record).SetRecordProperties(recordDetails);
                    recordsList.Add(record);
                }
                response.BulkData = recordsList;
                return response;
            }
            catch (Exception e)
            {
                //TODO: Handle the exceptions;
                Console.WriteLine("Exception in GetRecords");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }


        public BulkAPIResponse<ZCRMNote> GetNotes(string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince)
        {
            try{
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}";
                requestQueryParams.Add("sort_by", sortByField);
                if (sortOrder != null)
                {
                    requestQueryParams.Add("sort_order", sortOrder.ToString());
                }
                requestQueryParams.Add("page", page.ToString());
                requestQueryParams.Add("per_page", perPage.ToString());
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMNote> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMNote>();

                List<ZCRMNote> allNotes = new List<ZCRMNote>();
                JObject responseJSON = response.ResponseJSON;
                JArray notesArray = (JArray)responseJSON.GetValue("data");
                foreach (JObject noteDetails in notesArray)
                {
                    allNotes.Add(GetZCRMNote(noteDetails, null));
                }
                response.BulkData = allNotes;
                return response;
            }catch(Exception e){
                //TODO: Log the exception;
                Console.WriteLine("Exception in GetNotes");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        //TODO: Handle Exceptions;
        public APIResponse AddNote(ZCRMNote note)
        {
            try{
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}";
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                dataArray.Add(GetZCRMNoteAsJSON(note));
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;


                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject responseData = (JObject)responseDataArray[0];
                JObject responseDetails = (JObject)responseData.GetValue("details");
                note = GetZCRMNote(responseDetails, note);
                response.Data = note;
                return response;
            }catch(Exception e){
                //TODO: Log the exception;
                Console.WriteLine("Exception in Addnote");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }

        }


        public APIResponse UpdateNote(ZCRMNote note)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.PUT;
                urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}";
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                dataArray.Add(GetZCRMNoteAsJSON(note));
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;


                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject responseData = (JObject)responseDataArray[0];
                JObject responseDetails = (JObject)responseData.GetValue("details");
                note = GetZCRMNote(responseDetails, note);
                response.Data = note;
                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("Exception in Updatenote");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }


        public APIResponse DeleteNote(ZCRMNote note)
        {
            requestMethod = APIConstants.RequestMethod.DELETE;
            urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}/{note.Id}";

            APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

            return response;
        }


        //TODO: Handle Exception;
        public BulkAPIResponse<ZCRMAttachment> GetAllAttachmentDetails(int page, int perPage, string modifiedSince)
        {
            try{
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}";
                requestQueryParams.Add("page", page.ToString());
                requestQueryParams.Add("per_page", perPage.ToString());
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMAttachment> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMAttachment>();

                List<ZCRMAttachment> allAttachments = new List<ZCRMAttachment>();
                JObject responseJSON = response.ResponseJSON;
                JArray attachmentsArray = (JArray)responseJSON.GetValue("data");
                foreach (JObject attachmentDetails in attachmentsArray)
                {
                    allAttachments.Add(GetZCRMAttachment(attachmentDetails));
                }
                response.BulkData = allAttachments;
                return response;
            }catch (Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("Exception in GetAllAttachments");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        //TODO: Handle Exceptions <IMPORTANT = Handle UploadFile method in APIRequest>;
        /*public APIResponse UploadAttachment(string filePath)
        {
            //TODO: Validate File <IMPORTANT>
            try
            {
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}";

                APIResponse response = APIRequest.GetInstance(this).UploadFile(filePath);

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject responseData = (JObject)responseDataArray[0];
                JObject responseDetails = (JObject)responseData.GetValue("details");
                response.Data = GetZCRMAttachment(responseDetails);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("Exception in Addnote");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }*/

        public APIResponse UploadLinkAsAttachment(string attachmentUrl)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}";
                requestQueryParams.Add("attachmentUrl", attachmentUrl);

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject responseData = (JObject)responseDataArray[0];
                JObject responseDetails = (JObject)responseData.GetValue("details");

                response.Data = GetZCRMAttachment(responseDetails);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("Exception in Addnote");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
            
        }

        /*public FileAPIResponse DownloadAttachment(long attachmentId)
        {
            requestMethod = APIConstants.RequestMethod.GET;
            urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}/{attachmentId}";

            return APIRequest.GetInstance(this).DownloadFile();
        } */


        //TODO: Handle Exceptions
        public APIResponse DeleteAttachment(long attachmentId)
        {
            requestMethod = APIConstants.RequestMethod.DELETE;
            urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{relatedList.ApiName}/{attachmentId}";
            return APIRequest.GetInstance(this).GetAPIResponse();
        }



        private ZCRMNote GetZCRMNote(JObject noteDetails, ZCRMNote note)
        {
            if (note == null)
            {
                note = ZCRMNote.GetInstance(parentRecord, Convert.ToInt64(noteDetails.GetValue("id")));
            }
            note.Id = Convert.ToInt64(noteDetails.GetValue("id"));
            if (noteDetails.GetValue("Note_Title") != null)
            {
                note.Title = Convert.ToString(noteDetails.GetValue("Note_Title"));
            }
            if (noteDetails.GetValue("Note_Content") != null)
            {
                note.Content = Convert.ToString(noteDetails.GetValue("Note_Content"));
            }
            JObject createdByObject = (JObject)noteDetails.GetValue("Created_By");
            ZCRMUser createdBy = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject.GetValue("id")), Convert.ToString(createdByObject.GetValue("name")));
            note.CreatedBy = createdBy;
            note.CreatedTime = Convert.ToString(noteDetails.GetValue("Created_Time"));

            JObject modifiedByObject = (JObject)noteDetails.GetValue("Modified_By");
            ZCRMUser modifiedBy = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
            note.ModifiedBy = modifiedBy;
            note.ModifiedTime = Convert.ToString(noteDetails.GetValue("Modified_Time"));

            if (noteDetails.GetValue("Owner") != null)
            {
                JObject ownerObject = (JObject)noteDetails.GetValue("Owner");
                ZCRMUser owner = ZCRMUser.GetInstance(Convert.ToInt64(ownerObject.GetValue("id")), Convert.ToString(ownerObject.GetValue("name")));
                note.NotesOwner = owner;
            }
            else
            {
                note.NotesOwner = createdBy;
            }
            if (noteDetails.GetValue("$attachments") != null)
            {
                JArray attachmentsArray = (JArray)noteDetails.GetValue("$attachments");
                foreach (JObject attachmentDetails in attachmentsArray)
                {
                    note.AddAttachment(GetZCRMAttachment(attachmentDetails));
                }
            }
            return note;
        }




        private JObject GetZCRMNoteAsJSON(ZCRMNote note)
        {
            JObject noteJSON = new JObject();
            if(note.Title != null)
            {
                noteJSON.Add("Note_Title", note.Title);
            }
            else
            {
                noteJSON.Add("Note_Title", null);
            }
            noteJSON.Add("Note_Content", note.Content);

            return noteJSON;
        }


        //TODO: Handle Exceptions;
        private ZCRMAttachment GetZCRMAttachment(JObject attachmentDetails)
        {
            ZCRMAttachment attachment = ZCRMAttachment.GetInstance(parentRecord, Convert.ToInt64(attachmentDetails.GetValue("id")));
            string fileName = Convert.ToString(attachmentDetails.GetValue("File_Name"));
            if(fileName != null)
            {
                attachment.FileName = fileName;
                attachment.FileType = fileName.Substring(fileName.LastIndexOf('.') + 1);
            }
            if(attachmentDetails.ContainsKey("Size"))
            {
                attachment.Size = Convert.ToInt64(attachmentDetails.GetValue("Size"));
            }
            if (attachmentDetails.ContainsKey("Created_By"))
            {
                JObject createdByObject = (JObject)attachmentDetails.GetValue("Created_By");
                ZCRMUser createdBy = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject.GetValue("id")), Convert.ToString(createdByObject.GetValue("name")));
                attachment.CreatedBy = createdBy;
                attachment.CreatedTime = Convert.ToString(attachmentDetails.GetValue("Created_Time"));

                if (attachmentDetails.GetValue("Owner") != null)
                {
                    JObject ownerObject = (JObject)attachmentDetails.GetValue("Owner");
                    ZCRMUser owner = ZCRMUser.GetInstance(Convert.ToInt64(ownerObject.GetValue("id")), Convert.ToString(ownerObject.GetValue("name")));
                    attachment.Owner = owner;
                }
                else
                {
                    attachment.Owner = createdBy;
                }
            }
            if (attachmentDetails.ContainsKey("Modified_By"))
            {
                JObject modifiedByObject = (JObject)attachmentDetails.GetValue("Modified_By");
                ZCRMUser modifiedBy = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
                attachment.ModifiedBy = modifiedBy;
                attachment.ModifiedTime = Convert.ToString(attachmentDetails.GetValue("Modified_Time"));
            }

            return attachment;
        }

        //TODO: Handle Exceptions;
        public APIResponse AddRelation()
        {
            requestMethod = APIConstants.RequestMethod.PUT;
            urlPath = $"{parentRecord.ModuleAPIName}/{parentRecord.EntityId}/{junctionRecord.ApiName}/{junctionRecord.Id}";
            JObject requestBodyObject = new JObject();
            JArray dataArray = new JArray();
            dataArray.Add(GetRelationDetailsAsJSON(junctionRecord.RelatedDetails));
            requestBodyObject.Add("data", dataArray);
            requestBody = requestBodyObject;

            return APIRequest.GetInstance(this).GetAPIResponse();
        }

        private JObject GetRelationDetailsAsJSON(Dictionary<string, object> relatedDetails)
        {
            JObject relatedDetailsJSON = new JObject();
            foreach(KeyValuePair<string, object> keyValuePairs in relatedDetails)
            {
                //TODO: Didn't check null value;
                object value = keyValuePairs.Value;
                if(value is long)
                {
                    value = Convert.ToString(value);
                }
                relatedDetailsJSON.Add(keyValuePairs.Key, (JToken)value);
            }
            return relatedDetailsJSON;
        }
    }
}
