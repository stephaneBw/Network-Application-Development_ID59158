using System.ServiceModel;

namespace NbpRatesService
{
    [ServiceContract]
    public interface IExchangeRateService
    {
        [OperationContract]
        decimal GetExchangeRate(string currencyCode);
    }
}