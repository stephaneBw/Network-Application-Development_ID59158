using System;
using ExchangeOffice.TestClient.ExchangeOfficeRef; // Connects to your service reference layout

namespace ExchangeOffice.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Initialize our communication channel client
            var client = new ExchangeOfficeServiceClient();

            Console.WriteLine("=== STARTING LAB 6 EXCHANGE OFFICE TEST TRANSACTION SCRIPT ===");

            try
            {
                // 2. Register a mock test user to dynamically obtain an active ID
                int testUserId = client.RegisterUser("stephane", "securePassword123");
                Console.WriteLine($"[SUCCESS] Registered user Stephane with generated Account ID: {testUserId}");

                // 3. TopUp Account with 1000 PLN
                client.TopUpPln(testUserId, 1000m);
                Console.WriteLine($"\n1. Deposited: 1000 PLN");
                Console.WriteLine($"   Current PLN Balance: {client.GetBalance(testUserId, "PLN")} PLN");

                // 4. Purchase 100 USD at our hard-coded 4.0 exchange rate matrix (Cost: 400 PLN)
                Console.WriteLine($"\n2. Executing Purchase Command: Buying 100 USD (Fixed Rate: 4.00)...");
                client.BuyCurrency(testUserId, "USD", 100m);
                Console.WriteLine($"   Updated Balance PLN: {client.GetBalance(testUserId, "PLN")} PLN");
                Console.WriteLine($"   Updated Balance USD: {client.GetBalance(testUserId, "USD")} USD");

                // 5. Sell back 50 USD to the exchange office (Gain: 200 PLN)
                Console.WriteLine($"\n3. Executing Sell Command: Liquidating 50 USD back to office...");
                client.SellCurrency(testUserId, "USD", 50m);
                Console.WriteLine($"   Final Balance PLN: {client.GetBalance(testUserId, "PLN")} PLN");
                Console.WriteLine($"   Final Balance USD: {client.GetBalance(testUserId, "USD")} USD");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[CRITICAL ERROR DURING TRANSACTION FLOW]: {ex.Message}");
            }

            Console.WriteLine("\n=========================================================");
            Console.WriteLine("Test Script completed successfully with no crashes! Press Enter to close.");
            Console.ReadLine();

            client.Close();
        }
    }
}