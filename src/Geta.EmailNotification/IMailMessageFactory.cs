using MimeKit;

namespace Geta.EmailNotification
{
    public interface IMailMessageFactory
    {
        MimeMessage Create(EmailNotificationRequest request);
    }
}