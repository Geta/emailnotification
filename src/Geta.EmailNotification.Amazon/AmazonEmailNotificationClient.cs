using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;

namespace Geta.EmailNotification.Amazon
{
    public class AmazonEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly IAmazonSimpleEmailService _simpleEmailServiceClient;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AmazonEmailNotificationClient));

        public AmazonEmailNotificationClient(IAmazonSimpleEmailService simpleEmailServiceClient)
        {
            _simpleEmailServiceClient = simpleEmailServiceClient;
        }

        public EmailNotificationResponse Send(EmailNotificationRequest request)
        {
            try
            {
                var amazonRequest = CreateRequest(request);

                var response = _simpleEmailServiceClient.SendRawEmail(amazonRequest);

                return new EmailNotificationResponse
                {
                    IsSent = response.HttpStatusCode == HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Email failed to: {request.To}. Subject: {request.Subject}.", ex);

                return new EmailNotificationResponse
                {
                    Message = ex.Message
                };
            }
        }

        public async Task<EmailNotificationResponse> SendAsync(EmailNotificationRequest request)
        {
            try
            {
                var amazonRequest = CreateRequest(request);

                var response = await _simpleEmailServiceClient.SendRawEmailAsync(amazonRequest).ConfigureAwait(false);

                return new EmailNotificationResponse
                {
                    IsSent = response.HttpStatusCode == HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Email failed to: {request.To}. Subject: {request.Subject}.", ex);

                return new EmailNotificationResponse
                {
                    Message = ex.Message
                };
            }
        }

        private static SendRawEmailRequest CreateRequest(EmailNotificationRequest request)
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

            var message = new MimeMessage();
            message.From.Add(request.From);
            message.Subject = request.Subject;
            var builder = new BodyBuilder()
            {
                TextBody = request.Body, 
                HtmlBody = request.HtmlBody?.ToHtmlString()
            };

            
            foreach (var mailAddress in request.To)
            {
                message.To.Add(mailAddress);
            }

            foreach (var mailAddress in request.Cc)
            {
                message.Cc.Add(mailAddress);
            }

            foreach (var mailAddress in request.Bcc)
            {
                message.Bcc.Add(mailAddress);
            }

            foreach (var attachment in request.Attachments)
            {
                builder.Attachments.Add(attachment);
            }

            using (var messageStream = new MemoryStream())
            {
                message.Body = builder.ToMessageBody();
                message.WriteTo(messageStream);
                
                return new SendRawEmailRequest()
                {
                    RawMessage = new RawMessage() { Data = messageStream }
                };
            }
        }
    }
}