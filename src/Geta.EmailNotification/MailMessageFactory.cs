using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;

namespace Geta.EmailNotification
{
    public class MailMessageFactory : IMailMessageFactory
    {
        private readonly IEmailViewRenderer _renderer;

        public MailMessageFactory(IEmailViewRenderer renderer)
        {
            _renderer = renderer;
        }

        public MimeMessage Create(EmailNotificationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "EmailNotificationRequest cannot be null");
            }

            if (request.To == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(request)}.{nameof(request.To)}", "To email address cannot be null");
            }

            if (request.From == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(request)}.{nameof(request.From)}", "From email address cannot be null");
            }

            var mail = new MimeMessage();
            mail.From.Add(request.From);
            mail.Subject = request.Subject;
          
            CopyAddress(request.To, mail.To);
            CopyAddress(request.Cc, mail.Cc);
            CopyAddress(request.Bcc, mail.Bcc);
            CopyAddress(request.ReplyTo, mail.ReplyTo);
            var body = CreateBody(request, out var isHtml);
            
            var builder = new BodyBuilder();
            if (isHtml)
            {
                builder.HtmlBody = body;
            }
            else
            {
                builder.TextBody = body;
            }

            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var attachment in request.Attachments)
                {
                    builder.Attachments.Add(attachment);
                } 
            }

            mail.Body = builder.ToMessageBody();
            return mail;
        }

        private static void CopyAddress(IEnumerable<MailboxAddress> from, InternetAddressList to)
        {
            foreach (var mailAddress in from)
            {
                to.Add(mailAddress);
            }
        }

        private string CreateBody(EmailNotificationRequest request, out bool isHtml)
        {
            isHtml = true;
            if (!string.IsNullOrWhiteSpace(request.ViewName))
            {
                return AsyncHelper.RunSync(() => _renderer.RenderAsync(request, request.ViewName));
            }

            if (!string.IsNullOrEmpty(request.HtmlBody?.ToString()))
            {
                return request.HtmlBody.ToString();
            }

            isHtml = false;
            return request.Body;
        }
    }
}