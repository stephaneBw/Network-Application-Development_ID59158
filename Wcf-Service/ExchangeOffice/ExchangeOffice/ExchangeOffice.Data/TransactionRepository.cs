using System;
using System.Data.SqlClient;

namespace ExchangeOffice.Data
{
    public class TransactionRepository
    {
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
    }
}