namespace Geta.EmailNotification.Common
{
    public interface IEmailNotificationClient
    {
        EmailNotificationResponse Send(EmailNotificationRequestBase emailNotificationRequest);
    }
}