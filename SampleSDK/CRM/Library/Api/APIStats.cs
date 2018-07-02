using System;
using System.Collections.Generic;
using System.Net;

namespace SampleSDK.CRM.Library.Api
{
    public class APIStats
    {
        private static Dictionary<string, string> apiCountStats = new Dictionary<string, string>();

        public APIStats() { }

        public static void UpdateStats(HttpWebResponse response)
        {
            UpdateCountStats(response);
        }

        private static void UpdateCountStats(HttpWebResponse response)
        {
            apiCountStats.Add("X-RATELIMIT-LIMIT", response.GetResponseHeader("X-RATELIMIT-LIMIT"));
            apiCountStats.Add("X-RATELIMIT-REMAINING", response.GetResponseHeader("X-RATELIMIT-REMAINING"));
            apiCountStats.Add("X-RATELIMIT-RESET", response.GetResponseHeader("X-RATELIMIT-RESET"));
        }

        public static string GetRemainingCountForTheDay()
        {
            return apiCountStats["X-RATELIMIT-LIMIT"];
        }

        public static string GetRemainingCountForCurrentWindow()
        {
            return apiCountStats["X-RATELIMIT-REMAINING"];
        }
        public static string GetRemainingTimeForWindowReset()
        {
            return apiCountStats["X-RATELIMIT-RESET"];
        }
    }
}
