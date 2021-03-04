using System;
using System.Linq;
using System.Threading.Tasks;
using Geta.EmailNotification.Common;
using RestSharp;
using RestSharp.Authenticators;

namespace Geta.EmailNotification.MailGun
{   
    public class MailGunEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
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

        public EmailNotificationResponse Send(EmailNotificationRequestBase emailNotificationRequest)
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

                return new EmailNotificationResponse
                {
                    Message = $"Email failed to: {emailsSerialized}. Subject: {emailNotificationRequest.Subject} Error {ex.Message}."
                };
            }
        }
        public async Task<EmailNotificationResponse> SendAsync(EmailNotificationRequestBase emailNotificationRequest)
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

                return new EmailNotificationResponse
                {
                    Message = $"Email failed to: {emailsSerialized}. Subject: {emailNotificationRequest.Subject} Error {ex.Message}."
                };
            }
        }
    }
}