using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Geta.EmailNotification.Common;

namespace Geta.EmailNotification.Amazon
{
    public class AmazonEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly IAmazonSimpleEmailService _simpleEmailServiceClient;
        private readonly IAmazonMessageFactory _amazonMessageFactory;

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

                return new EmailNotificationResponse
                {
                    Message = $"Email failed to: {emailsSerialized}. Subject: {request.Subject} " +
                              $"Error: {ex.Message}. Stacktrace: {ex.StackTrace}"
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

                return new EmailNotificationResponse
                {
                    Message = $"Email failed to: {emailsSerialized}. Subject: {request.Subject} " +
                              $"Error: {ex.Message}. Stacktrace: {ex.StackTrace}"
                };
            }
        }
    }
}