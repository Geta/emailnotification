using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geta.EmailNotification.Common;
using MimeKit;

namespace Geta.EmailNotification
{
    public class WhitelistAsyncEmailNotificationClientDecorator : IAsyncEmailNotificationClient
    {
        private readonly IAsyncEmailNotificationClient _emailClient;
        private readonly WhitelistConfiguration _whitelistConfiguration;

        public WhitelistAsyncEmailNotificationClientDecorator(
            IAsyncEmailNotificationClient emailClient,
            WhitelistConfiguration whitelistConfiguration)
        {
            _emailClient = emailClient;
            _whitelistConfiguration = whitelistConfiguration;
        }

        public async Task<EmailNotificationResponse> SendAsync(EmailNotificationRequestBase emailNotificationRequest)
        {
            if (emailNotificationRequest == null)
            {
                throw new ArgumentNullException(nameof(emailNotificationRequest), "EmailNotificationRequest cannot be null");
            }

            if (!_whitelistConfiguration.HasWhitelist)
            {
                return await _emailClient.SendAsync(emailNotificationRequest);
            }

            emailNotificationRequest.To = WhiteList(emailNotificationRequest.To);
            emailNotificationRequest.Cc = WhiteList(emailNotificationRequest.Cc);
            emailNotificationRequest.Bcc = WhiteList(emailNotificationRequest.Bcc);

            return await _emailClient.SendAsync(emailNotificationRequest);
        }

        private List<MailboxAddress> WhiteList(IEnumerable<MailboxAddress> addressCollection)
        {
            var result = new List<MailboxAddress>();
            foreach (var address in addressCollection)
            {
                if (InWhitelist(address.Address))
                {
                    result.Add(address);
                }
            }
            return result;
        }

        private bool InWhitelist(string address)
        {
            return _whitelistConfiguration.Emails.Any(address.Equals)
                || _whitelistConfiguration.Domains.Any(address.EndsWith);
        }
    }
}