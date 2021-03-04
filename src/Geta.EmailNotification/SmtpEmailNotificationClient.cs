using System;
using System.Threading.Tasks;
using Geta.EmailNotification.Common;
using MailKit.Net.Smtp;

namespace Geta.EmailNotification
{
    public class SmtpEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly IMailMessageFactory _mailMessageFactory;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SmtpEmailNotificationClient));

        public SmtpEmailNotificationClient(IMailMessageFactory mailMessageFactory)
        {
            _mailMessageFactory = mailMessageFactory;
        }

        public EmailNotificationResponse Send(EmailNotificationRequestBase emailNotificationRequest)
        {
            var response = new EmailNotificationResponse();

            try
            {
                var mail = _mailMessageFactory.Create(emailNotificationRequest);
                using (var client = new SmtpClient())
                {
                    client.Send(mail);
                }

                response.IsSent = true;
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                Log.Error($"Email failed to: {emailNotificationRequest.To}. Subject: {emailNotificationRequest.Subject}", e);
            }

            return response;
        }

        public async Task<EmailNotificationResponse> SendAsync(EmailNotificationRequestBase request)
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
                response.Message = e.Message;
                Log.Error($"Email failed to: {request.To}. Subject: {request.Subject}", e);
            }

            return response;
        }
    }
}