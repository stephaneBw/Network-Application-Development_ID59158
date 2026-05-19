using System;
using HelloWcfClient.CalculatorRef; 

namespace HelloWcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Service1Client();

            int result = client.Add(10, 25);
            Console.WriteLine("10 + 25 = " + result);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            client.Close();
        }
    }
}