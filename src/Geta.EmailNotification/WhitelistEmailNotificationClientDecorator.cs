using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

        public EmailNotificationResponse Send(EmailNotificationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "EmailNotificationRequest cannot be null");
            }

            if (!_whitelistConfiguration.HasWhitelist)
            {
                return _emailClient.Send(request);
            }

            request.To = WhiteList(request.To);
            request.Cc = WhiteList(request.Cc);
            request.Bcc = WhiteList(request.Bcc);

            return _emailClient.Send(request);
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