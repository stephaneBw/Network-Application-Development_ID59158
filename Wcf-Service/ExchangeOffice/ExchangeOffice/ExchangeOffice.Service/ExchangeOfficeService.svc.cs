using System;
using System.Collections.Generic;
using ExchangeOffice.Contracts;
using ExchangeOffice.Business;
using ExchangeOffice.Nbp; // Connects to your new bank client layer

namespace ExchangeOffice.Service
{
    public class ExchangeOfficeService : IExchangeOfficeService
    {
        private readonly ExchangeOfficeManager _manager = ExchangeOfficeManager.Instance;
        private readonly NbpClient _nbpClient = new NbpClient(); // Live bank tracker

        public decimal GetCurrentRate(string currencyCode)
        {
            // Fetch live data directly from the National Bank of Poland API
            return _nbpClient.GetCurrentRate(currencyCode);
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
            // 1. Fetch live transaction rate from the bank server dynamically
            decimal liveRate = GetCurrentRate(currencyCode);

            // 2. Process exchange using the real rate matrix
            _manager.BuyCurrency(userId, currencyCode, foreignAmount, liveRate);
        }

        public void SellCurrency(int userId, string currencyCode, decimal foreignAmount)
        {
            // 1. Fetch live transaction rate from the bank server dynamically
            decimal liveRate = GetCurrentRate(currencyCode);

            // 2. Process exchange using the real rate matrix
            _manager.SellCurrency(userId, currencyCode, foreignAmount, liveRate);
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