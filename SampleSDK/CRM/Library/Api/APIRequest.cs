using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
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
        private static string boundary = "---------fileBoundary1234567890";
        private Stream fileRequestBody;

        public Dictionary<string, string> RequestHeaders { get => requestHeaders; private set => requestHeaders = value; }
        public Dictionary<string, string> RequestParams { get => requestParams; private set => requestParams = value; }
        private JObject RequestBody { get => requestBody; set => requestBody = value; }

        //TODO: Initialize everything in the constructor -> params,headers and urlpath;


        private APIRequest(IAPIHandler handler)
        {
            Console.WriteLine(url);
            Console.WriteLine(handler.GetUrlPath());
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
                if(!string.IsNullOrEmpty(keyValuePairs.Value))
                {
                    url = url.EndsWith("?", StringComparison.InvariantCulture) ? $"{url}{keyValuePairs.Key}={keyValuePairs.Value}" : $"{url}&{keyValuePairs.Key}={keyValuePairs.Value}";    
                }
            }
            url = url.EndsWith("?", StringComparison.InvariantCulture) ? url.TrimEnd('?') : url;
        }

        public void SetHeaders(ref HttpWebRequest request)
        {
            foreach(KeyValuePair<string, string> keyValuePairs in RequestHeaders)
            {
                if(!string.IsNullOrEmpty(keyValuePairs.Value))
                {
                    request.Headers[keyValuePairs.Key] = keyValuePairs.Value;   
                }
            }
            Console.WriteLine("Headers");
            Console.WriteLine(request.Headers);
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
        public BulkAPIResponse<T> GetBulkAPIResponse<T>() where T : ZCRMEntity
        {
            try{
                GetResponseFromServer();
                return new BulkAPIResponse<T>(response);
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
                Console.WriteLine("Static headers");
                PopulateRequestHeaders(ZCRMRestClient.StaticHeaders);
                //TODO<IMPORTANT- THREADLOCAL>:ZCRMRestClient.DYNAMIC_HEADERS-> Populate RequestHeaders if not null;
                if (ZCRMRestClient.DYNAMIC_HEADERS.IsValueCreated)
                {
                    Console.WriteLine("Dynamic Headers");
                    Console.WriteLine(ZCRMRestClient.DYNAMIC_HEADERS.IsValueCreated);
                    Console.WriteLine(ZCRMRestClient.DYNAMIC_HEADERS.Value);
                    PopulateRequestHeaders(ZCRMRestClient.GetDynamicHeaders());
                }
                else
                {
                    Console.WriteLine("Authenticating the request");
                    AuthenticateRequest();
                }
                SetQueryParams();
                HttpWebRequest request = GetHttpWebRequestClient();
                SetHeaders(ref request);
                request.Method = requestMethod.ToString();

                Console.WriteLine(request.Headers);
                Console.WriteLine(request.Method);
                Console.WriteLine(RequestBody.Type);

                //Post JSON part;
                if (RequestBody.Type != JTokenType.Null && RequestBody.Count > 0)
                {
                    Console.WriteLine(RequestBody.ToString());
                    ZCRMLogger.LogInfo("Populating Request Body");
                    string dataString = RequestBody.ToString();
                    var data = Encoding.ASCII.GetBytes(dataString);
                    int dataLength = data.Length;
                    request.ContentType = "application/json";
                    request.ContentLength = dataLength;
                    using (var writer = request.GetRequestStream())
                    {
                        writer.Write(data, 0, dataLength);
                    }
                }
                //File Upload Request
                //TODO: Test this part;
                else if(fileRequestBody != null && fileRequestBody.Length != 0)
                {
                    ZCRMLogger.LogInfo("Inside file upload request");
                    long fileDataLength = fileRequestBody.Length;
                    request.ContentType = $"multipart/form-data; boundary={boundary}";
                    request.ContentLength = fileDataLength;
                    request.UserAgent = "Mozilla/5.0";
                    fileRequestBody.Position = 0;
                    ZCRMLogger.LogInfo("FileRequestBody position : " +fileRequestBody.Position);
                    ZCRMLogger.LogInfo("Request Body Length : "+fileDataLength);
                    byte[] data = new byte[1024];
                    int bytesRead = 0;
                    Stream test = new MemoryStream();
                    using (var writer = request.GetRequestStream())
                    {
                        while((bytesRead = fileRequestBody.Read(data, 0, data.Length))!= 0)
                        {
                          //  test.Write(data, 0, bytesRead);
                            writer.Write(data, 0, bytesRead);
                        }
                    }
                    //test.Position = 0;
                    //ZCRMLogger.LogInfo("Stream");
                    //ZCRMLogger.LogInfo(new StreamReader(test).ReadToEnd());
                }
                response = (HttpWebResponse)request.GetResponse();
                ZCRMLogger.LogInfo("Received the response");
            }catch(Exception e)
            {
                Console.WriteLine("Exception in GetResponseFromServer");
                Console.WriteLine(e);
                throw new ZCRMException(e.ToString());
            }

        }




        private HttpWebRequest GetHttpWebRequestClient()
        {
            Console.WriteLine(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            /*if(ZCRMConfigUtil.ConfigProperties.ContainsKey("timeout")){
                int? timeoutPeriod = Convert.ToInt32(ZCRMConfigUtil.GetConfigValue("timeout"));
                if (timeoutPeriod != null)
                {
                    request.Timeout = (int)timeoutPeriod;
                }

            }
            string userAgent = ZCRMConfigUtil.GetConfigValue("userAgent");
            if(userAgent != null)
            {
                request.UserAgent = userAgent;
            }*/
            Console.WriteLine($"HttpWebRequestClient created with url {url}\n Timeout{request.Timeout}");
            return request;
        }

        private void PopulateRequestHeaders(Dictionary<string, string> dict)
        {
            Console.WriteLine("Inside Populate Request Headers");
            foreach(KeyValuePair<string, string> keyValuePair in dict)
            {
                Console.WriteLine(keyValuePair.Key);
                RequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        //TODO: AuthenticateRequest method <IMPORTANT- REFLECTION CONCEPT>

        //TODO: GetResponseFromServer Method ----Performs almost all the tasks and process the requests and populates the response;
        //NOTE: GetHTTPClient Method sets timeout and user-agent<Similar to getZohoConnector()>;
        //TODO: File upload and download methods();


        public APIResponse UploadFile(string filePath)
        {
            fileRequestBody = GetFileRequestBodyStream(filePath);
            GetResponseFromServer();
            return new APIResponse(response);

        }

        public FileAPIResponse DownloadFile()
        {
            try{
                GetResponseFromServer();
                return new FileAPIResponse(response);
            }catch(Exception e)
            {
                ZCRMLogger.LogError(e.ToString());
                throw new ZCRMException(e.ToString());
            }
        }


        private Stream GetFileRequestBodyStream(string filePath)
        {
            Stream fileDataStream = new MemoryStream();

            FileInfo fileInfo = new FileInfo(filePath);

            //File Content-Disposition header;
            string fileHeader = string.Format($"\r\n--{boundary}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{fileInfo.Name}\"\r\nContent-Type: {fileInfo.Extension}\r\n\r\n");
            byte[] fileHeaderBytes = Encoding.ASCII.GetBytes(fileHeader);
            fileDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);


            //File content 
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            using (FileStream fileStream = fileInfo.OpenRead())
            {
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fileDataStream.Write(buffer, 0, bytesRead);
                }
            }

            //Footer
            byte[] fileFooterBytes = Encoding.ASCII.GetBytes("--" + boundary + "--");
            fileDataStream.Write(fileFooterBytes, 0, fileFooterBytes.Length);
            fileDataStream.Position = 0;
            ZCRMLogger.LogInfo(new StreamReader(fileDataStream).ReadToEnd());
            ZCRMLogger.LogInfo("Line position : " + fileDataStream.Position);
            fileDataStream.Position = 0;
            return fileDataStream;
        }
    }
}
