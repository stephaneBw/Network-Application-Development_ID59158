using System;
using System.Collections.Generic;
using ExchangeOffice.Contracts;

namespace ExchangeOffice.Service
{
    public class ExchangeOfficeService : IExchangeOfficeService
    {
        public void DoWork()
        {
            // Required by IExchangeOfficeService; no-op implementation.
        }

        public decimal GetCurrentRate(string currencyCode)
        {
            throw new NotImplementedException("Lab 7");
        }

        public int RegisterUser(string username, string password)
        {
            throw new NotImplementedException("Lab 9");
        }

        public void TopUpPln(int userId, decimal amount)
        {
            throw new NotImplementedException("Lab 6");
        }

        public void BuyCurrency(int userId, string currencyCode, decimal foreignAmount)
        {
            throw new NotImplementedException("Lab 6");
        }

        public void SellCurrency(int userId, string currencyCode, decimal foreignAmount)
        {
            throw new NotImplementedException("Lab 6");
        }

        public decimal GetBalance(int userId, string currencyCode)
        {
            throw new NotImplementedException("Lab 6");
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
