namespace ExchangeOffice.Business
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Plain text for now; we will hash it in Lab 11
    }
}