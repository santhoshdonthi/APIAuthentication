namespace APIAuthentication.Models
{
    public class Auth
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
