using System.IO;
using System.Linq;
using MimeKit;
using RestSharp;

namespace Geta.EmailNotification.MailGun.Extensions
{
    public static class MailGunExtensions
    {
        public static RestRequest ToRestRequest(this MimeMessage request)
        {
            var restRequest = new RestRequest {Resource = "messages"};
            restRequest.AddParameter("from", request.From.Mailboxes.First().Address);
            restRequest.AddParameter("subject", request.Subject);

            if (!string.IsNullOrEmpty(request.TextBody))
            {
                restRequest.AddParameter("text", request.Body);
            }

            if (!string.IsNullOrEmpty(request.HtmlBody))
            {
                restRequest.AddParameter("html", request.HtmlBody);
            }
            
            foreach (var mail in request.To.Mailboxes)
            {
                restRequest.AddParameter("to", mail.Address);
            }
            
            foreach (var mail in request.Cc.Mailboxes)
            {
                restRequest.AddParameter("cc", mail.Address);
            }
            
            foreach (var mail in request.Bcc.Mailboxes)
            {
                restRequest.AddParameter("bcc", mail.Address);
            }

            foreach (var attachment in request.Attachments.OfType<MimePart>())
            {
                using (var stream = new MemoryStream())
                {
                    attachment.Content.Stream.CopyTo(stream);
                    restRequest.AddFile("attachment", stream.ToArray(), attachment.FileName);
                }
            }
            
            restRequest.Method = Method.POST;
            return restRequest;
        }
    }
}