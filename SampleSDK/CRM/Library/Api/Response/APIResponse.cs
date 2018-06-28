using System;
using System.Net;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.CRMException;


namespace SampleSDK.CRM.Library.Api.Response
{
    public class APIResponse : CommonAPIResponse
    {

        private ZCRMEntity data;
        private string status;
        private string message;


        public APIResponse() { }

        //TODO: Handle exceptions;
        public APIResponse(HttpWebResponse response) : base(response) { }

        public ZCRMEntity Data { get => data; set => data = value; }
        public string Message { get => message; private set => message = value; }
        public string Status { get => status; private set => status = value; }


        //TODO: Handle exceptions;
        protected override void ProcessDataResponse()
        {
            JObject msgJSON = ResponseJSON;
            if(msgJSON.ContainsKey("data"))
            {
                JArray recordsArray = (JArray)ResponseJSON.GetValue("data");
                msgJSON = (JObject)recordsArray[0];
            }

            if(msgJSON.ContainsKey("message"))
            {
                Message = msgJSON.GetValue("message").ToString();
            }

            if(msgJSON.ContainsKey("status"))
            {
                Status = msgJSON.GetValue("status").ToString();
                if(Status.Equals("error"))
                {
                    if(msgJSON.ContainsKey("details"))
                    {
                        //TODO: Inspect the working of this part;
                        throw new ZCRMException(msgJSON.GetValue("code").ToString(), Message, msgJSON.GetValue("details") as JObject);
                    }
                    throw new ZCRMException(msgJSON.GetValue("code").ToString(), Message);
                }
            }
        }

        //TODO: Handle Exceptions
        protected override void HandleFaultyResponse()
        {
            if(HttpStatusCode == APIConstants.ResponseCode.NO_CONTENT)
            {
                throw new ZCRMException("INVALID_DATA", "The given id seems to be invalid");
            }

            throw new ZCRMException(ResponseJSON.GetValue("code").ToString(), ResponseJSON.GetValue("message").ToString());
        }


    }
}
