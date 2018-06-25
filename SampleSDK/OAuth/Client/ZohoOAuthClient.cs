using System;
using SampleSDK.OAuth.Contract;
using SampleSDK.OAuth.Common;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SampleSDK.OAuth.Client
{


    /* NOTE: GenerateAccessToken() and RefreshToken() methods convert the response to dictionary 
     jsonConvert.Deserialize method */


    public class ZohoOAuthClient
    {
        //TODO: Get Logger

        private static ZohoOAuthClient client = null;
        private ZohoOAuthParams oAuthParams;

        public ZohoOAuthClient(ZohoOAuthParams oAuthParams)
        {
            this.oAuthParams = oAuthParams;
        }

        public static ZohoOAuthClient GetInstance(ZohoOAuthParams oAuthParams){
            client = client ?? new ZohoOAuthClient(oAuthParams);
            return client;
        }

        public static ZohoOAuthClient GetInstance(){
            return client;
        }

        //TODO: Throw exceptions;
        public static void Initialize() { }

        //TODO: Throw ZohoOAuthException ;
        public string GetAccessToken(string userMailId) {
            IZohoPersistenceHandler persistenceHandler = ZohoOAuth.GetPersistenceHandlerInstance();

            ZohoOAuthTokens tokens;
            try{
                Console.WriteLine("Getting tokens");
                tokens = persistenceHandler.GetOAuthTokens(userMailId);
            }catch(Exception e){
                Console.WriteLine("Exception while retrieving tokens from persistence "+ e.Message);
                throw;
            }
            try
            {
                return tokens.AccessToken;
            }catch(Exception e){
                Console.WriteLine(e.Message);
                tokens = RefreshAccessToken(tokens.RefreshToken, userMailId);
            }
            return tokens.AccessToken;
         } 


        //TODO: Generate ZohoOAuthTokens Class for GenerateAccessTokens Method();
        //TODO: This method throws an exception;
        public ZohoOAuthTokens GenerateAccessToken(string grantToken)
        {
            if (grantToken == null || grantToken.Length == 0){
                //TODO: Throw an ZohoOAuthException;
                Console.WriteLine("Null Grant Token");
            }
            //TODO: Inspect the usage of Contract;

            ZohoHTTPConnector conn = GetZohoConnector(ZohoOAuth.GetTokenURL());
            conn.AddParam("grant_type", "authorization_code");
            conn.AddParam("code", grantToken);
            string response = conn.Post();

            Dictionary<string, string> responseJSON = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            if(responseJSON.ContainsKey("access_token")){
                ZohoOAuthTokens tokens = GetTokensFromJSON(responseJSON);
                tokens.UserMaiilId = GetUserMailId(tokens.AccessToken);
                ZohoOAuth.GetPersistenceHandlerInstance().SaveOAuthData(tokens);
                return tokens;
            }

            //TODO: Throw exceptions and remove the return statement and console statement;
            Console.WriteLine("Exception fetching access tokens");
            return null;
        }


        //TODO: Create ZohoOAuthException class and change the throw exception class;
        private ZohoOAuthTokens RefreshAccessToken(string refreshToken, string userMailId){
            if(refreshToken == null){
                throw new Exception("Refresh token is not provided");
            }

            try{
                ZohoHTTPConnector conn = GetZohoConnector(ZohoOAuth.GetRefreshTokenURL());
                conn.AddParam("grant_type", "refresh_token");
                conn.AddParam("refresh_token", refreshToken);
                string response = conn.Post();
                Dictionary<string, string> responseJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                if(responseJson.ContainsKey("access_token")){
                    ZohoOAuthTokens tokens = GetTokensFromJSON(responseJson);
                    tokens.RefreshToken = refreshToken;
                    tokens.UserMaiilId = userMailId;
                    ZohoOAuth.GetPersistenceHandlerInstance().SaveOAuthData(tokens);
                    return tokens;
                }

                throw new Exception("Exception while fetching access tokens from site");
            }catch(Exception e){
                Console.WriteLine(e.Message);
                throw;
            }
        }


        //TODO: the method throws three exceptions and check for null exception on access_token.
        private string GetUserMailId(string accessToken)
        {
            ZohoHTTPConnector conn = new ZohoHTTPConnector() { Url = ZohoOAuth.GetUserInfoURL() };
            conn.AddHeader("Authorization", ZohoOAuthConstants.AuthHeaderPrefix + accessToken);
            string response = conn.Get();
            JObject responseJSON = JObject.Parse(response);
            return responseJSON["Email"].ToString();
        }

        private ZohoHTTPConnector GetZohoConnector(string url)
        {
            ZohoHTTPConnector conn = new ZohoHTTPConnector() { Url = url };
            conn.AddParam("client_id", oAuthParams.ClientId);
            conn.AddParam("client_secret", oAuthParams.ClientSecret);
            conn.AddParam("redirect_uri", oAuthParams.RedirectURL);
            return conn;
        }

        //TODO: Throws jsonexception;
        private ZohoOAuthTokens GetTokensFromJSON(Dictionary<string, string> responseDict){
            ZohoOAuthTokens tokens = new ZohoOAuthTokens();
            long expiresIn = Convert.ToInt64(responseDict["expires_in"]);
            tokens.ExpiryTime = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalMilliseconds + expiresIn;
            tokens.AccessToken = responseDict["access_token"];
            if(responseDict.ContainsKey("refresh_token")){
                tokens.RefreshToken = responseDict["refresh_token"];
            }
            return tokens;
        }
    }

}
