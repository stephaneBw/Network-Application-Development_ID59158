using System;
using System.Data.SqlClient;
using System.Configuration;

namespace ExchangeOffice.Data
{
    public class UserRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ExchangeOfficeDb"].ConnectionString;

        public bool UsernameExists(string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM Users WHERE LOWER(Username) = LOWER(@Username)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username.Trim());
                    conn.Open();
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public int CreateUser(string username, string passwordHash)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Users (Username, PasswordHash) OUTPUT INSERTED.Id VALUES (@Username, @PasswordHash)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}