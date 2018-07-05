using System;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using SampleSDK.CRM.Library.CRMException;
using System.Collections.Generic;

namespace SampleSDK.CRM.Library.Api.Response
{
    public class CommonAPIResponse
    {

        private HttpWebResponse response;
        private JObject responseJSON;
        //TODO: Define enum responsecodes and work out the functions in that class;
        private APIConstants.ResponseCode? httpStatusCode;
        private ResponseHeaders responseHeaders;
      //  private MemoryStream responseStream;

        internal HttpWebResponse Response { get => response; set => response = value; }

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

       // public MemoryStream ResponseStream { get => responseStream; private set => responseStream = value; }

        public CommonAPIResponse() { }

        public CommonAPIResponse(HttpWebResponse response)
        {
            Console.WriteLine("Inside CommonAPIResponse");
            Response = response;
            for (int i = 0; i < response.Headers.Count; i++)
            {
                ZCRMLogger.LogInfo(response.Headers.Keys[i] + " - " + response.Headers[i]);
            }
            Init();
            ProcessResponse();
            //TODO:Log info;

        }

        //TODO: Handle exceptions;
        protected void Init()
        {
            Console.WriteLine("Init()");
            HttpStatusCode = APIConstants.GetEnum((int)response.StatusCode);
            Console.WriteLine(HttpStatusCode);
          //  ResponseStream = CopyStream(response.GetResponseStream());
            SetResponseJSON();
        }


        //TODO: Handle exceptions
        protected void ProcessResponse()
        {
            if(APIConstants.FaultyResponseCodes.Contains(HttpStatusCode))
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
        protected virtual void SetResponseJSON()
        {
            if((APIConstants.ResponseCode.NO_CONTENT == HttpStatusCode) || (APIConstants.ResponseCode.NOT_MODIFIED == HttpStatusCode))
            {
                ResponseJSON = new JObject();
            }
            else
            {
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //ResponseStream.Position = 0;

                Console.Write("Reponse string : ");
                Console.Write(responseString);
                Console.WriteLine();
                Console.WriteLine("Before JObject conversion");
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



        /*
        private MemoryStream CopyStream(Stream inputStream)
        {
            ResponseStream = new MemoryStream();
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                ResponseStream.Write(buffer, 0, bytesRead);
            }
            ResponseStream.Position = 0;
            inputStream.Close();
            inputStream.Dispose();
            return ResponseStream;
        }

*/


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
