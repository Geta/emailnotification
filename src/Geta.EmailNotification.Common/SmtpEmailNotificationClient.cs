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

        public SmtpEmailNotificationClient(IMailMessageFactory mailMessageFactory)
        {
            _mailMessageFactory = mailMessageFactory;
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
                    client.Send(mail);
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
                    await client.SendAsync(mail).ConfigureAwait(false);
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