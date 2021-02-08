using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using RestSharp;
using RestSharp.Authenticators;

namespace Geta.EmailNotification.MailGun
{   
    public class MailGunEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MailGunEmailNotificationClient));
        private readonly MailGunCredentials _mailGunCredentials;
        private readonly IRestClient _restClient;

        public MailGunEmailNotificationClient(MailGunCredentials mailGunCredentials, IRestClient restClient)
        {
            _mailGunCredentials = mailGunCredentials;
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
                var request = BuildRequest(emailNotificationRequest);

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
                var request = BuildRequest(emailNotificationRequest);

                var response = await _restClient.ExecuteAsync(request);
                return new EmailNotificationResponse
                {
                    IsSent = true,
                    Message = response.Content
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Email failed to: {emailNotificationRequest.From.Address}. Subject: {emailNotificationRequest.Subject}.", ex);

                return new EmailNotificationResponse
                {
                    Message = ex.Message
                };
            }
        }
        
        private static RestRequest BuildRequest(EmailNotificationRequest request)
        {
            var restRequest = new RestRequest {Resource = "messages"};
            restRequest.AddParameter("from", request.From.Address);
            restRequest.AddParameter("subject", request.Subject);

            if (!string.IsNullOrEmpty(request.Body))
            {
                restRequest.AddParameter("text", request.Body);
            }

            if (request.HtmlBody != null)
            {
                restRequest.AddParameter("html", request.HtmlBody.ToString());
            }
            
            foreach (var mail in request.To)
            {
                restRequest.AddParameter("to", mail.Address);
            }
            
            foreach (var mail in request.Cc)
            {
                restRequest.AddParameter("cc", mail.Address);
            }
            
            foreach (var mail in request.Bcc)
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