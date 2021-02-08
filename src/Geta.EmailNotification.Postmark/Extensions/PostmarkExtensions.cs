using System.Linq;
using System.Net.Mail;
using MimeKit;
using PostmarkDotNet;

namespace Geta.EmailNotification.Postmark.Extensions
{
    public static class PostmarkExtensions
    {
        public static PostmarkMessage ToPostmarkMessage(this MimeMessage mailMessage)
        {
            var postmarkMessage = new PostmarkMessage
            {
                From = mailMessage.From.Mailboxes.First().Address,
                ReplyTo = mailMessage.ReplyTo.Mailboxes.FirstOrDefault()?.Address,
                To = string.Join(",", mailMessage.To.Mailboxes.Select(to => to.Address)),
                Cc = string.Join(",", mailMessage.Cc.Mailboxes.Select(cc => cc.Address)),
                Bcc = string.Join(",", mailMessage.Bcc.Mailboxes.Select(bcc => bcc.Address)),
                Subject = mailMessage.Subject,
                TextBody = !string.IsNullOrEmpty(mailMessage.TextBody)
                    ? mailMessage.TextBody
                    : null,
                HtmlBody = !string.IsNullOrEmpty(mailMessage.HtmlBody)
                    ? mailMessage.HtmlBody
                    : null
            };

            foreach (var mailAttachment in mailMessage.Attachments.OfType<MimePart>())
            {
                postmarkMessage.AddAttachment(mailAttachment.Content.Stream, mailAttachment.FileName, mailAttachment.ContentType.MediaType, mailAttachment.ContentId);
            }

            return postmarkMessage;
        }
    }
}