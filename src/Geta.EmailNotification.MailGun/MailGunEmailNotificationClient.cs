using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace Geta.EmailNotification.MailGun
{   
    public class MailGunEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MailGunEmailNotificationClient));
        private readonly MailGunCredentials _mailGunCredentials;
        private readonly IMailGunMessageFactory _mailGunMessageFactory;
        private readonly IRestClient _restClient;

        public MailGunEmailNotificationClient(MailGunCredentials mailGunCredentials, 
            IMailGunMessageFactory mailGunMessageFactory,
            IRestClient restClient)
        {
            _mailGunCredentials = mailGunCredentials;
            _mailGunMessageFactory = mailGunMessageFactory;
            _restClient = restClient;
        }

        public EmailNotificationResponse Send(EmailNotificationRequest emailNotificationRequest)
        {
            try
            {
                _restClient.BaseUrl = new Uri (_mailGunCredentials.ApiBaseUrl);
                _restClient.Authenticator =
                    new HttpBasicAuthenticator ("api",
                    _mailGunCredentials.ApiKey);
                var request = _mailGunMessageFactory.Create(emailNotificationRequest);

                var response = _restClient.Execute(request);
                return new EmailNotificationResponse
                {
                    IsSent = true,
                    Message = response.Content
                };
            }
            catch (Exception ex)
            {
                var emails = emailNotificationRequest.To?.Select(s => s?.Address);
                var emailsSerialized = emails != null ? string.Join(", ", emails) : string.Empty;
                Log.Error($"Email failed to: {emailsSerialized}. Subject: {emailNotificationRequest.Subject}.", ex);

                return new EmailNotificationResponse
                {
                    Message = ex.Message
                };
            }
        }
        public async Task<EmailNotificationResponse> SendAsync(EmailNotificationRequest emailNotificationRequest)
        {
            try
            {
                _restClient.BaseUrl = new Uri (_mailGunCredentials.ApiBaseUrl);
                _restClient.Authenticator =
                    new HttpBasicAuthenticator ("api",
                        _mailGunCredentials.ApiKey);
                var request = _mailGunMessageFactory.Create(emailNotificationRequest);

                var response = await _restClient.ExecuteAsync(request);
                return new EmailNotificationResponse
                {
                    IsSent = response.IsSuccessful,
                    Message = response.Content
                };
            }
            catch (Exception ex)
            {
                var emails = emailNotificationRequest.To?.Select(s => s?.Address);
                var emailsSerialized = emails != null ? string.Join(", ", emails) : string.Empty;
                Log.Error($"Email failed to: {emailsSerialized}. Subject: {emailNotificationRequest.Subject}.", ex);

                return new EmailNotificationResponse
                {
                    Message = ex.Message
                };
            }
        }
    }
}