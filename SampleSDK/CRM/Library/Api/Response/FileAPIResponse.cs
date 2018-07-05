using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.CRMException;

namespace SampleSDK.CRM.Library.Api.Response
{
    public class FileAPIResponse : APIResponse
    {
        
        public FileAPIResponse(HttpWebResponse response) : base(response) 
        {
            
        }

        public string GetFileName()
        {
            ZCRMLogger.LogInfo("Content-Type = "+Response.Headers["Content-Type"]);
            string fileMetaData = Response.Headers["Content-Disposition"];
            string fileName = fileMetaData.Split(new string[] { "=" }, StringSplitOptions.None)[1];
            if(fileName.Contains("''"))
            {
                fileName = fileName.Split(new string[] {"''"}, StringSplitOptions.None)[1];

            }
            return fileName;
        }



        //TODO: Override the method;
        protected override void SetResponseJSON()
        {
            if(HttpStatusCode == APIConstants.ResponseCode.OK || HttpStatusCode == APIConstants.ResponseCode.NO_CONTENT)
            {
                ResponseJSON = new JObject();
                if(HttpStatusCode == APIConstants.ResponseCode.OK)
                {
                    Status = "success";
                }
                else{
                    string responseString = new StreamReader(Response.GetResponseStream()).ReadToEnd();
                    ResponseJSON = JObject.Parse(responseString);
                }
            }
        }

        public Stream GetFileAsStream()
        {
            return Response.GetResponseStream();
        }


    }
}
