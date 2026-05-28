using System;
using System.Data.SqlClient;
using System.Configuration;
using ExchangeOffice.Data;

namespace ExchangeOffice.Business
{
    public class ExchangeOfficeManager
    {
        private static readonly ExchangeOfficeManager _instance = new ExchangeOfficeManager();
        public static ExchangeOfficeManager Instance => _instance;

        private readonly UserRepository _userRepo = new UserRepository();
        private readonly BalanceRepository _balanceRepo = new BalanceRepository();
        private readonly TransactionRepository _txRepo = new TransactionRepository();

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ExchangeOfficeDb"].ConnectionString;

        private ExchangeOfficeManager() { }

        // Secure baseline string password encoding converter block
        private string HashPassword(string password)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public int RegisterUser(string username, string password)
        {
            if (_userRepo.UsernameExists(username))
                throw new InvalidOperationException("Account registration error: Username is already taken.");

            string passHash = HashPassword(password);
            return _userRepo.CreateUser(username, passHash);
        }

        public decimal GetBalance(int userId, string currencyCode)
        {
            return _balanceRepo.GetBalance(userId, currencyCode);
        }

        public void TopUpPln(int userId, decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Top-up amount must be greater than zero.");

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        _balanceRepo.AddToBalance(userId, "PLN", amount, conn, tx);
                        _txRepo.AddTransaction(userId, "TopUp", "PLN", amount, 1.0m, conn, tx);
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void BuyCurrency(int userId, string currencyCode, decimal foreignAmount, decimal rate)
        {
            if (foreignAmount <= 0 || rate <= 0) throw new ArgumentException("Invalid financial trading parameters.");
            string targetCurrency = currencyCode.Trim().ToUpper();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        decimal costPln = foreignAmount * rate;
                        decimal currentPln = _balanceRepo.GetBalance(userId, "PLN");

                        if (currentPln < costPln)
                            throw new InvalidOperationException($"Insufficient funds. Cost: {costPln} PLN, Available: {currentPln} PLN.");

                        _balanceRepo.AddToBalance(userId, "PLN", -costPln, conn, tx);
                        _balanceRepo.AddToBalance(userId, targetCurrency, foreignAmount, conn, tx);
                        _txRepo.AddTransaction(userId, "Buy", targetCurrency, foreignAmount, rate, conn, tx);

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SellCurrency(int userId, string currencyCode, decimal foreignAmount, decimal rate)
        {
            if (foreignAmount <= 0 || rate <= 0) throw new ArgumentException("Invalid financial trading parameters.");
            string targetCurrency = currencyCode.Trim().ToUpper();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        decimal currentForeign = _balanceRepo.GetBalance(userId, targetCurrency);
                        if (currentForeign < foreignAmount)
                            throw new InvalidOperationException($"Insufficient balance in portfolio for asset: {targetCurrency}.");

                        decimal gainPln = foreignAmount * rate;

                        _balanceRepo.AddToBalance(userId, targetCurrency, -foreignAmount, conn, tx);
                        _balanceRepo.AddToBalance(userId, "PLN", gainPln, conn, tx);
                        _txRepo.AddTransaction(userId, "Sell", targetCurrency, foreignAmount, rate, conn, tx);

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}