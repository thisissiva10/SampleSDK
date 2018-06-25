using System.Collections.Generic;
namespace SampleSDK.CRM.Library.Api.Handler
{
    public interface IAPIHandler
    {
        string GetUrlPath();

        Dictionary<string, string> GetRequestHeadersAsDict();

        Dictionary<string, string> GetRequestQueryParamsAsDict();

        //TODO: Declare GetRequestHeaders(), GetRequestBody(), GetRequestQueryParams() which all returns JSONObject;
    }
}
