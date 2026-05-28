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
            ToggleTradingInterface(false); // Buttons start locked out by default
            InitializeHistoricalDatePickers();
        }

        private void InitializeHistoricalDatePickers()
        {
            HistoryToDatePicker.SelectedDate = DateTime.Today;
            HistoryFromDatePicker.SelectedDate = DateTime.Today.AddDays(-7);
        }

        // Feature 2: Helper to turn trading desk buttons on or off automatically
        private void ToggleTradingInterface(bool isEnabled)
        {
            TopUpButton.IsEnabled = isEnabled;
            BuyButton.IsEnabled = isEnabled;
            SellButton.IsEnabled = isEnabled;
            RefreshHistoryButton.IsEnabled = isEnabled;
        }

        // Feature 6: Centralized status bar messaging engine
        private void LogMessage(string message, bool isError = false)
        {
            if (isError)
            {
                GlobalErrorTextBlock.Text = message;
                UserStatusTextBlock.Text = "Status: Transaction Error";
                UserStatusTextBlock.Foreground = Brushes.Red;
            }
            else
            {
                GlobalErrorTextBlock.Text = string.Empty; // clear previous errors
                if (_currentUserId.HasValue)
                {
                    UserStatusTextBlock.Text = $"Status: Logged In (ID: {_currentUserId})";
                    UserStatusTextBlock.Foreground = Brushes.Green;
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both a username and a password.", "Input Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                int userId = client.RegisterUser(username, password);
                client.Close();

                _currentUserId = userId;

                // Unlock the interface and clear fields
                ToggleTradingInterface(true);
                LogMessage("Account created successfully!");
                RefreshBalances();
                RefreshHistory();

                MessageBox.Show($"Welcome, {username}!\nYour account ID is: {userId}", "Registration Confirmed", MessageBoxButton.OK, MessageBoxImage.Information);
                UsernameTextBox.Text = string.Empty;
                PasswordBox.Password = string.Empty;
            }
            catch (Exception ex)
            {
                LogMessage("Registration failed.", true);
                MessageBox.Show(ex.Message, "WCF Link Failure", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TopUpButton_Click(object sender, RoutedEventArgs e)
        {
            // Feature 3: Safe Parsing validation instead of unhandled app crashes
            if (!decimal.TryParse(TopUpTextBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            {
                LogMessage("Error: Invalid deposit amount entry.", true);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                client.TopUpPln(_currentUserId.Value, amount);
                client.Close();

                LogMessage($"Deposited {amount:N2} PLN cleanly.");
                RefreshBalances();
                RefreshHistory();
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message, true);
            }
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            string currency = TradeCurrencyTextBox.Text.Trim().ToUpper();
            if (!decimal.TryParse(TradeAmountTextBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            {
                LogMessage("Error: Invalid transaction balance volume.", true);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                client.BuyCurrency(_currentUserId.Value, currency, amount);
                client.Close();

                LogMessage($"Successfully purchased {amount} {currency}!");
                RefreshBalances();
                RefreshHistory();
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message, true);
            }
        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            string currency = TradeCurrencyTextBox.Text.Trim().ToUpper();
            if (!decimal.TryParse(TradeAmountTextBox.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount) || amount <= 0)
            {
                LogMessage("Error: Invalid transaction volume entry.", true);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                client.SellCurrency(_currentUserId.Value, currency, amount);
                client.Close();

                LogMessage($"Successfully sold {amount} {currency}!");
                RefreshBalances();
                RefreshHistory();
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message, true);
            }
        }

        private void RefreshBalances()
        {
            if (_currentUserId == null) return;

            try
            {
                var client = new ExchangeOfficeServiceClient();
                decimal pln = client.GetBalance(_currentUserId.Value, "PLN");
                decimal usd = client.GetBalance(_currentUserId.Value, "USD");
                decimal eur = client.GetBalance(_currentUserId.Value, "EUR");
                client.Close();

                // Feature 4: Format balances cleanly onto always-visible side tracker panel
                BalancesTextBlock.Text = $"PLN: {pln:N2} | USD: {usd:N2} | EUR: {eur:N2}";
            }
            catch
            {
                BalancesTextBlock.Text = "Ledger Connection Offline";
                LogMessage("Could not sync ledger balances.", true);
            }
        }

        private void RefreshHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshHistory();
        }

        private void RefreshHistory()
        {
            if (_currentUserId == null)
            {
                HistoryDataGrid.ItemsSource = null;
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                var history = client.GetTransactionHistory(_currentUserId.Value);
                client.Close();

                HistoryDataGrid.ItemsSource = history ?? Array.Empty<TransactionDto>();
                LogMessage("Transaction history updated.");
            }
            catch (Exception ex)
            {
                HistoryDataGrid.ItemsSource = null;
                LogMessage("Could not load transaction history: " + ex.Message, true);
            }
        }

        private void GetRateButton_Click(object sender, RoutedEventArgs e)
        {
            string code = CurrencyTextBox.Text.Trim().ToUpper();
            HistoricalCurrencyTextBox.Text = code;
            if (string.IsNullOrWhiteSpace(code)) return;

            try
            {
                var client = new ExchangeOfficeServiceClient();
                decimal rate = client.GetCurrentRate(code);
                client.Close();

                RateTextBlock.Text = $"1 {code} = {rate:N4} PLN";
                LogMessage($"Fetched current {code} pricing rate successfully.");
            }
            catch (Exception ex)
            {
                RateTextBlock.Text = "Rate Unavailable";
                LogMessage(ex.Message, true);
            }
        }

        private void GetHistoricalRatesButton_Click(object sender, RoutedEventArgs e)
        {
            string code = HistoricalCurrencyTextBox.Text.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(code))
            {
                LogMessage("Enter a currency code for historical rates.", true);
                return;
            }

            if (!HistoryFromDatePicker.SelectedDate.HasValue || !HistoryToDatePicker.SelectedDate.HasValue)
            {
                LogMessage("Select both from and to dates.", true);
                return;
            }

            DateTime from = HistoryFromDatePicker.SelectedDate.Value.Date;
            DateTime to = HistoryToDatePicker.SelectedDate.Value.Date;
            if (from > to)
            {
                LogMessage("'From' date cannot be after 'To' date.", true);
                return;
            }

            try
            {
                var client = new ExchangeOfficeServiceClient();
                var rates = client.GetHistoricalRates(code, from, to);
                client.Close();

                HistoricalRatesDataGrid.ItemsSource = rates ?? Array.Empty<RatePointDto>();
                LogMessage($"Loaded {rates?.Length ?? 0} historical rate points for {code}.");
            }
            catch (Exception ex)
            {
                HistoricalRatesDataGrid.ItemsSource = null;
                LogMessage("Historical rates failed: " + ex.Message, true);
            }
        }
    }
}