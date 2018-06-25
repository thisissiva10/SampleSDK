using SampleSDK.OAuth.Contract;


namespace SampleSDK.OAuth.Client
{
    public interface IZohoPersistenceHandler
    {
        void SaveOAuthData(ZohoOAuthTokens zohoOAuthTokens);

        ZohoOAuthTokens GetOAuthTokens(string paramString);

        void DeleteOAuthTokens(string paramName);

    }
}
