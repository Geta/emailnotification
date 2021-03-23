using MimeKit;

namespace Geta.EmailNotification.Common
{
    /// <summary>
    /// Creates MIME massage
    /// </summary>
    public interface IMailMessageFactory
    {
        /// <summary>
        /// Creates MIME massage from EmailNotificationRequest.
        /// </summary>
        /// <param name="request">Email request data.</param>
        /// <returns>MIME message object from MimeKit package.</returns>
        MimeMessage Create(EmailNotificationRequest request);
    }
}