using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Common;

namespace SampleSDK.CRM.Library.Api.Handler
{
    public class CommonAPIHandler : IAPIHandler
    {
        protected APIConstants.RequestMethod requestMethod;
        protected string urlPath;
        protected JObject requestHeaders = new JObject();
        protected JObject requestQueryParams = new JObject();
        protected JObject requestBody = new JObject();


        public CommonAPIHandler() { }

        public JObject GetRequestBody() { return requestBody; }

        public JObject GetRequestHeaders() { return requestHeaders; }

        public APIConstants.RequestMethod GetRequestMethod() { return requestMethod; }

        public JObject GetRequestQueryParams() { return requestQueryParams; }

        public string GetUrlPath() { return urlPath; }

        public Dictionary<string, string> GetRequestHeadersAsDict() { return CommonUtil.ConvertJObjectToDict(requestHeaders); }

        public Dictionary<string, string> GetRequestQueryParamsAsDict() { return CommonUtil.ConvertJObjectToDict(requestQueryParams); }


    }
}
