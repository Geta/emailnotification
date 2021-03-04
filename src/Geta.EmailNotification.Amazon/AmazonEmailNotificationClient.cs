﻿using System;
using System.IO;
using System.Linq;
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
        private readonly IAmazonMessageFactory _amazonMessageFactory;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(AmazonEmailNotificationClient));

        public AmazonEmailNotificationClient(IAmazonSimpleEmailService simpleEmailServiceClient,
            IAmazonMessageFactory amazonMessageFactory)
        {
            _simpleEmailServiceClient = simpleEmailServiceClient;
            _amazonMessageFactory = amazonMessageFactory;
        }

        public EmailNotificationResponse Send(EmailNotificationRequest request)
        {
            try
            {
                var amazonRequest = _amazonMessageFactory.Create(request);
                var response = AsyncHelper.RunSync(() => _simpleEmailServiceClient.SendRawEmailAsync(amazonRequest));

                return new EmailNotificationResponse
                {
                    IsSent = response.HttpStatusCode == HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                var emails = request.To?.Select(s => s?.Address);
                var emailsSerialized = emails != null ? string.Join(", ", emails) : string.Empty;
                Log.Error($"Email failed to: {emailsSerialized}. Subject: {request.Subject}.", ex);
                
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
                var amazonRequest = _amazonMessageFactory.Create(request);
                var response = await _simpleEmailServiceClient.SendRawEmailAsync(amazonRequest)
                    .ConfigureAwait(false);

                return new EmailNotificationResponse
                {
                    IsSent = response.HttpStatusCode == HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                var emails = request.To?.Select(s => s?.Address);
                var emailsSerialized = emails != null ? string.Join(", ", emails) : string.Empty;
                Log.Error($"Email failed to: {emailsSerialized}. Subject: {request.Subject}.", ex);

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
            var builder = new BodyBuilder
            {
                TextBody = request.Body, 
                HtmlBody = request.HtmlBody
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
                
                return new SendRawEmailRequest
                {
                    RawMessage = new RawMessage { Data = messageStream }
                };
            }
        }
    }
}