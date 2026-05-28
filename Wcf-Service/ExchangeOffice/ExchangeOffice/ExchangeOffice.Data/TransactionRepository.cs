using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using ExchangeOffice.Contracts;

namespace ExchangeOffice.Data
{
    public class TransactionRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ExchangeOfficeDb"].ConnectionString;

        public void AddTransaction(int userId, string type, string currency, decimal amount, decimal rate, SqlConnection conn, SqlTransaction tx)
        {
            string sql = @"INSERT INTO Transactions (UserId, [Date], [Type], CurrencyCode, Amount, Rate) 
                           VALUES (@UserId, GETDATE(), @Type, @Currency, @Amount, @Rate)";

            using (var cmd = new SqlCommand(sql, conn, tx))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Currency", currency.Trim().ToUpper());
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Rate", rate);
                cmd.ExecuteNonQuery();
            }
        }

        // Lab 12 Step 1 — Fetch historical transaction entries ordered by date
        public List<TransactionDto> GetTransactionsByUser(int userId)
        {
            var history = new List<TransactionDto>();

            using (var conn = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT [Date], [Type], CurrencyCode, Amount, Rate 
                               FROM Transactions 
                               WHERE UserId = @UserId 
                               ORDER BY [Date] DESC";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            history.Add(new TransactionDto
                            {
                                Date = Convert.ToDateTime(reader["Date"]),
                                Type = reader["Type"].ToString(),
                                Currency = reader["CurrencyCode"].ToString(),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                Rate = Convert.ToDecimal(reader["Rate"])
                            });
                        }
                    }
                }
            }
            return history;
        }
    }
}