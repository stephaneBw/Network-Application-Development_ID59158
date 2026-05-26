using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ExchangeOffice.Contracts
{
    [ServiceContract]
    public interface IExchangeOfficeService
    {
        [OperationContract]
        decimal GetCurrentRate(string currencyCode);

        [OperationContract]
        int RegisterUser(string username, string password);

        [OperationContract]
        void TopUpPln(int userId, decimal amount);

        [OperationContract]
        void BuyCurrency(int userId, string currencyCode, decimal foreignAmount);

        [OperationContract]
        void SellCurrency(int userId, string currencyCode, decimal foreignAmount);

        [OperationContract]
        decimal GetBalance(int userId, string currencyCode);

        [OperationContract]
        List<TransactionDto> GetTransactionHistory(int userId);

        [OperationContract]
        List<RatePointDto> GetHistoricalRates(string currencyCode, DateTime from, DateTime to);
    }

    [DataContract]
    public class TransactionDto
    {
        [DataMember] public DateTime Date { get; set; }
        [DataMember] public string Type { get; set; }
        [DataMember] public string Currency { get; set; }
        [DataMember] public decimal Amount { get; set; }
        [DataMember] public decimal Rate { get; set; }
    }

    [DataContract]
    public class RatePointDto
    {
        [DataMember] public DateTime Date { get; set; }
        [DataMember] public decimal Rate { get; set; }
    }
}