using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Fetches NBP table A mid rates for a date range (inclusive).
        /// API: /api/exchangerates/rates/a/{code}/{start}/{end}/?format=json
        /// </summary>
        public List<NbpRatePoint> GetHistoricalRates(string currencyCode, DateTime from, DateTime to)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency code is required.");

            string code = currencyCode.Trim().ToLowerInvariant();
            if (code == "pln")
                throw new ArgumentException("Historical NBP rates are not available for PLN.");

            DateTime start = from.Date;
            DateTime end = to.Date;
            if (start > end)
                throw new ArgumentException("'from' date must be on or before 'to' date.");

            string url = $"https://api.nbp.pl/api/exchangerates/rates/a/{code}/{start:yyyy-MM-dd}/{end:yyyy-MM-dd}/?format=json";

            using (var http = new HttpClient())
            {
                var response = http.GetAsync(url).Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Could not fetch historical rates from NBP for '{currencyCode.ToUpper()}' ({start:yyyy-MM-dd} to {end:yyyy-MM-dd}).");

                string json = response.Content.ReadAsStringAsync().Result;
                var data = JObject.Parse(json);
                var ratesArray = data["rates"];

                if (ratesArray == null || !ratesArray.HasValues)
                    return new List<NbpRatePoint>();

                var points = new List<NbpRatePoint>();
                foreach (var item in ratesArray)
                {
                    points.Add(new NbpRatePoint
                    {
                        Date = DateTime.Parse(item["effectiveDate"].ToString()),
                        Rate = item["mid"].Value<decimal>()
                    });
                }

                return points.OrderBy(p => p.Date).ToList();
            }
        }
    }
}