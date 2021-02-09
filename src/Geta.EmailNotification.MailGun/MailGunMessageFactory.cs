using Geta.EmailNotification.MailGun.Extensions;
using RestSharp;

namespace Geta.EmailNotification.MailGun
{
    public class MailGunMessageFactory : IMailGunMessageFactory
    {
        private readonly IMailMessageFactory _mailMessageFactory;

        public MailGunMessageFactory(IMailMessageFactory mailMessageFactory)
        {
            _mailMessageFactory = mailMessageFactory;
        }
        
        public RestRequest Create(EmailNotificationRequest request)
        {
            return _mailMessageFactory.Create(request).ToRestRequest();
        }
    }
}