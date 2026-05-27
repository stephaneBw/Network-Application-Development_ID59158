using System;
using System.Windows;
using System.Windows.Media;
using ExchangeOffice.Client.ExchangeOfficeRef; // Connects to your network channel link

namespace ExchangeOffice.Client
{
    public partial class MainWindow : Window
    {
        // Internal state memory placeholder to track the registered account ID for upcoming transaction labs
        private int? _currentUserId = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Lab 9: Register Button Click Event Handler
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text.Trim();
                string password = PasswordBox.Password; // Grabs string securely from password element

                // Operational input safety checks
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter both a username and a password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 1. Establish pipeline client to the WCF network service
                var client = new ExchangeOfficeServiceClient();

                // 2. Invoke registration on the core business engine memory structure
                int userId = client.RegisterUser(username, password);

                // 3. Close pipeline communication safely
                client.Close();

                // 4. Save the returned incremental ID to application state memory
                _currentUserId = userId;

                // 5. Update UI components state presentation elements
                UserStatusTextBlock.Text = $"Status: Registered! Assigned User ID: {userId}";
                UserStatusTextBlock.Foreground = Brushes.Green;

                MessageBox.Show($"Welcome, {username}!\nYour unique account User ID is: {userId}", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                // Optional cleanup: empty the fields after successful run
                UsernameTextBox.Text = string.Empty;
                PasswordBox.Password = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration pipeline failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Lab 8: Live Rate Lookup handler
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