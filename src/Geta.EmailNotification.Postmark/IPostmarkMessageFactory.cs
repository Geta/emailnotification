using Geta.EmailNotification.Common;
using PostmarkDotNet;

namespace Geta.EmailNotification.Postmark
{
    public interface IPostmarkMessageFactory
    {
        PostmarkMessage Create(EmailNotificationRequestBase request);
    }
}