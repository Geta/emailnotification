using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Geta.EmailNotification.Common
{
    /// <summary>
    /// Sends an using SMTP (MailKit.Net.Smtp).
    /// </summary>
    public class SmtpEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly IMailMessageFactory _mailMessageFactory;
        private readonly SmtpConfiguration _smtpConfiguration;

        public SmtpEmailNotificationClient(IMailMessageFactory mailMessageFactory, SmtpConfiguration smtpConfiguration)
        {
            _mailMessageFactory = mailMessageFactory;
            _smtpConfiguration = smtpConfiguration;
        }

        /// <summary>
        /// Sends email synchronously
        /// </summary>
        /// <param name="request">Email request data</param>
        /// <returns>Email notification response data</returns>
        public EmailNotificationResponse Send(EmailNotificationRequest request)
        {
            var response = new EmailNotificationResponse();

            try
            {
                var mail = _mailMessageFactory.Create(request);
                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.UseSsl);
                    if (_smtpConfiguration.UseAuthentication)
                    {
                        client.Authenticate(_smtpConfiguration.Username, _smtpConfiguration.Password);
                    }
                    client.Send(mail);
                    client.Disconnect (true);
                }

                response.IsSent = true;
            }
            catch (Exception e)
            {
                var emails = request?.To.Select(x => x.Address) ?? new List<string>();
                var emailsSerialized = string.Join(",", emails);
                response.Message = $"Email failed to: {emailsSerialized}. Subject: {request?.Subject} Error {e.Message}.";
            }

            return response;
        }

        /// <summary>
        /// Sends email asynchronously
        /// </summary>
        /// <param name="request">Email request data</param>
        /// <returns>Email notification response data</returns>
        public async Task<EmailNotificationResponse> SendAsync(EmailNotificationRequest request)
        {
            var response = new EmailNotificationResponse();

            try
            {
                var mail = _mailMessageFactory.Create(request);
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpConfiguration.Host, _smtpConfiguration.Port, _smtpConfiguration.UseSsl);
                    if (_smtpConfiguration.UseAuthentication)
                    {
                        await client.AuthenticateAsync(_smtpConfiguration.Username, _smtpConfiguration.Password);
                    }
                    await client.SendAsync(mail).ConfigureAwait(false);
                    await client.DisconnectAsync(true);
                }

                response.IsSent = true;
            }
            catch (Exception e)
            {
                var emails = request?.To.Select(x => x.Address) ?? new List<string>();
                var emailsSerialized = string.Join(",", emails);
                response.Message = $"Email failed to: {emailsSerialized}. Subject: {request?.Subject} Error {e.Message}.";
            }

            return response;
        }
    }
}