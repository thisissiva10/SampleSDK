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
                urlPath = "settings/layouts/" + module.ApiName;
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


        //TODO: Handle Exceptions;
        private ZCRMEntity GetZCRMLayout(JObject layoutDetails)
        {
            ZCRMLayout layout = ZCRMLayout.GetInstance(Convert.ToInt64(layoutDetails.GetValue("id")));
            layout.Name = Convert.ToString(layoutDetails.GetValue("name"));
            layout.Visible = Convert.ToBoolean(layoutDetails.GetValue("visible"));
            layout.Status = Convert.ToInt32(layoutDetails.GetValue("status"));
            if(layoutDetails.GetValue("created_by") != null)
            {
                JObject createdByObject = (JObject)layoutDetails.GetValue("created_by");
                ZCRMUser createdUser = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject.GetValue("id")), Convert.ToString(createdByObject.GetValue("name")));
                layout.CreatedBy = createdUser;
                layout.CreatedTime = Convert.ToString(layoutDetails.GetValue("created_time"));
            }
            if (layoutDetails.GetValue("modified_by") != null)
            {
                JObject modifiedByObject = (JObject)layoutDetails.GetValue("modified_by");
                ZCRMUser modifiedUser = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
                layout.ModifiedBy = modifiedUser;
                layout.ModifiedTime = Convert.ToString(layoutDetails.GetValue("modified_time"));
            }
            JArray accessibleProfilesArray = (JArray)layoutDetails.GetValue("profiles");
            foreach (JObject accessibleProfiles in accessibleProfilesArray)
            {
                ZCRMProfile profile = ZCRMProfile.GetInstance(Convert.ToInt64(accessibleProfiles.GetValue("id")), Convert.ToString(accessibleProfiles.GetValue("name")));
                layout.AddAccessibleProfiles(profile);
            }
            layout.Sections = GetAllSectionsofLayout(layoutDetails);
            return layout;
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


        //TODO: Check this method and the token object;
        private ZCRMField GetZCRMField(JObject fieldJSON)
        {
            ZCRMField field= ZCRMField.GetInstance(Convert.ToString(fieldJSON.GetValue("api_name")));
            field.DefaultValue = (object)fieldJSON.GetValue("default_value");
            field.Mandatory = Convert.ToBoolean(fieldJSON.GetValue("required"));
            field.Id = Convert.ToInt64(fieldJSON.GetValue("id"));
            field.CustomField = Convert.ToBoolean(fieldJSON.GetValue("custom_field"));
            field.DataType = Convert.ToString(fieldJSON.GetValue("data_type"));
            field.DisplayName = Convert.ToString(fieldJSON.GetValue("field_label"));
            field.MaxLength = Convert.ToInt32(fieldJSON.GetValue("length"));
            field.Precesion = Convert.ToInt32(fieldJSON.GetValue("decimal_place"));
            field.ReadOnly = Convert.ToBoolean(fieldJSON.GetValue("read_only"));
            field.Visible = Convert.ToBoolean(fieldJSON.GetValue("visible"));
            field.SequenceNo = Convert.ToInt32(fieldJSON.GetValue("sequence_number"));
            field.ToolTip = Convert.ToString(fieldJSON.GetValue("tooltip"));
            field.Webhook = Convert.ToBoolean(fieldJSON.GetValue("webhook"));
            field.CreatedSource = Convert.ToString(fieldJSON.GetValue("created_source"));
            JToken token = fieldJSON.GetValue("formula");
            if(token.HasValues)
            {
                
            }




        }
    }
}
