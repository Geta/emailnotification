using MimeKit;

namespace Geta.EmailNotification.Common
{
    public interface IMailMessageFactory
    {
        MimeMessage Create(EmailNotificationRequestBase request);
    }
}