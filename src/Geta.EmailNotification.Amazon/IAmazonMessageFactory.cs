using Amazon.SimpleEmail.Model;

namespace Geta.EmailNotification.Amazon
{
    public interface IAmazonMessageFactory
    {
        SendRawEmailRequest Create(EmailNotificationRequest request);
    }
}