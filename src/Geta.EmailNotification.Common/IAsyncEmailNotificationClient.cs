using System.Threading.Tasks;

namespace Geta.EmailNotification.Common
{
    public interface IAsyncEmailNotificationClient
    {
        /// <summary>
        /// Sends email asynchronously
        /// </summary>
        /// <param name="request">Email request data</param>
        /// <returns>Email notification response data</returns>
        Task<EmailNotificationResponse> SendAsync(EmailNotificationRequestBase request);
    }
}