using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Api.Handler;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Setup.Restclient;
using SampleSDK.CRM.Library.CRMException;


namespace SampleSDK.CRM.Library.Api
{
    public class APIRequest
    {
        private string url = $"{ZCRMConfigUtil.GetApiBaseURL()}/crm/{ZCRMConfigUtil.GetApiVersion()}/";
        private APIConstants.RequestMethod requestMethod;
        private Dictionary<string, string> requestHeaders = new Dictionary<string, string>();
        private Dictionary<string, string> requestParams = new Dictionary<string, string>();
        private JObject requestBody;
        private HttpWebResponse response = null;

        public Dictionary<string, string> RequestHeaders { get => requestHeaders; private set => requestHeaders = value; }
        public Dictionary<string, string> RequestParams { get => requestParams; private set => requestParams = value; }
        private JObject RequestBody { get => requestBody; set => requestBody = value; }

        //TODO: Initialize everything in the constructor -> params,headers and urlpath;


        private APIRequest(IAPIHandler handler)
        {
            url = handler.GetUrlPath().Contains("http") ? handler.GetUrlPath() : url + handler.GetUrlPath();
            requestMethod = handler.GetRequestMethod();
            RequestHeaders = handler.GetRequestHeadersAsDict();
            RequestParams = handler.GetRequestQueryParamsAsDict();
            RequestBody = handler.GetRequestBody();
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

        private string GetHeader(string name){
            return RequestHeaders[name];
        }

        public string GetRequestParam(string name)
        {
            return RequestParams[name];
        }

        public void SetQueryParams()
        {
            if(RequestParams.Count == 0) { return; }
            url += "?";
            foreach(KeyValuePair<string, string> keyValuePairs in RequestParams)
            {
                //TODO: Inspect the working of this line;
                url = url.EndsWith("?", StringComparison.InvariantCulture) ? $"{url}{keyValuePairs.Key}={keyValuePairs.Value}": $"&{url}{keyValuePairs.Key}={keyValuePairs.Value}";
            }
        }

        public void SetHeaders(ref HttpWebRequest request)
        {
            foreach(KeyValuePair<string, string> keyValuePairs in RequestHeaders)
            {
                request.Headers[keyValuePairs.Key] = keyValuePairs.Value;
            }
        }

        private void AuthenticateRequest()
        {
            //TODO<IMPORTANT>: Inspect the usage of dynamic authentication handling;
            if(ZCRMConfigUtil.HandleAuthentication)
            {
                string accessToken = ZCRMConfigUtil.GetAccessToken();
                if(accessToken == null)
                {
                    //TODO: Log the error;
                    throw new ZCRMException("AUTHENTICATOIN_FAILURE", "Access token is not set");
                }
                SetHeader("Authorization", APIConstants.authHeaderPrefix+accessToken);
                //TODO: Log the info;
            }
        }

        //TODO: Handle exceptions appropriately;
        public APIResponse GetAPIResponse()
        {
            try{
                GetResponseFromServer();
                return new APIResponse(response);
            }catch(ZCRMException)
            {
                Console.WriteLine("Excetption caught");
                throw;
            }
        }

        //TODO: Handle exceptions appropriately and Complete BulkAPIResponse class;
        public BulkAPIResponse GetBulkAPIResponse()
        {
            try{
                GetResponseFromServer();
                return new BulkAPIResponse(response);
            }catch(ZCRMException)
            {
                throw;
            }
        }


        //TODO: Handle exceptions and Inspect this block of code<IMPORTANT>;
        private void GetResponseFromServer()
        {
            try
            {
                PopulateRequestHeaders(ZCRMRestClient.StaticHeaders);
                //TODO<IMPORTANT- THREADLOCAL>:ZCRMRestClient.DYNAMIC_HEADERS-> Populate RequestHeaders if not null;
                if (ZCRMRestClient.DYNAMIC_HEADERS != null)
                {
                    PopulateRequestHeaders(ZCRMRestClient.GetDynamicHeaders());
                }
                else
                {
                    AuthenticateRequest();
                }
                SetQueryParams();
                HttpWebRequest request = GetHttpWebRequestClient();
                SetHeaders(ref request);
                request.Method = requestMethod.ToString();
                if (RequestBody != null)
                {
                    string dataString = RequestBody.ToString();
                    var data = Encoding.ASCII.GetBytes(dataString);
                    int dataLength = data.Length;
                    request.ContentType = "application/json";
                    using (var writer = request.GetRequestStream())
                    {
                        writer.Write(data, 0, dataLength);
                    }
                }
                response = (HttpWebResponse)request.GetResponse();
            }catch(Exception)
            {
                throw;
            }

        }




        private HttpWebRequest GetHttpWebRequestClient()
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
        //TODO: File upload and download methods();
    }
}
