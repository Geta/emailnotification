using System;
using System.Collections.Generic;
using System.Linq;
using Geta.EmailNotification.Common;
using MimeKit;

namespace Geta.EmailNotification
{
    public class EmailNotificationRequestBuilder
    {
        private readonly EmailNotificationRequest _request;

        /// <summary>
        /// Creates new instance of EmailNotificationRequestBuilder.
        /// </summary>
        /// <param name="request">Existing EmailNotificationRequest from which to copy values. Creates empty EmailNotificationRequest if null passed.</param>
        public EmailNotificationRequestBuilder(EmailNotificationRequest request = null)
        {
            _request = request != null ? Clone(request) : new EmailNotificationRequest();
        }

        /// <summary>
        /// Sets Subject.
        /// </summary>
        /// <param name="subject">Email subject.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithSubject(string subject)
        {
            _request.Subject = subject;
            return this;
        }

        /// <summary>
        /// Sets From email address.
        /// </summary>
        /// <param name="email">From email address.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithFrom(string email)
        {
            _request.From = MailboxAddress.Parse(email);
            return this;
        }

        /// <summary>
        /// Sets From email address with display name.
        /// </summary>
        /// <param name="email">From email address.</param>
        /// <param name="displayName">Display name.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithFrom(string email, string displayName)
        {
            _request.From = new MailboxAddress(displayName,email);
            return this;
        }

        /// <summary>
        /// Adds To email address to current To email addresses.
        /// </summary>
        /// <param name="email">To email address.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithTo(string email)
        {
            _request.To.Add(MailboxAddress.Parse(email));
            return this;
        }

        /// <summary>
        /// Adds To email address with display name to current To email addresses.
        /// </summary>
        /// <param name="email">To email address.</param>
        /// <param name="displayName">Display name.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithTo(string email, string displayName)
        {
            _request.To.Add(new MailboxAddress(displayName, email));
            return this;
        }

        /// <summary>
        /// Adds multiple To email addresses from MailAddressCollection to current To email addresses.
        /// </summary>
        /// <param name="toEmails">MailAddressCollection with To addresses.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithTo(IEnumerable<MailboxAddress> toEmails)
        {
            CopyEmails(toEmails, _request.To);
            return this;
        }

        /// <summary>
        /// Adds multiple To email addresses from sequence of Tuple email address and display name to current To email addresses. 
        /// </summary>
        /// <param name="toEmails">Sequence of Tuple where Item1 is To email and Item2 is display name in the Tuple.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithTo(IEnumerable<Tuple<string, string>> toEmails)
        {
            var list = toEmails.ToList();
            list.ForEach(x => _request.To.Add(new MailboxAddress(x.Item2, x.Item1)));
            return this;
        }

        /// <summary>
        /// Adds Cc email address to current Cc email addresses.
        /// </summary>
        /// <param name="email">Cc email address.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithCc(string email)
        {
            _request.Cc.Add(MailboxAddress.Parse(email));
            return this;
        }

        /// <summary>
        /// Adds Cc email address with display name to current Cc email addresses.
        /// </summary>
        /// <param name="email">Cc email address.</param>
        /// <param name="displayName">Display name.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithCc(string email, string displayName)
        {
            _request.Cc.Add(new MailboxAddress(displayName, email));
            return this;
        }

        /// <summary>
        /// Adds multiple Cc email addresses from MailAddressCollection to current Cc email addresses.
        /// </summary>
        /// <param name="toEmails">MailAddressCollection with Cc addresses.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithCc(IEnumerable<MailboxAddress> toEmails)
        {
            CopyEmails(toEmails, _request.Cc);
            return this;
        }

        /// <summary>
        /// Adds multiple Cc email addresses from sequence of Tuple email address and display name to current Cc email addresses. 
        /// </summary>
        /// <param name="toEmails">Sequence of Tuple where Item1 is Cc email and Item2 is display name in the Tuple.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithCc(IEnumerable<Tuple<string, string>> toEmails)
        {
            var list = toEmails.ToList();
            list.ForEach(x => _request.Cc.Add(new MailboxAddress(x.Item2, x.Item1)));
            return this;
        }

        /// <summary>
        /// Adds Bcc email address to current Bcc email addresses.
        /// </summary>
        /// <param name="email">Bcc email address.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithBcc(string email)
        {
            _request.Bcc.Add(MailboxAddress.Parse(email));
            return this;
        }

        /// <summary>
        /// Adds Bcc email address with display name to current Bcc email addresses.
        /// </summary>
        /// <param name="email">Bcc email address.</param>
        /// <param name="displayName">Display name.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithBcc(string email, string displayName)
        {
            _request.Bcc.Add(new MailboxAddress(displayName, email));
            return this;
        }

        /// <summary>
        /// Adds multiple Bcc email addresses from MailAddressCollection to current Bcc email addresses.
        /// </summary>
        /// <param name="toEmails">MailAddressCollection with Bcc addresses.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithBcc(IEnumerable<MailboxAddress> toEmails)
        {
            CopyEmails(toEmails, _request.Bcc);
            return this;
        }

        /// <summary>
        /// Adds multiple Bcc email addresses from sequence of Tuple email address and display name to current Bcc email addresses. 
        /// </summary>
        /// <param name="toEmails">Sequence of Tuple where Item1 is Bcc email and Item2 is display name in the Tuple.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithBcc(IEnumerable<Tuple<string, string>> toEmails)
        {
            var list = toEmails.ToList();
            list.ForEach(x => _request.Bcc.Add(new MailboxAddress(x.Item2, x.Item1)));
            return this;
        }

        /// <summary>
        /// Adds ReplyTo email address to current ReplyTo email addresses.
        /// </summary>
        /// <param name="email">ReplyTo email address.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithReplyTo(string email)
        {
            _request.ReplyTo.Add(MailboxAddress.Parse(email));
            return this;
        }

        /// <summary>
        /// Adds ReplyTo email address with display name to current ReplyTo email addresses.
        /// </summary>
        /// <param name="email">ReplyTo email address.</param>
        /// <param name="displayName">Display name.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithReplyTo(string email, string displayName)
        {
            _request.ReplyTo.Add(new MailboxAddress(displayName, email));
            return this;
        }

        /// <summary>
        /// Adds multiple ReplyTo email addresses from MailAddressCollection to current ReplyTo email addresses.
        /// </summary>
        /// <param name="toEmails">MailAddressCollection with ReplyTo addresses.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithReplyTo(IEnumerable<MailboxAddress> toEmails)
        {
            CopyEmails(toEmails, _request.ReplyTo);
            return this;
        }

        /// <summary>
        /// Adds multiple ReplyTo email addresses from sequence of Tuple email address and display name to current ReplyTo email addresses. 
        /// </summary>
        /// <param name="toEmails">Sequence of Tuple where Item1 is ReplyTo email and Item2 is display name in the Tuple.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithReplyTo(IEnumerable<Tuple<string, string>> toEmails)
        {
            var list = toEmails.ToList();
            list.ForEach(x => _request.ReplyTo.Add(new MailboxAddress(x.Item2, x.Item1)));
            return this;
        }

        /// <summary>
        /// Sets Razor view's ViewName to render email of.
        /// </summary>
        /// <param name="viewName">Razor view's ViewName.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithViewName(string viewName)
        {
            _request.ViewName = viewName;
            return this;
        }

        /// <summary>
        /// Adds ViewData values to existing ViewData dictionary. ViewData values can be used in Razor view to render email.
        /// </summary>
        /// <param name="key">Key of the value.</param>
        /// <param name="value">Value.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithViewData(string key, object value)
        {
            _request.ViewData.Add(key, value);
            return this;
        }

        /// <summary>
        /// Adds multiple ViewData values from ViewDataDictionary to existing ViewData dictionary.
        /// </summary>
        /// <param name="dictionary">Dictionary of ViewData values.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithViewData(IDictionary<string, object> dictionary)
        {
            foreach (var pair in dictionary)
            {
                _request.ViewData.Add(pair);
            }
            return this;
        }

        /// <summary>
        /// Adds Attachment to existing Attachment collection.
        /// </summary>
        /// <param name="attachment">Attachment.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithAttachment(MimeEntity attachment)
        {
            _request.Attachments.Add(attachment);
            return this;
        }

        /// <summary>
        /// Adds multiple Attachment values to existing Attachment collection.
        /// </summary>
        /// <param name="attachments">Sequence of multiple Attachment values.</param>
        /// <returns>Current EmailNotificationRequestBuilder instance.</returns>
        public EmailNotificationRequestBuilder WithAttachments(AttachmentCollection attachments)
        {
            foreach (var attachment in attachments)
            {
                _request.Attachments.Add(attachment);
            }

            return this;
        }

        /// <summary>
        /// Creates EmailNotificationRequest instance.
        /// </summary>
        /// <returns>Instance of EmailNotificationRequest.</returns>
        public EmailNotificationRequest Build()
        {
            return _request;
        }

        private static EmailNotificationRequest Clone(EmailNotificationRequest request)
        {
            // Do not clone with .WithViewModel(...) as it is cloned already with .WithViewData(...)
            return new EmailNotificationRequestBuilder()
                .WithAttachments(request.Attachments)
                .WithBcc(request.Bcc)
                .WithCc(request.Cc)
                .WithTo(request.To)
                .WithFrom(request.From.Address, request.From.Name)
                .WithSubject(request.Subject)
                .WithViewName(request.ViewName)
                .WithViewData(request.ViewData)
                .Build();

        }

        private void CopyEmails(IEnumerable<MailboxAddress> input, List<MailboxAddress> output)
        {
            foreach (var inp in input)
            {
                output.Add(inp);
            }
        }
    }
}