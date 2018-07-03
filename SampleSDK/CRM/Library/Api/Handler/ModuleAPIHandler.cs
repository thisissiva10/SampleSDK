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
                JArray layoutsArray = (JArray)responseJSON["layouts"];
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
                urlPath = "settings/layouts";
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
                urlPath = "settings/fields";
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
                urlPath = "settings/fields";
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
                JArray layoutsArray = (JArray)responseJSON["custom_views"];
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
            ZCRMLayout layout = ZCRMLayout.GetInstance(Convert.ToInt64(layoutDetails["id"]));
            layout.Name = Convert.ToString(layoutDetails["name"]);
            layout.Visible = Convert.ToBoolean(layoutDetails["visible"]);
            layout.Status = Convert.ToInt32(layoutDetails["status"]);
            if(layoutDetails["created_by"].HasValues)
            {
                JObject createdByObject = (JObject)layoutDetails["created_by"];
                ZCRMUser createdUser = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject["id"]), Convert.ToString(createdByObject["name"]));
                layout.CreatedBy = createdUser;
                layout.CreatedTime = (string)layoutDetails["created_time"];
            }
            if (layoutDetails["modified_by"].HasValues)
            {
                JObject modifiedByObject = (JObject)layoutDetails["modified_by"];
                ZCRMUser modifiedUser = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject["id"]), Convert.ToString(modifiedByObject["name"]));
                layout.ModifiedBy = modifiedUser;
                layout.ModifiedTime = (string)layoutDetails["modified_time"];
            }
            JArray accessibleProfilesArray = (JArray)layoutDetails["profiles"];
            foreach(JObject profileObject in accessibleProfilesArray)
            {
                ZCRMProfile profile = ZCRMProfile.GetInstance(Convert.ToInt64(profileObject["id"]), Convert.ToString(profileObject["name"]));
                layout.AddAccessibleProfiles(profile);
            }
            layout.Sections = GetAllSectionsofLayout(layoutDetails);
            return layout;
            
        }

        //TODO: Check this method and the token object and take a look into the precision field;
        private ZCRMField GetZCRMField(JObject fieldJSON)
        {
                                        ZCRMField field = ZCRMField.GetInstance(Convert.ToString(fieldJSON["api_name"]));
            field.DefaultValue = (object)fieldJSON["default_value"];
            field.Mandatory = Convert.ToBoolean(fieldJSON["required"]);
            field.Id = Convert.ToInt64(fieldJSON["id"]);
            field.CustomField = Convert.ToBoolean(fieldJSON["custom_field"]);
            field.DataType = Convert.ToString(fieldJSON["data_type"]);
            field.DisplayName = Convert.ToString(fieldJSON["field_label"]);
            field.MaxLength = (int?)fieldJSON["length"];
            field.Precision = (int?)fieldJSON["decimal_place"];
            field.ReadOnly = Convert.ToBoolean(fieldJSON["read_only"]);
            field.Visible = Convert.ToBoolean(fieldJSON["visible"]);
            field.SequenceNo = (int?)fieldJSON["sequence_number"];
            field.ToolTip = Convert.ToString(fieldJSON["tooltip"]);
            field.Webhook = Convert.ToBoolean(fieldJSON["webhook"]);
            field.CreatedSource = Convert.ToString(fieldJSON["created_source"]);
            JToken tempJSONObect = fieldJSON["formula"];
            if (tempJSONObect.HasValues)
            {
                field.FormulaReturnType = Convert.ToString(fieldJSON["return_type"]);
            }
            tempJSONObect = fieldJSON["currency"];
            if (tempJSONObect.HasValues)
            {
                field.Precision = (int?)fieldJSON["precision"];
            }
            JObject subLayouts = (JObject)fieldJSON["view_type"];
            if (subLayouts != null)
            {
                List<string> layoutsPresent = new List<string>();
                if (Convert.ToBoolean(subLayouts["create"]))
                {
                    layoutsPresent.Add("CREATE");
                }
                if (Convert.ToBoolean(subLayouts["view"]))
                {
                    layoutsPresent.Add("VIEW");
                }
                if (Convert.ToBoolean(subLayouts["quick_create"]))
                {
                    layoutsPresent.Add("QUICK_CREATE");
                }
                field.SubLayoutsPresent = layoutsPresent;
            }

            JArray pickList = (JArray)fieldJSON["pick_list_values"];
            foreach (JObject pickListObject in pickList)
            {
                ZCRMPickListValue pickListValue = ZCRMPickListValue.GetInstance();
                pickListValue.DisplayName = Convert.ToString(fieldJSON["display_value"]);
                pickListValue.ActualName = Convert.ToString(fieldJSON["actual_value"]);
                pickListValue.SequenceNumber = Convert.ToInt32(fieldJSON["sequence_number"]);
                pickListValue.Maps = (JArray)fieldJSON["maps"];
                field.AddPickListValue(pickListValue);
            }
            JObject lookup = (JObject)fieldJSON["lookup"];
            foreach (KeyValuePair<string, JToken> lookupObject in lookup)
            {
                field.SetLookupDetails(lookupObject.Key, (object)lookupObject.Value);
            }
            JObject multilookup = (JObject)fieldJSON["multiselectlookup"];
            foreach (KeyValuePair<string, JToken> multiLookupObject in multilookup)
            {
                field.SetMultiselectLookup(multiLookupObject.Key, (object)multiLookupObject.Value);
            }
            if (fieldJSON.ContainsKey("subformtabId"))
            {
                field.SubFormTabId = Convert.ToInt64(fieldJSON["subformtabid"]);
            }
            if (fieldJSON.ContainsKey("subform"))
            {
                JObject subformDetails = (JObject)fieldJSON["subform"];
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
            ZCRMSection section = ZCRMSection.GetInstance(Convert.ToString(sectionJSON["name"]));
            section.ColumnCount = Convert.ToInt32(sectionJSON["column_count"]);
            section.DisplayName = Convert.ToString(sectionJSON["display_label"]);
            section.Sequence = Convert.ToInt32(sectionJSON["sequence_number"]);
            section.Fields = GetAllFields(sectionJSON);
            return section;
        }

        //TODO: Handle exceptions appropriately;
        private ZCRMCustomView GetZCRMCustomView(JObject customViewObject)
        {
            ZCRMCustomView customView = ZCRMCustomView.GetInstance(module.ApiName, Convert.ToInt64(customViewObject["id"]));
            customView.DisplayName = (string)customViewObject["display_value"];
            customView.Isdefault = Convert.ToBoolean(customViewObject["default"]);
            customView.SystemName = Convert.ToString(customViewObject["system_name"]);
            customView.Category = Convert.ToString(customViewObject["category"]);
            if (customViewObject["favourite"] != null)
            {
                customView.Favourite = Convert.ToInt32(customViewObject["favourite"]);
            }
            customView.Name = Convert.ToString(customViewObject["name"]);
            customView.SortBy = Convert.ToString(customViewObject["sort_by"]);
            if (customViewObject["sort_order"] != null)
            {
                customView.SortOrder = (CommonUtil.SortOrder)Enum.Parse(typeof(CommonUtil.SortOrder), Convert.ToString(customViewObject["sort_order"]));
            }
            List<string> fields = new List<string>();
            if (customViewObject.ContainsKey("fields"))
            {
                JArray fieldsArray = (JArray)customViewObject["fields"];
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
            ZCRMModuleRelation moduleRelation = ZCRMModuleRelation.GetInstance(module.ApiName, Convert.ToInt64(relatedList["id"]));
            moduleRelation.ApiName = Convert.ToString(relatedList["api_name"]);
            moduleRelation.Label = Convert.ToString(relatedList["display_label"]);
            moduleRelation.Module = Convert.ToString(relatedList["module"]);
            moduleRelation.Type = Convert.ToString(relatedList["type"]);
            return moduleRelation;
        }

        //TODO: Throws Exception;
        private List<ZCRMSection> GetAllSectionsofLayout(JObject layoutDetails)
        {
            List<ZCRMSection> sections = new List<ZCRMSection>();
            JArray sectionsArray = (JArray)layoutDetails["sections"];
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
            JArray layoutsArray = (JArray)layoutsJSONObject["layouts"];
            foreach(JObject layoutObject in layoutsArray)
            {
                allLayouts.Add(GetZCRMLayout(layoutObject));
            }
            return allLayouts;
        }

        private List<ZCRMField> GetAllFields(JObject sectionJSON)
        {
            List<ZCRMField> fields = new List<ZCRMField>();
            JArray fieldsArray = (JArray)sectionJSON["fields"];
            foreach(JObject fieldObject in fieldsArray)
            {
                fields.Add(GetZCRMField(fieldObject));
            }
            return fields;
        }

        private List<ZCRMCustomView> GetAllCustomViews(JObject customviewJSON)
        {
            List<ZCRMCustomView> allCustomViews = new List<ZCRMCustomView>();
            JArray customViewsArray = (JArray)customviewJSON["custom_views"];
            foreach (JObject customViewObject in customViewsArray)
            {
                allCustomViews.Add(GetZCRMCustomView(customViewObject));
            }
            return allCustomViews;
        }

        private List<ZCRMModuleRelation> GetAllRelatedLists(JObject responseJSON)
        {
            List<ZCRMModuleRelation> relatedLists = new List<ZCRMModuleRelation>();
            JArray relatedListArray = (JArray)responseJSON["related_lists"];
            foreach(JObject relatedList in relatedListArray)
            {
                relatedLists.Add(GetZCRMModuleRelation(relatedList));
            }
            return relatedLists;
        }

    }
}
