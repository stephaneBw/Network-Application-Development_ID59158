using System;
using System.Windows;
using ExchangeOffice.Client.ExchangeOfficeRef; // Connects to network channel link

namespace ExchangeOffice.Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetRateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Open up a connection link to WCF exchange service
                var client = new ExchangeOfficeServiceClient();

                // 2. Grab the text input from the UI box and ask WCF for the live NBP rate
                string code = CurrencyTextBox.Text;
                decimal rate = client.GetCurrentRate(code);

                // 3. Print the resulting bank decimal output neatly on the desktop screen
                RateTextBlock.Text = $"1 {code.ToUpper()} = {rate} PLN";

                // 4. Safely close the network pipeline
                client.Close();
            }
            catch (Exception ex)
            {
                // Show a nice Windows alert window if the user types a fake currency code like XXX
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}