using Geta.EmailNotification.Common;
using SendGrid.Helpers.Mail;

namespace Geta.EmailNotification.SendGrid
{
    public class SendGridMessageFactory : MailMessageFactory
    {
        public SendGridMessageFactory(IEmailViewRenderer renderer) : base(renderer) { }

        public SendGridMessage CreateSendGridMessage(EmailNotificationRequestBase notification)
        {
            var message = Create(notification);
            var sendGridMessage = message.ConvertToSendGridMessage();

            return sendGridMessage;
        }
    }
}