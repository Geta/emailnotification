using Geta.EmailNotification.Common;
using RestSharp;

namespace Geta.EmailNotification.MailGun
{
    public interface IMailGunMessageFactory
    {
        RestRequest Create(EmailNotificationRequestBase request);
    }
}