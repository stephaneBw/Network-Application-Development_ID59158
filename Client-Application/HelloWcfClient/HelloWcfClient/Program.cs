using System;
using HelloWcfClient.NbpRef; // Connects to your new live service reference

namespace HelloWcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Create a connection client to the NBP Exchange Rate service
            var client = new ExchangeRateServiceClient();

            Console.WriteLine("--- Fetching Live Exchange Rates from NBP ---");

            try
            {
                // 2. Query USD and print the returned live value
                decimal usdRate = client.GetExchangeRate("USD");
                Console.WriteLine("USD: " + usdRate + " PLN");

                // 3. Query EUR and print the returned live value
                decimal eurRate = client.GetExchangeRate("EUR");
                Console.WriteLine("EUR: " + eurRate + " PLN");

                // 4. Lab 4 Error Handling Test: Let's see what happens with a fake currency
                Console.WriteLine("\nTesting invalid code 'XXX'...");
                decimal fakeRate = client.GetExchangeRate("XXX");
            }
            catch (Exception ex)
            {
                // Catch the FaultException sent by your service and display it nicely
                Console.WriteLine("Error caught: " + ex.Message);
            }

            Console.WriteLine("\nPress Enter to exit.");
            Console.ReadLine();

            client.Close();
        }
    }
}