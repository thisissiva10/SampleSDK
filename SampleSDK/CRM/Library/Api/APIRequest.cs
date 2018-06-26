using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Api.Handler;
using SampleSDK.CRM.Library.Setup.Restclient;


namespace SampleSDK.CRM.Library.Api
{
    public class APIRequest
    {
        private string url = $"{ZCRMConfigUtil.GetApiBaseURL()}/crm/{ZCRMConfigUtil.GetApiVersion()}/";
        private Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
        private Dictionary<string, string> requestParams = new Dictionary<string, string>();
        private JObject requestBody = new JObject();
        private HttpWebResponse response = null;

        public Dictionary<string, string> RequestHeaders { get => requestHeaders; private set => requestHeaders = value; }
        public Dictionary<string, string> RequestParams { get => requestParams; private set => requestParams = value; }

        //TODO: Initialize everything in the constructor -> params,headers and urlpath;


        private APIRequest(IAPIHandler handler)
        {
        }

        public static APIRequest GetInstance(IAPIHandler handler){
            return new APIRequest(handler);
        }






        public void SetHeader(string name, string value)
        {
            RequestHeaders.Add(name, value);    
        }

        public void SetRequestParam(string name, string value)
        {
            RequestParams.Add(name, value);
        }

        public string GetHeader(string name){
            return RequestHeaders[name];
        }

        public string GetRequestParam(string name)
        {
            return RequestParams[name];
        }


        //TODO: Handle exceptions;
        private void GetResponseFromServer()
        {
            HttpWebRequest request = GetHttpWebRequest();
            PopulateRequestHeaders(ZCRMRestClient.StaticHeaders);
            //TODO<IMPORTANT- THREADLOCAL>:ZCRMRestClient.DYNAMIC_HEADERS-> Populate RequestHeaders if not null;


        }




        private HttpWebRequest GetHttpWebRequest()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            int? timeoutPeriod = Convert.ToInt32(ZCRMConfigUtil.GetConfigValue("timeout"));
            string userAgent = ZCRMConfigUtil.GetConfigValue("userAgent");
            if(timeoutPeriod != null)
            {
                request.Timeout = (int)timeoutPeriod;
            }

            if(userAgent != null)
            {
                request.UserAgent = userAgent;
            }
            return request;
        }



        private void PopulateRequestHeaders(Dictionary<string, string> dict)
        {
            foreach(KeyValuePair<string, string> keyValuePair in dict)
            {
                RequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        //TODO: AuthenticateRequest method <IMPORTANT- REFLECTION CONCEPT>

        //TODO: GetResponseFromServer Method ----Performs almost all the tasks and process the requests and populates the response;
        //NOTE: GetHTTPClient Method sets timeout and user-agent<Similar to getZohoConnector()>;
    }
}
