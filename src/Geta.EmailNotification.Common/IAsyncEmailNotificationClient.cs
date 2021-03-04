using System.Threading.Tasks;

namespace Geta.EmailNotification.Common
{
    public interface IAsyncEmailNotificationClient
    {
        Task<EmailNotificationResponse> SendAsync(EmailNotificationRequestBase request);
    }
}