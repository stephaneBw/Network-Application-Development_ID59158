using System;
using System.Windows;
using System.Windows.Media;
using ExchangeOffice.Client.ExchangeOfficeRef;

namespace ExchangeOffice.Client
{
    public partial class MainWindow : Window
    {
        private int? _currentUserId = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Step 2 — Helper: Ensure the active process contains an authentic registration tracking identity
        private bool EnsureLoggedIn()
        {
            if (_currentUserId == null)
            {
                MessageBox.Show("Operational Lockout: Please establish an active User Account Registration identity first (Lab 9).", "Authentication Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        // Step 3 — Handlers implementation
        private void TopUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnsureLoggedIn()) return;

            // Culture-safe string-to-decimal calculation filters
            if (!decimal.TryParse(TopUpTextBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show("Input Validation Error: Please enter a valid numerical balance layout format.", "Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                client.TopUpPln(_currentUserId.Value, amount);
                client.Close();

                MessageBox.Show($"Successfully deposited {amount:N2} PLN into your account allocation memory.", "Deposit Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshBalances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Top-up transaction failed: " + ex.Message, "WCF System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnsureLoggedIn()) return;

            string currency = TradeCurrencyTextBox.Text.Trim();
            if (!decimal.TryParse(TradeAmountTextBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show("Input Validation Error: Invalid foreign quantity metric.", "Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                client.BuyCurrency(_currentUserId.Value, currency, amount);
                client.Close();

                MessageBox.Show($"Trade Executed: Successfully purchased {amount} {currency.ToUpper()} using live market rate matrices.", "Purchase Confirmed", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshBalances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Purchase execution transaction failed:\n" + ex.Message, "Exchange Engine Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnsureLoggedIn()) return;

            string currency = TradeCurrencyTextBox.Text.Trim();
            if (!decimal.TryParse(TradeAmountTextBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show("Input Validation Error: Invalid foreign quantity metric.", "Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                client.SellCurrency(_currentUserId.Value, currency, amount);
                client.Close();

                MessageBox.Show($"Trade Executed: Successfully liquidated {amount} {currency.ToUpper()} back to the exchange reserves.", "Liquidation Confirmed", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshBalances();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Liquidation execution transaction failed:\n" + ex.Message, "Exchange Engine Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshBalancesButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshBalances();
        }

        private void RefreshBalances()
        {
            if (!EnsureLoggedIn()) return;

            try
            {
                var client = new ExchangeOfficeServiceClient();

                // Fetch independent allocations directly from core service RAM dictionary structures
                decimal pln = client.GetBalance(_currentUserId.Value, "PLN");
                decimal usd = client.GetBalance(_currentUserId.Value, "USD");
                decimal eur = client.GetBalance(_currentUserId.Value, "EUR");

                client.Close();

                BalancesTextBlock.Text = $"CURRENT USER ID: {_currentUserId.Value}\n" +
                                         $"--------------------------------\n" +
                                         $"PLN Balance: {pln:N4} PLN\n" +
                                         $"USD Balance: {usd:N4} USD\n" +
                                         $"EUR Balance: {eur:N4} EUR";
            }
            catch (Exception ex)
            {
                BalancesTextBlock.Text = "Error communicating with accounting balance ledger: " + ex.Message;
            }
        }

        // Lab 9: Account user establishment registration module
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text.Trim();
                string password = PasswordBox.Password;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter both a username and a password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var client = new ExchangeOfficeServiceClient();
                int userId = client.RegisterUser(username, password);
                client.Close();

                _currentUserId = userId;
                UserStatusTextBlock.Text = $"Status: Logged In! Active Account ID: {userId}";
                UserStatusTextBlock.Foreground = Brushes.Green;

                RefreshBalances();
                MessageBox.Show($"Welcome, {username}!\nYour unique account User ID is: {userId}", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration pipeline failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Lab 8: Marketplace NBP lookup reference locator engine
        private void GetRateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new ExchangeOfficeServiceClient();
                string code = CurrencyTextBox.Text.Trim();
                decimal rate = client.GetCurrentRate(code);
                RateTextBlock.Text = $"1 {code.ToUpper()} = {rate} PLN";
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}