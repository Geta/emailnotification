using SendGrid;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Geta.EmailNotification.Common;

namespace Geta.EmailNotification.SendGrid
{
    public class SendGridEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly SendGridMessageFactory _mailMessageFactory;

        public SendGridEmailNotificationClient(ISendGridClient sendGridClient, SendGridMessageFactory mailMessageFactory)
        {
            _sendGridClient = sendGridClient;
            _mailMessageFactory = mailMessageFactory;
        }

        public async Task<EmailNotificationResponse> SendAsync(EmailNotificationRequestBase request)
        {
            try
            {
                var message = _mailMessageFactory.CreateSendGridMessage(request);
                var response = await _sendGridClient.SendEmailAsync(message);

                return new EmailNotificationResponse
                {
                    IsSent = response.StatusCode == HttpStatusCode.Accepted,
                    Message = await response.Body.ReadAsStringAsync()
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

        public EmailNotificationResponse Send(EmailNotificationRequestBase request)
        {
            try
            {
                var message = _mailMessageFactory.CreateSendGridMessage(request);

                var response = AsyncHelper.RunSync(() => _sendGridClient.SendEmailAsync(message));

                return new EmailNotificationResponse
                {
                    IsSent = response.StatusCode == HttpStatusCode.Accepted,
                    Message = response.Body.ReadAsStringAsync().Result
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