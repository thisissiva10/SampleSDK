using System;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Common;

namespace SampleSDK.CRM.Library.Setup.Restclient
{
    public class ZCRMRestClient
    {

        //TODO: ThreadLocal Current_User_Mail_id;

        private static Dictionary<string, string> staticHeaders = new Dictionary<string, string>();

        //TODO: ThreadLocal Dynamic_Headers;


        public static Dictionary<string, string> StaticHeaders { get => staticHeaders; set => staticHeaders = value; }

        public ZCRMRestClient() { }

        public static ZCRMRestClient GetInstance()
        {
            return new ZCRMRestClient();
        }

        //TODO: Throw exception and pass proper argument
        public static void Initialize()
        {
            Initialize(true);
        }

        public static void Initialize(Boolean handleAuthentication)
        {
            ZCRMConfigUtil.Initialize(handleAuthentication);
        }

        //TODO: Handle all remianing methods!!;
    }
}
