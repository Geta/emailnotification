using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MimeKit;
using SendGrid.Helpers.Mail;
using Attachment = SendGrid.Helpers.Mail.Attachment;

namespace Geta.EmailNotification.SendGrid
{
    public static class MailMessageExtensions
    {
        public static EmailAddress GetSendGridAddress(this MailboxAddress mailbox)
        {
            return String.IsNullOrWhiteSpace(mailbox.Name) ?
                new EmailAddress(mailbox.Address) :
                new EmailAddress(mailbox.Address, mailbox.Name.Replace(",", "").Replace(";", ""));
        }

        public static Attachment ConvertToSendGridAttachment(this MimePart attachment)
        {
            using (var stream = new MemoryStream())
            {
               attachment.Content.Stream.CopyTo(stream);
               return new Attachment()
                {
                    Disposition = "attachment",
                    Type = attachment.ContentType.MediaType,
                    Filename = attachment.FileName,
                    ContentId = attachment.ContentId,
                    Content = Convert.ToBase64String(stream.ToArray())
                };
            }
        }

        public static SendGridMessage ConvertToSendGridMessage(this MimeMessage message)
        {
            var sendgridMessage = new SendGridMessage();

            sendgridMessage.From = GetSendGridAddress(message.From.Mailboxes.First());

            if (message.ReplyTo.Any())
            {
                sendgridMessage.ReplyTo = message.ReplyTo.Mailboxes.First().GetSendGridAddress();
            }

            if (message.To.Any())
            {
                var tos = message.To.Mailboxes.Select(x => x.GetSendGridAddress()).ToList();
                sendgridMessage.AddTos(tos);
            }

            if (message.Cc.Any())
            {
                var cc = message.Cc.Mailboxes.Select(x => x.GetSendGridAddress()).ToList();
                sendgridMessage.AddCcs(cc);
            }

            if (message.Bcc.Any())
            {
                var bcc = message.Bcc.Mailboxes.Select(x => x.GetSendGridAddress()).ToList();
                sendgridMessage.AddBccs(bcc);
            }

            if (!string.IsNullOrWhiteSpace(message.Subject))
            {
                sendgridMessage.Subject = message.Subject;
            }
            
            if (message.HtmlBody != null)
            {
                string content = message.HtmlBody;
                content = content.Replace("\r", string.Empty).Replace("\n", string.Empty);
                if (content.StartsWith("<html"))
                {
                    content = message.HtmlBody;
                }
                else
                {
                    content = $"<html><body>{message.HtmlBody}</body></html>";
                }

                sendgridMessage.AddContent("text/html", content);
            }

            if (!string.IsNullOrWhiteSpace(message.TextBody))
            {
                sendgridMessage.AddContent("text/plain", message.Body.ToString());
            }

            if (message.Attachments.Any())
            {
                sendgridMessage.Attachments = new List<Attachment>();
                sendgridMessage.Attachments.AddRange(message.Attachments
                    .OfType<MimePart>().Select(ConvertToSendGridAttachment));
            }

            return sendgridMessage;
        }
    }
}