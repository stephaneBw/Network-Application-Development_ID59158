using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ExchangeOffice.Nbp
{
    public class NbpClient
    {
        public decimal GetCurrentRate(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency code is required.");

            string code = currencyCode.Trim().ToLowerInvariant();

            // If the requested currency is already PLN, the exchange rate to itself is always 1
            if (code == "pln") return 1.0m;

            string url = $"https://api.nbp.pl/api/exchangerates/rates/a/{code}/?format=json";

            using (var http = new HttpClient())
            {
                var response = http.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not fetch live rate from NBP for code: '{currencyCode.ToUpper()}'.");

                string json = response.Content.ReadAsStringAsync().Result;
                var data = JObject.Parse(json);

                return data["rates"][0]["mid"].Value<decimal>();
            }
        }
    }
}