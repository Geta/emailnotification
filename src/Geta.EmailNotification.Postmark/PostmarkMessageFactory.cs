using Geta.EmailNotification.Common;
using Geta.EmailNotification.Postmark.Extensions;
using PostmarkDotNet;

namespace Geta.EmailNotification.Postmark
{
    public class PostmarkMessageFactory : IPostmarkMessageFactory
    {
        private readonly IMailMessageFactory _mailMessageFactory;

        public PostmarkMessageFactory(IMailMessageFactory mailMessageFactory)
        {
            _mailMessageFactory = mailMessageFactory;
        }

        public PostmarkMessage Create(EmailNotificationRequestBase request)
        {
            return _mailMessageFactory.Create(request).ToPostmarkMessage();
        }
    }
}