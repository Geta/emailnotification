using Amazon.SimpleEmail.Model;
using Geta.EmailNotification.Amazon.Extensions;
using Geta.EmailNotification.Common;

namespace Geta.EmailNotification.Amazon
{
    public class AmazonMessageFactory : IAmazonMessageFactory
    {
        private readonly IMailMessageFactory _mailMessageFactory;

        public AmazonMessageFactory(IMailMessageFactory mailMessageFactory)
        {
            _mailMessageFactory = mailMessageFactory;
        }
        
        public SendRawEmailRequest Create(EmailNotificationRequestBase request)
        {
            return _mailMessageFactory.Create(request).ToAmazonMessage();
        }
    }
}