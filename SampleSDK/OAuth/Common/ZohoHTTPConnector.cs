using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SampleSDK
{
    //TODO: Add Logger to the File;
    class ZohoHTTPConnector
    {
        private string url;
        private Dictionary<string, string> requestParams = new Dictionary<string, string>();
        private Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

        public string Url { get => url; set => url = value; }
        public Dictionary<string, string> RequestHeaders { get => requestHeaders; set => requestHeaders = value; }

        public void AddParam(string key, string value)
        {
            requestParams.Add(key, value);
        }

        public void AddHeader(string key, string value)
        {
            RequestHeaders.Add(key, value);
        }

        //TODO: Change the access-modifier to internal;
        public string Post()
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            string postData = null;
            if (requestParams.Count != 0)
            {
                foreach (KeyValuePair<string, string> param in requestParams)
                {
                    if (postData == null)
                    {
                        postData = $"{param.Key}={param.Value}";
                    }
                    else
                    {
                        postData += $"&{param.Key}={param.Value}";
                    }
                }
            }
            var data = Encoding.ASCII.GetBytes(postData);

            if (RequestHeaders.Count != 0)
            {
                foreach (KeyValuePair<string, string> header in RequestHeaders)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Method = "POST";


            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        //TODO: Throw Exceptions
        public string Get()
        {
            //TODO: Inspect the usage of ssl context;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            //TODO: Add user-agent header;

            if (RequestHeaders != null && RequestHeaders.Count != 0){
                foreach(KeyValuePair<string, string> header in RequestHeaders){
                    request.Headers[header.Key] = header.Value;
                }
            }

            //TODO: Log the info;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = "";
            using(StreamReader reader = new StreamReader(response.GetResponseStream())){
                responseString = reader.ReadToEnd();
            }
            return responseString;
        }

    }
}
