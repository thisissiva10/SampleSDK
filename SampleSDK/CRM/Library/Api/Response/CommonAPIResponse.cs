using System;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace SampleSDK.CRM.Library.Api.Response
{
    public class CommonAPIResponse
    {

        private HttpWebResponse response;
        private JObject responseJSON;
        //TODO: Define enum responsecodes and work out the functions in that class;
        private APIConstants.ResponseCode? httpStatusCode;
        private ResponseHeaders responseHeaders;

        private HttpWebResponse Response { get => response; set => response = value; }

        //NOTE: Because of naming collision, the properties have been changed to properties;
        protected ResponseHeaders GetResponseHeaders()
        {
            return responseHeaders;
        }
        protected void SetResponseHeaders(ResponseHeaders value)
        {
            responseHeaders = value;
        }

        protected APIConstants.ResponseCode? HttpStatusCode { get => httpStatusCode; private set => httpStatusCode = value; }

        internal JObject ResponseJSON { get => responseJSON; set => responseJSON = value; }




        public CommonAPIResponse() { }

        public CommonAPIResponse(HttpWebResponse response)
        {
            Response = response;
            Init();
            ProcessResponse();
            //TODO:Log info;

        }

        //TODO: Handle exceptions;
        protected void Init()
        {
            HttpStatusCode = APIConstants.GetEnum((int)response.StatusCode);
            SetResponseJSON();
        }


        //TODO: Handle exceptions
        protected void ProcessResponse()
        {
            if(APIConstants.FaultyResponseCodes.Contains(httpStatusCode))
            {
                HandleFaultyResponse();
            }
            else if((HttpStatusCode == APIConstants.ResponseCode.ACCEPTED) || (HttpStatusCode == APIConstants.ResponseCode.OK) ||
                    (HttpStatusCode == APIConstants.ResponseCode.CREATED))
            {
                ProcessDataResponse();
            }
        }

        //TODO: Handle exceptions
        protected void SetResponseJSON()
        {
            if((APIConstants.ResponseCode.NO_CONTENT == HttpStatusCode) || (APIConstants.ResponseCode.NOT_MODIFIED == HttpStatusCode))
            {
                ResponseJSON = new JObject();
            }
            else
            {
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ResponseJSON = JObject.Parse(responseString);
            }
        }

        //TODO: Handle exceptions;
        protected virtual void HandleFaultyResponse() { }

        //TODO: HAndle Exceptions;
        protected virtual void ProcessDataResponse() { }


        public override string ToString()
        {
            return $"STATUS_CODE = {HttpStatusCode}, RESPONSE_JSON = {ResponseJSON}, RESPONSE_HEADERS = {GetResponseHeaders().ToString()}";
        }




        //TODO: Inspect the usage of static modifier in the below class;
        public class ResponseHeaders
        {
            private int remainingCountForThisDay;
            private int remainingCountForThisWindow;
            private long remainingTimeForThisWindowReset;

            public ResponseHeaders(HttpWebResponse response)
            {
                string header = response.GetResponseHeader("X-RATELIMIT-LIMIT");
                if(header != null)
                {
                    RemainingAPICountForThisDay = Convert.ToInt32(header);
                    RemainingAPICountForThisWindow = Convert.ToInt32(response.GetResponseHeader("X-RATELIMIT-REMAINING"));
                    RemainingTimeForThisWindowReset = Convert.ToInt64(response.GetResponseHeader("X-RATELIMIT-RESET"));
                }
            }

            public int RemainingAPICountForThisDay { get => remainingCountForThisDay; private set => remainingCountForThisDay = value; }
            public int RemainingAPICountForThisWindow { get => remainingCountForThisWindow; private set => remainingCountForThisWindow = value; }
            public long RemainingTimeForThisWindowReset { get => remainingTimeForThisWindowReset; private set => remainingTimeForThisWindowReset = value; }

            public override string ToString()
            {
                return $"X-RATELIMIT-LIMIT={RemainingAPICountForThisDay}; X-RATELIMIT-REMIANING={RemainingAPICountForThisWindow}; X-RATELIMIT-RESET={RemainingTimeForThisWindowReset};";
            }
        }
    }
}
