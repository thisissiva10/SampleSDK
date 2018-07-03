using System;
using System.Threading;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Api.Handler;
using SampleSDK.CRM.Library.CRUD;
using SampleSDK.CRM.Library.Setup.MetaData;

namespace SampleSDK.CRM.Library.Setup.Restclient
{
    public class ZCRMRestClient
    {

        private static readonly ThreadLocal<string> CURRENT_USER_EMAIL = new ThreadLocal<string>();
        //TODO: Inspect the usage of populating DYNAMIC_HEADERS;
        public static readonly ThreadLocal<Dictionary<string, string>> DYNAMIC_HEADERS = new ThreadLocal<Dictionary<string, string>>();
        private static Dictionary<string, string> staticHeaders = new Dictionary<string, string>();



        public static Dictionary<string, string> StaticHeaders { get => staticHeaders; set => staticHeaders = value; }


        public static ZCRMRestClient GetInstance()
        {
            return new ZCRMRestClient();
        }

        //TODO: Throw exception and pass proper argument
        public static void Initialize()
        {
            Initialize(true);
        }

        public static void Initialize(Boolean handleAuthentication)
        {
            ZCRMConfigUtil.Initialize(handleAuthentication);
        }




        public static void SetCurrentUser(string userEmail)
        {
            CURRENT_USER_EMAIL.Value = userEmail;
        }

        public static string GetCurrentUserEmail()
        {
            return CURRENT_USER_EMAIL.Value;
        }

        public static void setDynamicHeaders(Dictionary<string, string> headers)
        {
            DYNAMIC_HEADERS.Value = headers;
        }

        public static Dictionary<string, string> GetDynamicHeaders()
        {
            return DYNAMIC_HEADERS.Value;
        }






        public ZCRMOrganization GetOrganizationInstance()
        {
            return ZCRMOrganization.GetInstance();
        }


        public APIResponse GetOrganizationDetails()
        {
            return OrganizationAPIHandler.GetInstance().GetOrganizationDetails();
        }


        public ZCRMModule GetModuleInstance(string moduleAPIName)
        {
            return ZCRMModule.GetInstance(moduleAPIName);
        }


        public ZCRMRecord GetRecordInstance(string moduleAPIName, long entityId)
        {
            return ZCRMRecord.GetInstance(moduleAPIName, entityId);
        }

        public BulkAPIResponse<ZCRMModule> GetAllModules()
        {
            return GetAllModules(null);
        }

        public BulkAPIResponse<ZCRMModule> GetAllModules(string modifiedSince)
        {
            return MetaDataAPIHandler.GetInstance().GetAllModules(modifiedSince);
        }


        public APIResponse GetModule(string moduleName)
        {
            return MetaDataAPIHandler.GetInstance().GetModule(moduleName); 
        }
    }
}
