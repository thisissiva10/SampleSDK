using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace SampleSDK.CRM.Library.Api.Handler
{
    public interface IAPIHandler
    {
        APIConstants.RequestMethod GetRequestMethod();

        string GetUrlPath();

        JObject GetRequestBody();

        JObject GetRequestHeaders();

        JObject GetRequestQueryParams();

        Dictionary<string, string> GetRequestHeadersAsDict();

        Dictionary<string, string> GetRequestQueryParamsAsDict();
    }
}
