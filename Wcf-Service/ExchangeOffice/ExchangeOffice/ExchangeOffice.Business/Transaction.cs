using System;

namespace ExchangeOffice.Business
{
    public class Transaction
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }      // "TopUp", "Buy", "Sell"
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
    }
}