using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace Geta.EmailNotification.Amazon
{
    /// <summary>
    /// TODO consider this wrapper for Amazon SES instead: http://www.nuget.org/packages/MailChimpAmazonSES/
    /// TODO Add support for attachments
    /// </summary>
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

                var response = _simpleEmailServiceClient.SendEmail(amazonRequest);

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

                var response = await _simpleEmailServiceClient.SendEmailAsync(amazonRequest).ConfigureAwait(false);

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

        private static SendEmailRequest CreateRequest(EmailNotificationRequest request)
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

            var destination = new Destination();

            foreach (var mailAddress in request.To)
            {
                destination.ToAddresses.Add(mailAddress.Name);
            }

            foreach (var mailAddress in request.Cc)
            {
                destination.CcAddresses.Add(mailAddress.Name);
            }

            foreach (var mailAddress in request.Bcc)
            {
                destination.BccAddresses.Add(mailAddress.Name);
            }

            return new SendEmailRequest
            {
                Destination = destination,
                ReplyToAddresses = new List<string> {request.From.Address},
                Message = new Message(
                    new Content(request.Subject),
                    new Body(new Content(request.Body)))
            };
        }
    }
}