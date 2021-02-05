using System;
using System.Globalization;
using System.Threading.Tasks;
using PostmarkDotNet;

namespace Geta.EmailNotification.Postmark
{
    public class PostmarkEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly PostmarkClient _postmarkClient;
        private readonly IPostmarkMessageFactory _postmarkMessageFactory;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(PostmarkEmailNotificationClient));
        
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
                Log.Error($"Email failed to: {request.To}. Subject: {request.Subject}.", ex);

                return new EmailNotificationResponse
                {
                    Message = ex.Message
                };
            }
        }
    }
}