using System;
using System.Collections.Generic;

namespace ExchangeOffice.Business
{
    public class ExchangeOfficeManager
    {
        // Thread-safe singleton pattern so the entire app shares the exact same RAM memory data
        private static readonly ExchangeOfficeManager _instance = new ExchangeOfficeManager();
        public static ExchangeOfficeManager Instance => _instance;

        private readonly List<User> _users = new List<User>();
        private readonly List<Transaction> _transactions = new List<Transaction>();

        // Maps "userId:CURRENCY" (e.g. "1:USD") to a numerical balance
        private readonly Dictionary<string, decimal> _balances = new Dictionary<string, decimal>();
        private int _nextUserId = 1;

        private ExchangeOfficeManager() { } // Constructor hidden for singleton

        // Helper to generate consistent keys for our memory dictionary
        private string GetBalanceKey(int userId, string currencyCode)
        {
            return $"{userId}:{currencyCode.Trim().ToUpperInvariant()}";
        }

        public int RegisterUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password cannot be empty.");

            var user = new User { Id = _nextUserId++, Username = username, Password = password };
            _users.Add(user);
            return user.Id;
        }

        public void TopUpPln(int userId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Top-up amount must be greater than zero.");

            string key = GetBalanceKey(userId, "PLN");

            if (!_balances.ContainsKey(key)) _balances[key] = 0;
            _balances[key] += amount;

            _transactions.Add(new Transaction
            {
                UserId = userId,
                Date = DateTime.Now,
                Type = "TopUp",
                Currency = "PLN",
                Amount = amount,
                Rate = 1.0m
            });
        }

        public decimal GetBalance(int userId, string currencyCode)
        {
            string key = GetBalanceKey(userId, currencyCode);
            return _balances.ContainsKey(key) ? _balances[key] : 0m;
        }

        public void BuyCurrency(int userId, string currencyCode, decimal foreignAmount, decimal rate)
        {
            if (foreignAmount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            if (rate <= 0) throw new ArgumentException("Invalid exchange rate.");

            decimal costPln = foreignAmount * rate;
            decimal userPln = GetBalance(userId, "PLN");

            if (userPln < costPln)
                throw new InvalidOperationException($"Insufficient PLN balance. Cost: {costPln} PLN, Available: {userPln} PLN.");

            // Deduct PLN
            _balances[GetBalanceKey(userId, "PLN")] -= costPln;

            // Add Foreign Currency
            string foreignKey = GetBalanceKey(userId, currencyCode);
            if (!_balances.ContainsKey(foreignKey)) _balances[foreignKey] = 0m;
            _balances[foreignKey] += foreignAmount;

            // Record transaction log
            _transactions.Add(new Transaction
            {
                UserId = userId,
                Date = DateTime.Now,
                Type = "Buy",
                Currency = currencyCode.ToUpperInvariant(),
                Amount = foreignAmount,
                Rate = rate
            });
        }

        public void SellCurrency(int userId, string currencyCode, decimal foreignAmount, decimal rate)
        {
            if (foreignAmount <= 0) throw new ArgumentException("Amount must be greater than zero.");
            if (rate <= 0) throw new ArgumentException("Invalid exchange rate.");

            string foreignKey = GetBalanceKey(userId, currencyCode);
            decimal userForeignBalance = GetBalance(userId, currencyCode);

            if (userForeignBalance < foreignAmount)
                throw new InvalidOperationException($"Insufficient balance in {currencyCode.ToUpperInvariant()}.");

            // Deduct Foreign Currency
            _balances[foreignKey] -= foreignAmount;

            // Add PLN
            decimal gainPln = foreignAmount * rate;
            _balances[GetBalanceKey(userId, "PLN")] += gainPln;

            // Record transaction log
            _transactions.Add(new Transaction
            {
                UserId = userId,
                Date = DateTime.Now,
                Type = "Sell",
                Currency = currencyCode.ToUpperInvariant(),
                Amount = foreignAmount,
                Rate = rate
            });
        }
    }
}