namespace Geta.EmailNotification.Common
{
    public interface IEmailNotificationClient
    {
        /// <summary>
        /// Sends email synchronously
        /// </summary>
        /// <param name="request">Email request data</param>
        /// <returns>Email notification response data</returns>
        EmailNotificationResponse Send(EmailNotificationRequestBase request);
    }
}