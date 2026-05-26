using System;
using System.Collections.Generic;
using ExchangeOffice.Contracts;
using ExchangeOffice.Business; // Connects to your business layer domain

namespace ExchangeOffice.Service
{
    public class ExchangeOfficeService : ExchangeOffice.Contracts.IExchangeOfficeService
    {
        // Reference our single, persistent memory manager instance
        private readonly ExchangeOfficeManager _manager = ExchangeOfficeManager.Instance;

        public decimal GetCurrentRate(string currencyCode)
        {
            // Hard-coded exchange rate rules for Lab 6 (until Lab 7 plugs in the bank)
            if (string.Equals(currencyCode, "USD", StringComparison.OrdinalIgnoreCase)) return 4.0m;
            if (string.Equals(currencyCode, "EUR", StringComparison.OrdinalIgnoreCase)) return 4.5m;
            return 1.0m;
        }

        public int RegisterUser(string username, string password)
        {
            return _manager.RegisterUser(username, password);
        }

        public void TopUpPln(int userId, decimal amount)
        {
            _manager.TopUpPln(userId, amount);
        }

        public void BuyCurrency(int userId, string currencyCode, decimal foreignAmount)
        {
            decimal rate = GetCurrentRate(currencyCode); // Uses our fixed 4.0m rate
            _manager.BuyCurrency(userId, currencyCode, foreignAmount, rate);
        }

        public void SellCurrency(int userId, string currencyCode, decimal foreignAmount)
        {
            decimal rate = GetCurrentRate(currencyCode);
            _manager.SellCurrency(userId, currencyCode, foreignAmount, rate);
        }

        public decimal GetBalance(int userId, string currencyCode)
        {
            return _manager.GetBalance(userId, currencyCode);
        }

        public List<TransactionDto> GetTransactionHistory(int userId)
        {
            throw new NotImplementedException("Lab 12");
        }

        public List<RatePointDto> GetHistoricalRates(string currencyCode, DateTime from, DateTime to)
        {
            throw new NotImplementedException("Lab 13");
        }
    }
}