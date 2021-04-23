namespace Geta.EmailNotification.Common
{
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseAuthentication => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
    }
}