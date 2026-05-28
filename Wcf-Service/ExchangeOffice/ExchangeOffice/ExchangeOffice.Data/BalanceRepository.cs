using System;
using System.Data.SqlClient;
using System.Configuration;

namespace ExchangeOffice.Data
{
    public class BalanceRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ExchangeOfficeDb"].ConnectionString;

        public decimal GetBalance(int userId, string currency)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Amount FROM Balances WHERE UserId = @UserId AND UPPER(CurrencyCode) = UPPER(@Currency)";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Currency", currency.Trim());
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToDecimal(result) : 0m;
                }
            }
        }

        // Read balance inside an open transaction (buy/sell consistency).
        public decimal GetBalance(int userId, string currency, SqlConnection conn, SqlTransaction tx)
        {
            string sql = "SELECT Amount FROM Balances WHERE UserId = @UserId AND UPPER(CurrencyCode) = UPPER(@Currency)";
            using (var cmd = new SqlCommand(sql, conn, tx))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Currency", currency.Trim());
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0m;
            }
        }

        public void AddToBalance(int userId, string currency, decimal delta, SqlConnection conn, SqlTransaction tx)
        {
            string currencyCode = currency.Trim().ToUpper();

            // SQL UPSERT: Updates the balance if it exists, or inserts a row if it doesn't
            string sql = @"
                IF EXISTS (SELECT 1 FROM Balances WHERE UserId = @UserId AND CurrencyCode = @Currency)
                    UPDATE Balances SET Amount = Amount + @Delta WHERE UserId = @UserId AND CurrencyCode = @Currency
                ELSE
                    INSERT INTO Balances (UserId, CurrencyCode, Amount) VALUES (@UserId, @Currency, @Delta);";

            using (var cmd = new SqlCommand(sql, conn, tx))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Currency", currencyCode);
                cmd.Parameters.AddWithValue("@Delta", delta);
                cmd.ExecuteNonQuery();
            }
        }
    }
}