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
    public class MetaDataAPIHandler : CommonAPIHandler, IAPIHandler
    {

        private MetaDataAPIHandler() { }

        public static MetaDataAPIHandler GetInstance()
        {
            return new MetaDataAPIHandler();
        }


        //TODO: Handle Exception;
        public APIResponse GetModule(string apiName)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/modules/" + apiName;

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray modulesArray = (JArray)response.ResponseJSON.GetValue("modules");
                JObject moduleDetails = (JObject)modulesArray[0];
                response.Data = GetZCRMModule(moduleDetails);

                return response;
            }catch(Exception e)
            {
                //TODO: Log the info and handle exceptions;
                Console.WriteLine("Exception caught in GetModule");
                Console.WriteLine(e);
                throw new ZCRMException(e.ToString());
            }
        }


        //TODO: Inspect the field modified_by in the response;
        private ZCRMEntity GetZCRMModule(JObject moduleDetails)
        {
            ZCRMModule module = ZCRMModule.GetInstance(Convert.ToString(moduleDetails.GetValue("api_name")));
            module.Id = Convert.ToInt64(moduleDetails.GetValue("id"));
            module.SystemName = Convert.ToString(moduleDetails.GetValue("module_name"));
            module.SingularLabel = Convert.ToString(moduleDetails.GetValue("singular_label"));
            module.PluralLabel = Convert.ToString(moduleDetails.GetValue("plural_label"));
            module.Creatable = Convert.ToBoolean(moduleDetails.GetValue("creatable"));
            module.Viewable = Convert.ToBoolean(moduleDetails.GetValue("viewable"));
            module.Editable = Convert.ToBoolean(moduleDetails.GetValue("editable"));
            module.Convertible = Convert.ToBoolean(moduleDetails.GetValue("convertible"));
            module.Deletable = Convert.ToBoolean(moduleDetails.GetValue("deletable"));
            module.CustomModule = Convert.ToBoolean(moduleDetails.GetValue("generated_type").ToString().Equals("custom"));

            JArray accessibleProfilesArray = (JArray)moduleDetails.GetValue("profiles");
            foreach(JObject accessibleProfiles in accessibleProfilesArray)
            {
                ZCRMProfile profile = ZCRMProfile.GetInstance(Convert.ToInt64(accessibleProfiles.GetValue("id")), Convert.ToString(accessibleProfiles.GetValue("name")));
                module.AddAccessibleProfiles(profile);
            }
            if(moduleDetails.GetValue("modified_by") != null)
            {
                JObject modifiedByObject = (JObject)moduleDetails.GetValue("modified_by");
                ZCRMUser modifiedUser = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
                module.ModifiedBy = modifiedUser;
                module.ModifiedTime = Convert.ToString(moduleDetails.GetValue("modified_time"));
            }
            if(moduleDetails.ContainsKey("related_lists"))
            {
                List<ZCRMModuleRelation> relatedLists = new List<ZCRMModuleRelation>();
                JArray relatedListsArray = (JArray)moduleDetails.GetValue("related_lists");
                foreach(JObject relatedListDetails in relatedListsArray)
                {
                    ZCRMModuleRelation relatedList = ZCRMModuleRelation.GetInstance(module.ApiName, Convert.ToString(relatedListDetails.GetValue("api_name)")));
                    SetRelatedListProperties(relatedList, relatedListDetails);
                    relatedLists.Add(relatedList);
                }
                module.RelatedLists = relatedLists;
            }
            if(moduleDetails.ContainsKey("business_card_fields"))
            {
                List<string> bcFields = new List<string>();
                JArray bcFieldsArray = (JArray)moduleDetails.GetValue("business_card_fields");
                foreach(JObject bcField in bcFieldsArray)
                {
                    bcFields.Add(bcField.ToString());
                }
                module.BussinessCardFields = bcFields;
            }
            if(moduleDetails.ContainsKey("layouts"))
            {
                //TODO: Complete the below line after completing ModuleAPIHandler;
               // module.Layouts = ModuleAPIHandler.GetInstance(module).GetAllLayouts();
            }
            return module;
        }

        private void SetRelatedListProperties(ZCRMModuleRelation relatedList, JObject relatedListDetails)
        {
            relatedList.Id = Convert.ToInt64(relatedListDetails.GetValue("id"));
            relatedList.Visible = Convert.ToBoolean(relatedListDetails.GetValue("visible"));
            relatedList.Label = Convert.ToString(relatedListDetails.GetValue("display_label"));
        }
    }
}
