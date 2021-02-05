namespace Geta.EmailNotification
{
    public interface IEmailNotificationClient
    {
        EmailNotificationResponse Send(EmailNotificationRequest emailNotificationRequest);
    }
}