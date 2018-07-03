﻿using System;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Api.Handler;
using SampleSDK.CRM.Library.Setup.Users;
using SampleSDK.CRM.Library.CRMException;

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





        //TODO: Handle exceptions
        //TODO: Inspect the usage of this function;
        public BulkAPIResponse<ZCRMModuleRelation> GetRelatedLists()
        {
            return ModuleAPIHandler.GetInstance(this).GetAllRelatedLists();
        }


        //TODO: Handle exceptions
        public BulkAPIResponse<ZCRMField> GetAllFields()
        {
            return GetAllFields(null);
        }


        //TODO: Handle exceptions
        public BulkAPIResponse<ZCRMField> GetAllFields(string modifiedSince)
        {
            return ModuleAPIHandler.GetInstance(this).GetAllFields(modifiedSince);
        }


        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMLayout> GetLayouts()
        {
            return GetLayouts(null);
        }



        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMLayout> GetLayouts(string modifiedSince)
        {
            return ModuleAPIHandler.GetInstance(this).GetAllLayouts(modifiedSince);
        }

        //TODO: Handle exceptions;
        public APIResponse GetLayoutDetails(long layoutId)
        {
            return ModuleAPIHandler.GetInstance(this).GetLayoutDetails(layoutId);
        }


        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMCustomView> GetCustomViews()
        {
            return GetCustomViews(null);
        }



        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMCustomView> GetCustomViews(string modifiedSince)
        {
            return ModuleAPIHandler.GetInstance(this).GetAllCustomViews(modifiedSince);
        }



        //TODO: Handle exceptions;
        public APIResponse GetCustomView(long cvId)
        {
            return ModuleAPIHandler.GetInstance(this).GetCustomView(cvId);
        }



        public BulkAPIResponse<ZCRMRecord> CreateRecords(List<ZCRMRecord> records)
        {
            if (records == null || records.Count == 0)
            {
                throw new ZCRMException(" Records list MUST NOT be null for Create operation");
            }
            return MassEntityAPIHandler.GetInstance(this).CreateRecords(records);
        }


        public BulkAPIResponse<ZCRMRecord> UpdateRecords(List<long> entityIds, string fieldAPIName, object value)
        {
            if (entityIds == null || entityIds.Count == 0)
            {
                throw new ZCRMException("Entity ID list MUST NOT be null/empty for update operation");
            }
            return MassEntityAPIHandler.GetInstance(this).UpdateRecords(entityIds, fieldAPIName, value);
        }



        public BulkAPIResponse<ZCRMRecord> UpsertRecords(List<ZCRMRecord> records)
        {
            if(records == null || records.Count == 0)
            {
                throw new ZCRMException("Record ID list MUST NOT be null/empty for upsert operation");
            }
            return MassEntityAPIHandler.GetInstance(this).UpsertRecords(records);
        }


        public BulkAPIResponse<ZCRMEntity> DeleteRecords(List<long> entityIds)
        {
            if (entityIds == null || entityIds.Count == 0)
            {
                throw new ZCRMException("Entity ID list MUST NOT be null/empty for delete operation");
            }
            return MassEntityAPIHandler.GetInstance(this).DeleteRecords(entityIds);
        }


        public APIResponse GetRecord(long entityId)
        {
            ZCRMRecord record = ZCRMRecord.GetInstance(ApiName, entityId);
            return EntityAPIHandler.GetInstance(record).GetRecord();
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords()
        {
            return GetRecords(null, null);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords(long? cvId)
        {
            return GetRecords(cvId, null);
        }



        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords(List<string> fields)
        {
            return GetRecords(null, fields);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords(long? cvId, List<string> fields)
        {
            return GetRecords(cvId, 1, 200, fields);
        }




        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords(long? cvId, int page, int perPage, List<string> fields)
        {
            return GetRecords(cvId, null, null, page, perPage, fields);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, List<string> fields)
        {
            return GetRecords(cvId, sortByField, sortOrder, 1, 200,  fields);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, List<string> fields)
        {
            return GetRecords(cvId, sortByField, sortOrder, page, perPage, null, fields);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince, List<string> fields)
        {
            return MassEntityAPIHandler.GetInstance(this).GetRecords(cvId, sortByField, sortOrder, page, perPage, modifiedSince, null, null, fields);
        }



        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetConvertedRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince, List<string> fields)
        {
            return MassEntityAPIHandler.GetInstance(this).GetRecords(cvId, sortByField, sortOrder, page, perPage, modifiedSince, "true", null, fields);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetUnConvertedRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince, List<string> fields)
        {
            return MassEntityAPIHandler.GetInstance(this).GetRecords(cvId, sortByField, sortOrder, page, perPage, modifiedSince, "false", null , fields);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetApprovedRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince, List<string> fields)
        {
            return MassEntityAPIHandler.GetInstance(this).GetRecords(cvId, sortByField, sortOrder, page, perPage, modifiedSince, null, "true", fields);
        }

        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMRecord> GetUnApprovedRecords(long? cvId, string sortByField, CommonUtil.SortOrder? sortOrder, int page, int perPage, string modifiedSince, List<string> fields)
        {
            return MassEntityAPIHandler.GetInstance(this).GetRecords(cvId, sortByField, sortOrder, page, perPage, modifiedSince, null, "false", fields);
        }


        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMTrashRecord> GetAllDeletedRecords()
        {
            return MassEntityAPIHandler.GetInstance(this).GetAllDeletedRecords();
        }

        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMTrashRecord> GetRecycleBinRecords()
        {
            return MassEntityAPIHandler.GetInstance(this).GetRecycleBinRecords();
        }

        //TODO: Handle Exceptions
        public BulkAPIResponse<ZCRMTrashRecord> GetPermanentlyDeletedRecords()
        {
            return MassEntityAPIHandler.GetInstance(this).GetPermanentlyDeletedRecords();
        }


        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByText(string value)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByText(value, 1, 200);
        }

        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByText(string value, int page, int perPage)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByText(value, page, perPage);
        }



        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByCriteria(string value)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByCriteria(value, 1, 200);
        }



        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByCriteria(string value, int page, int perPage)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByCriteria(value, page, perPage);
        }


        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByPhone(string value)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByPhone(value, 1, 200);
        }


        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByPhone(string value, int page, int perPage)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByPhone(value, page, perPage);
        }


        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByEmail(string value)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByEmail(value, 1, 200);
        }

        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMRecord> SearchByEmail(string value, int page, int perPage)
        {
            return MassEntityAPIHandler.GetInstance(this).SearchByEmail(value, page, perPage);
        }

        public void AddAccessibleProfiles(ZCRMProfile profile)
        {
            AccessibleProfiles.Add(profile);
        }



    }
}
