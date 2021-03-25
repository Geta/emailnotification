using System;
using System.Collections.Generic;
using System.Linq;
using Geta.EmailNotification.Common;
using MimeKit;

namespace Geta.EmailNotification
{
    public class WhitelistEmailNotificationClientDecorator : IEmailNotificationClient
    {
        private readonly IEmailNotificationClient _emailClient;
        private readonly WhitelistConfiguration _whitelistConfiguration;

        public WhitelistEmailNotificationClientDecorator(
            IEmailNotificationClient emailClient,
            WhitelistConfiguration whitelistConfiguration)
        {
            _emailClient = emailClient;
            _whitelistConfiguration = whitelistConfiguration;
        }

        public EmailNotificationResponse Send(EmailNotificationRequest emailNotificationRequest)
        {
            if (emailNotificationRequest == null)
            {
                throw new ArgumentNullException(nameof(emailNotificationRequest), "EmailNotificationRequest cannot be null");
            }

            if (!_whitelistConfiguration.HasWhitelist)
            {
                return _emailClient.Send(emailNotificationRequest);
            }

            emailNotificationRequest.To = WhiteList(emailNotificationRequest.To);
            emailNotificationRequest.Cc = WhiteList(emailNotificationRequest.Cc);
            emailNotificationRequest.Bcc = WhiteList(emailNotificationRequest.Bcc);

            return _emailClient.Send(emailNotificationRequest);
        }

        private List<MailboxAddress> WhiteList(IEnumerable<MailboxAddress> addressCollection)
        {
            var result = new List<MailboxAddress>();
            foreach (var address in addressCollection)
            {
                if (InWhitelist(address.Name))
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