using System;
using SampleSDK.OAuth.Client;
using SampleSDK.OAuth.Common;
using SampleSDK.OAuth.Contract;
using MySql.Data.MySqlClient;

namespace SampleSDK.OAuth.ClientApp
{
    class ZohoOAuthDBPersistence : IZohoPersistenceHandler
    {
        public void DeleteOAuthTokens(string userMailId)
        {
            string connectionString = $"server=localhost;database=zohooauth;port=3306;username={GetMySqlUserName()};password={GetMySqlPassword()}";
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                string commandStatement = "delete from oauthtokens where useridentifier = @userIdentifier";
                connection.Open();
                MySqlCommand command = new MySqlCommand(commandStatement, connection);
                command.Parameters.AddWithValue("@userIdentifier", userMailId);
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                //TODO: Log the exceptions;
                //Console.WriteLine(e.ToString());
                throw new ZohoOAuthException(e);
            }
            finally
            {
                if (connection != null)
                {
                    Console.WriteLine("Connection closed");
                    connection.Close();
                }
            }
        }



        public ZohoOAuthTokens GetOAuthTokens(string userMailId)
        {
            string connectionString = $"server=localhost;database=zohooauth;port=3306;username={GetMySqlUserName()};password={GetMySqlPassword()}";
            MySqlConnection connection = null;
            ZohoOAuthTokens tokens = new ZohoOAuthTokens();
            Boolean isUserAvailable = false;
            try
            {
                connection = new MySqlConnection(connectionString);
                string commandStatement = "select * from oauthtokens where useridentifier = @userIdentifier";
                connection.Open();
                MySqlCommand command = new MySqlCommand(commandStatement, connection);
                command.Parameters.AddWithValue("@userIdentifier", userMailId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        isUserAvailable = true;
                        tokens.UserMaiilId = userMailId;
                        tokens.AccessToken = reader["accesstoken"].ToString();
                        tokens.RefreshToken = reader["refreshtoken"].ToString();
                        tokens.ExpiryTime = Convert.ToInt64(reader["expirytime"]);
                    }
                }
                if (!isUserAvailable)
                {
                    throw new ZohoOAuthException("User not available in persistence");
                }
            }
            catch (MySqlException e)
            {
                tokens = null;
                //TODO: Log the info;
                Console.WriteLine(e.ToString());
                throw new ZohoOAuthException(e);
            }
            finally
            {
                if (connection != null)
                {
                    Console.WriteLine("Connection closed");
                    connection.Close();
                }
            }
            return tokens;
        }



        public void SaveOAuthData(ZohoOAuthTokens zohoOAuthTokens)
        {
            string connectionString = $"server=localhost;database=zohooauth;port=3306;username={GetMySqlUserName()};password={GetMySqlPassword()}";
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                string commandStatement = "insert into oauthtokens (useridentifier, accesstoken, refreshtoken, expirytime) values (@userIdentifier, @accessToken, @refreshToken, @expiryTime) on duplicate key update accesstoken = @accessToken, refreshtoken = @refreshToken, expirytime = @expiryTime";
                connection.Open();
                MySqlCommand command = new MySqlCommand(commandStatement, connection);
                command.Parameters.AddWithValue("@userIdentifier", zohoOAuthTokens.UserMaiilId);
                command.Parameters.AddWithValue("@accessToken", zohoOAuthTokens.AccessToken);
                command.Parameters.AddWithValue("@refreshToken", zohoOAuthTokens.RefreshToken);
                command.Parameters.AddWithValue("@expiryTime", zohoOAuthTokens.ExpiryTime);
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (connection != null)
                {
                    Console.WriteLine("Connection closed");
                    connection.Close();
                }
            }

        }

        public static string GetMySqlUserName()
        {
            string userName = ZohoOAuth.GetConfigValue("mysql_username");
            if (userName == null)
            {
                return "root";
            }
            return userName;
        }

        public static string GetMySqlPassword()
        {
            string password = ZohoOAuth.GetConfigValue("mysql_password");
            if (password == null)
            {
                return "";
            }
            return password;
        }
    }
}
