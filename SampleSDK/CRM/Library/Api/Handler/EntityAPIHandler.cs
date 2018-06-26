using System;
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
            requestMethod = APIConstants.RequestMethod.GET;
            urlPath = $"{record.ModuleAPIName}/{record.EntityId}";

            return null;
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


        public EntityAPIHandler()
        {
        }
    }
}
