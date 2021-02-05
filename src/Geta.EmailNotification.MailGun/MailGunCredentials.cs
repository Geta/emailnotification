namespace Geta.EmailNotification.MailGun
{
    public class MailGunCredentials
    {
        public string ApiKey { get; }
        public string ApiBaseUrl { get; }

        public MailGunCredentials(string apiKey, string apiBaseUrl)
        {
            ApiKey = apiKey;
            ApiBaseUrl = apiBaseUrl;
        }
    }
}