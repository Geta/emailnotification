using Amazon.SimpleEmail.Model;
using Geta.EmailNotification.Common;

namespace Geta.EmailNotification.Amazon
{
    public interface IAmazonMessageFactory
    {
        SendRawEmailRequest Create(EmailNotificationRequestBase request);
    }
}