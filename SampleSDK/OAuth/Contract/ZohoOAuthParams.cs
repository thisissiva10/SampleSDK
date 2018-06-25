using System;
namespace SampleSDK.OAuth.Contract
{
    public class ZohoOAuthParams
    {
        private string clientId;
        private string clientSecret;
        private string redirectURL;
        private string accessType;
        private string scopes;

        public ZohoOAuthParams() { }

        public string ClientId { get => clientId; set => clientId = value; }

        public string ClientSecret { get => clientSecret; set => clientSecret = value; }
       
        public string RedirectURL { get => redirectURL; set => redirectURL = value; }

        public string AccessType { get => accessType; set => accessType = value; }

        public string Scopes {
            get => scopes;
            set { 
                if((scopes != null) && (scopes.Contains("AaaServer.profile.Read"))){
                    scopes = value + ",AaaServer.profile.Read";
                }
                scopes = value;
            }
        }
    }
}
