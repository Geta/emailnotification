using RestSharp;

namespace Geta.EmailNotification.MailGun
{
    public interface IMailGunMessageFactory
    {
        RestRequest Create(EmailNotificationRequest request);
    }
}