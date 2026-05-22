using System;
using System.Net.Http;
using System.ServiceModel;
using Newtonsoft.Json.Linq;

namespace NbpRatesService
{
    public class ExchangeRateService : IExchangeRateService
    {
        public decimal GetExchangeRate(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new System.ServiceModel.FaultException("Currency code is required.");

            // Convert to lowercase (e.g., "USD" to "usd") because the bank API prefers it
            string code = currencyCode.Trim().ToLowerInvariant();
            string url = $"https://api.nbp.pl/api/exchangerates/rates/a/{code}/?format=json";

            using (var http = new HttpClient())
            {
                // Download the data from the NBP website
                var response = http.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                    throw new System.ServiceModel.FaultException($"No rate found for '{currencyCode}'.");

                // Convert raw download into readable text
                string json = response.Content.ReadAsStringAsync().Result;

                // Parse the text and grab the 'mid' value from the array
                var data = JObject.Parse(json);
                decimal rate = data["rates"][0]["mid"].Value<decimal>();

                return rate;
            }
        }
    }
}