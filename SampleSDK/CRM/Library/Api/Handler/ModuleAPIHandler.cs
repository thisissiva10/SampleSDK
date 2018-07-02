using System;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.CRUD;
using SampleSDK.CRM.Library.CRMException;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Setup.Users;
using System.Collections.Generic;

namespace SampleSDK.CRM.Library.Api.Handler
{
    public class ModuleAPIHandler : CommonAPIHandler, IAPIHandler
    {
        private ZCRMModule module;

        private ModuleAPIHandler(ZCRMModule zcrmModule)
        {
            module = zcrmModule;
        }

        public static ModuleAPIHandler GetInstance(ZCRMModule zcrmModule)
        {
            return new ModuleAPIHandler(zcrmModule);
        }


        //TODO: Handle Exception;
        public void GetModuleDetails()
        {
            module = (ZCRMModule)MetaDataAPIHandler.GetInstance().GetModule(module.ApiName).Data;
        }

        //TODO: Handle Exceptions;
        public APIResponse GetLayoutDetails(long layoutId)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/layouts/" + layoutId;
                requestQueryParams.Add("module", module.ApiName);

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JObject responseJSON = response.ResponseJSON;
                JArray layoutsArray = (JArray)responseJSON.GetValue("layouts");
                response.Data = GetZCRMLayout((JObject)layoutsArray[0]);
                return response;
            }catch(Exception e)
            {
                //TODO: Log the info;
                Console.WriteLine("Exception caught in Get Layout Details");
                throw new ZCRMException(e.ToString());
            }
        }

        //TODO: Handle all exceptions;
        public BulkAPIResponse<ZCRMLayout> GetAllLayouts(string modifiedSince)
        {
            try{

                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/layouts/";
                requestQueryParams.Add("module", module.ApiName);
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMLayout> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMLayout>();

                response.BulkData = GetAllLayouts(response.ResponseJSON);
                return response;
            }catch(Exception e)
            {
                //TODO: Throw the exception and log the exceptions;
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e);
                throw new ZCRMException(e.Message);
            }
        }

        //TODO: Handle exceptions;
        public BulkAPIResponse<ZCRMField> GetAllFields(string modifiedSince)
        {
            try
            {

                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/fields/";
                requestQueryParams.Add("module", module.ApiName);
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMField> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMField>();

                response.BulkData = GetAllFields(response.ResponseJSON);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Throw the exception and log the exceptions;
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e);
                throw new ZCRMException(e.Message);
            }
            
        }

        //TODO: Handle all exceptions and test the method;
        public BulkAPIResponse<ZCRMCustomView> GetAllCustomViews(string modifiedSince)
        {
            try
            {

                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/fields/";
                requestQueryParams.Add("module", module.ApiName);
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMCustomView> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMCustomView>();

                response.BulkData = GetAllCustomViews(response.ResponseJSON);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Throw the exception and log the exceptions;
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e);
                throw new ZCRMException(e.Message);
            }

        }

        //TODO: Handle Exceptions;
        public APIResponse GetCustomView(long cvId)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/custom_views/" + cvId;
                requestQueryParams.Add("module", module.ApiName);

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JObject responseJSON = response.ResponseJSON;
                JArray layoutsArray = (JArray)responseJSON.GetValue("custom_views");
                response.Data = GetZCRMCustomView((JObject)layoutsArray[0]);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the info;
                Console.WriteLine("Exception caught in Get CustomView");
                throw new ZCRMException(e.ToString());
            }
        }

        //TODO: Handle all exceptions and test the method;
        public BulkAPIResponse<ZCRMModuleRelation> GetAllRelatedLists()
        {
            try
            {

                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/related_lists";
                requestQueryParams.Add("module", module.ApiName);

                BulkAPIResponse<ZCRMModuleRelation> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMModuleRelation>();

                response.BulkData = GetAllRelatedLists(response.ResponseJSON);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Throw the exception and log the exceptions;
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e);
                throw new ZCRMException(e.Message);
            }

        }

        //TODO: Handle Exceptions;
        private ZCRMLayout GetZCRMLayout(JObject layoutDetails)
        {
            ZCRMLayout layout = ZCRMLayout.GetInstance(Convert.ToInt64(layoutDetails.GetValue("id")));
            layout.Name = Convert.ToString(layoutDetails.GetValue("name"));
            layout.Visible = Convert.ToBoolean(layoutDetails.GetValue("visible"));
            layout.Status = Convert.ToInt32(layoutDetails.GetValue("status"));
            if(layoutDetails.GetValue("created_by").HasValues)
            {
                JObject createdByObject = (JObject)layoutDetails.GetValue("created_by");
                ZCRMUser createdUser = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject.GetValue("id")), Convert.ToString(createdByObject.GetValue("name")));
                layout.CreatedBy = createdUser;
                layout.CreatedTime = Convert.ToString("created_time");
            }
            if (layoutDetails.GetValue("modified_by").HasValues)
            {
                JObject modifiedByObject = (JObject)layoutDetails.GetValue("modified_by");
                ZCRMUser modifiedUser = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
                layout.ModifiedBy = modifiedUser;
                layout.ModifiedTime = Convert.ToString("modified_time");
            }
            JArray accessibleProfilesArray = (JArray)layoutDetails.GetValue("profiles");
            foreach(JObject profileObject in accessibleProfilesArray)
            {
                ZCRMProfile profile = ZCRMProfile.GetInstance(Convert.ToInt64(profileObject.GetValue("id")), Convert.ToString(profileObject.GetValue("name")));
                layout.AddAccessibleProfiles(profile);
            }
            layout.Sections = GetAllSectionsofLayout(layoutDetails);
            return layout;
            
        }

        //TODO: Check this method and the token object and take a look into the precision field;
        private ZCRMField GetZCRMField(JObject fieldJSON)
        {
            ZCRMField field = ZCRMField.GetInstance(Convert.ToString(fieldJSON.GetValue("api_name")));
            field.DefaultValue = (object)fieldJSON.GetValue("default_value");
            field.Mandatory = Convert.ToBoolean(fieldJSON.GetValue("required"));
            field.Id = Convert.ToInt64(fieldJSON.GetValue("id"));
            field.CustomField = Convert.ToBoolean(fieldJSON.GetValue("custom_field"));
            field.DataType = Convert.ToString(fieldJSON.GetValue("data_type"));
            field.DisplayName = Convert.ToString(fieldJSON.GetValue("field_label"));
            field.MaxLength = Convert.ToInt32(fieldJSON.GetValue("length"));
            field.Precision = Convert.ToInt32(fieldJSON.GetValue("decimal_place"));
            field.ReadOnly = Convert.ToBoolean(fieldJSON.GetValue("read_only"));
            field.Visible = Convert.ToBoolean(fieldJSON.GetValue("visible"));
            field.SequenceNo = Convert.ToInt32(fieldJSON.GetValue("sequence_number"));
            field.ToolTip = Convert.ToString(fieldJSON.GetValue("tooltip"));
            field.Webhook = Convert.ToBoolean(fieldJSON.GetValue("webhook"));
            field.CreatedSource = Convert.ToString(fieldJSON.GetValue("created_source"));
            JToken tempJSONObect = fieldJSON.GetValue("formula");
            if (tempJSONObect.HasValues)
            {
                field.FormulaReturnType = Convert.ToString(fieldJSON.GetValue("return_type"));
            }
            tempJSONObect = fieldJSON.GetValue("currency");
            if (tempJSONObect.HasValues)
            {
                field.Precision = Convert.ToInt32(fieldJSON.GetValue("precision"));
            }
            JObject subLayouts = (JObject)fieldJSON.GetValue("view_type");
            if (subLayouts != null)
            {
                List<string> layoutsPresent = new List<string>();
                if (Convert.ToBoolean(subLayouts.GetValue("create")))
                {
                    layoutsPresent.Add("CREATE");
                }
                if (Convert.ToBoolean(subLayouts.GetValue("view")))
                {
                    layoutsPresent.Add("VIEW");
                }
                if (Convert.ToBoolean(subLayouts.GetValue("quick_create")))
                {
                    layoutsPresent.Add("QUICK_CREATE");
                }
                field.SubLayoutsPresent = layoutsPresent;
            }

            JArray pickList = (JArray)fieldJSON.GetValue("pick_list_values");
            foreach (JObject pickListObject in pickList)
            {
                ZCRMPickListValue pickListValue = ZCRMPickListValue.GetInstance();
                pickListValue.DisplayName = Convert.ToString(fieldJSON.GetValue("display_value"));
                pickListValue.ActualName = Convert.ToString(fieldJSON.GetValue("actual_value"));
                pickListValue.SequenceNumber = Convert.ToInt32(fieldJSON.GetValue("sequence_number"));
                pickListValue.Maps = (JArray)fieldJSON.GetValue("maps");
                field.AddPickListValue(pickListValue);
            }
            JObject lookup = (JObject)fieldJSON.GetValue("lookup");
            foreach (KeyValuePair<string, JToken> lookupObject in lookup)
            {
                field.SetLookupDetails(lookupObject.Key, (object)lookupObject.Value);
            }
            JObject multilookup = (JObject)fieldJSON.GetValue("multiselectlookup");
            foreach (KeyValuePair<string, JToken> multiLookupObject in multilookup)
            {
                field.SetMultiselectLookup(multiLookupObject.Key, (object)multiLookupObject.Value);
            }
            if (fieldJSON.ContainsKey("subformtabId"))
            {
                field.SubFormTabId = Convert.ToInt64(fieldJSON.GetValue("subformtabid"));
            }
            if (fieldJSON.ContainsKey("subform"))
            {
                JObject subformDetails = (JObject)fieldJSON.GetValue("subform");
                foreach (KeyValuePair<string, JToken> subformDetail in subformDetails)
                {
                    field.SetSubformDetails(subformDetail.Key, (object)subformDetail.Value);
                }
            }
            return field;

        }

        //TODO: Handle Excepiton;
        private ZCRMSection GetZCRMSection(JObject sectionJSON)
        {
            ZCRMSection section = ZCRMSection.GetInstance(Convert.ToString(sectionJSON.GetValue("name")));
            section.ColumnCount = Convert.ToInt32(sectionJSON.GetValue("column_count"));
            section.DisplayName = Convert.ToString(sectionJSON.GetValue("display_label"));
            section.Sequence = Convert.ToInt32(sectionJSON.GetValue("sequence_number"));
            section.Fields = GetAllFields(sectionJSON);
            return section;
        }

        //TODO: Handle exceptions appropriately;
        private ZCRMCustomView GetZCRMCustomView(JObject customViewObject)
        {
            ZCRMCustomView customView = ZCRMCustomView.GetInstance(module.ApiName, Convert.ToInt64(customViewObject.GetValue("id")));
            customView.DisplayName = Convert.ToString(customViewObject.GetValue("disolay_value"));
            customView.Isdefault = Convert.ToBoolean(customViewObject.GetValue("default"));
            customView.SystemName = Convert.ToString(customViewObject.GetValue("system_name"));
            customView.Category = Convert.ToString(customViewObject.GetValue("category"));
            if (customViewObject.GetValue("favourite") != null)
            {
                customView.Favourite = Convert.ToInt32(customViewObject.GetValue("favourite"));
            }
            customView.Name = Convert.ToString(customViewObject.GetValue("name"));
            customView.SortBy = Convert.ToString(customViewObject.GetValue("sort_by"));
            if (customViewObject.GetValue("sort_order") != null)
            {
                customView.SortOrder = (CommonUtil.SortOrder)Enum.Parse(typeof(CommonUtil.SortOrder), Convert.ToString(customViewObject.GetValue("sort_order")));
            }
            List<string> fields = new List<string>();
            if (customViewObject.ContainsKey("fields"))
            {
                JArray fieldsArray = (JArray)customViewObject.GetValue("fields");
                foreach (string fieldObject in fieldsArray)
                {
                    fields.Add(fieldObject);
                }
            }
            customView.Fields = fields;
            return customView;
        }

        private ZCRMModuleRelation GetZCRMModuleRelation(JObject relatedList)
        {
            ZCRMModuleRelation moduleRelation = ZCRMModuleRelation.GetInstance(module.ApiName, Convert.ToInt64(relatedList.GetValue("id")));
            moduleRelation.ApiName = Convert.ToString(relatedList.GetValue("api_name"));
            moduleRelation.Label = Convert.ToString(relatedList.GetValue("display_label"));
            moduleRelation.Module = Convert.ToString(relatedList.GetValue("module"));
            moduleRelation.Type = Convert.ToString(relatedList.GetValue("type"));
            return moduleRelation;
        }

        //TODO: Throws Exception;
        private List<ZCRMSection> GetAllSectionsofLayout(JObject layoutDetails)
        {
            List<ZCRMSection> sections = new List<ZCRMSection>();
            JArray sectionsArray = (JArray)layoutDetails.GetValue("sections");
            foreach(JObject section in sectionsArray)
            {
                sections.Add(GetZCRMSection(section));
            }
            return sections;
        }

        //TODO: Handle all exceptions;
        public List<ZCRMLayout> GetAllLayouts(JObject layoutsJSONObject)
        {
            List<ZCRMLayout> allLayouts = new List<ZCRMLayout>();
            JArray layoutsArray = (JArray)layoutsJSONObject.GetValue("layouts");
            foreach(JObject layoutObject in layoutsArray)
            {
                allLayouts.Add(GetZCRMLayout(layoutObject));
            }
            return allLayouts;
        }

        private List<ZCRMField> GetAllFields(JObject sectionJSON)
        {
            List<ZCRMField> fields = new List<ZCRMField>();
            JArray fieldsArray = (JArray)sectionJSON.GetValue("fields");
            foreach(JObject fieldObject in fieldsArray)
            {
                fields.Add(GetZCRMField(fieldObject));
            }
            return fields;
        }

        private List<ZCRMCustomView> GetAllCustomViews(JObject customviewJSON)
        {
            List<ZCRMCustomView> allCustomViews = new List<ZCRMCustomView>();
            JArray customViewsArray = (JArray)customviewJSON.GetValue("custom_views");
            foreach (JObject customViewObject in customViewsArray)
            {
                allCustomViews.Add(GetZCRMCustomView(customViewObject));
            }
            return allCustomViews;
        }

        private List<ZCRMModuleRelation> GetAllRelatedLists(JObject responseJSON)
        {
            List<ZCRMModuleRelation> relatedLists = new List<ZCRMModuleRelation>();
            JArray relatedListArray = (JArray)responseJSON.GetValue("related_lists");
            foreach(JObject relatedList in relatedListArray)
            {
                relatedLists.Add(GetZCRMModuleRelation(relatedList));
            }
            return relatedLists;
        }

    }
}
