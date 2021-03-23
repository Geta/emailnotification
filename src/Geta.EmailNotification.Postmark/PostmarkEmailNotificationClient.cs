using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Geta.EmailNotification.Common;
using PostmarkDotNet;

namespace Geta.EmailNotification.Postmark
{
    public class PostmarkEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly PostmarkClient _postmarkClient;
        private readonly IPostmarkMessageFactory _postmarkMessageFactory;

        public PostmarkEmailNotificationClient(
            PostmarkClient postmarkClient, IPostmarkMessageFactory postmarkMessageFactory)
        {
            _postmarkClient = postmarkClient;
            _postmarkMessageFactory = postmarkMessageFactory;
        }

        public EmailNotificationResponse Send(EmailNotificationRequest request)
        {
            try
            {
                var message = _postmarkMessageFactory.Create(request);

                var response = AsyncHelper.RunSync(() => _postmarkClient.SendMessageAsync(message));

                return new EmailNotificationResponse
                {
                    IsSent = response.Status == PostmarkStatus.Success,
                    Message = response.ErrorCode.ToString(CultureInfo.InvariantCulture)
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
                var message = _postmarkMessageFactory.Create(request);

                var response = await _postmarkClient.SendMessageAsync(message).ConfigureAwait(false);

                return new EmailNotificationResponse
                {
                    IsSent = response.Status == PostmarkStatus.Success,
                    Message = response.ErrorCode.ToString(CultureInfo.InvariantCulture)
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