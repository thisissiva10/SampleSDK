using System;
namespace SampleSDK.OAuth.Contract
{
    public class ZohoOAuthTokens
    {

        private string userMailId;
        private string accessToken;
        private string refreshToken;
        private long expiryTime;

        public ZohoOAuthTokens() { }

        public string UserMaiilId { get => userMailId; set => userMailId = value; }

        public string RefreshToken { get => refreshToken; set => refreshToken = value; }

        public long ExpiryTime { get => expiryTime; set => expiryTime = value; }

        public string AccessToken { 
            get {
                if(IsAccessTokenValid()){
                    return accessToken;
                }

                //TODO: Throw an appropriate exception;
                throw new Exception("Access token expired");
            }
            set => accessToken = value; 
        }



        private Boolean IsAccessTokenValid(){
            if(GetExpiryLapseInMillis() > 10L){
                return true;
            }
            return false;
        }

        //TODO: Got to look into this logic;
        public long GetExpiryLapseInMillis() {
            long time = (ExpiryTime - (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
            return (time);
        }

        //TODO: JsonObject to Json Method();

    }
}
