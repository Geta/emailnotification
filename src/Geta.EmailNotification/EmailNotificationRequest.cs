using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MimeKit;
using AttachmentCollection = MimeKit.AttachmentCollection;

namespace Geta.EmailNotification
{
    public class EmailNotificationRequest
    {
        public EmailNotificationRequest()
        {
            this.Attachments = new AttachmentCollection();
            this.To = new List<MailboxAddress>();
            this.Cc = new List<MailboxAddress>();
            this.Bcc = new List<MailboxAddress>();
            this.ReplyTo = new List<MailboxAddress>();
            this.ViewData = new ViewDataDictionary(this);
        }

        /// <summary>
        /// From email address
        /// </summary>
        public MailboxAddress From { get; set; }

        /// <summary>
        /// To email address'
        /// </summary>
        public List<MailboxAddress> To { get; set; }

        /// <summary>
        /// Copy email address'
        /// </summary>
        public List<MailboxAddress> Cc { get; set; }

        /// <summary>
        /// Blind copy email address'
        /// </summary>
        public List<MailboxAddress> Bcc { get; set; }

        /// <summary>
        /// Reply to email address'
        /// </summary>
        public List<MailboxAddress> ReplyTo { get; set; }

        public string Subject { get; set; }

        /// <summary>
        /// HTML content for HTML emails
        /// </summary>
        public IHtmlString HtmlBody { get; set; }

        /// <summary>
        /// Text content for fallback or text only emails
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Razor view name (without .cshtml)
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Key/value collection for placeholders
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// By default we try and send asynchronous
        /// </summary>
        [Obsolete("This property is not in use anymore. IEmailNotificationClient.Send will be always synchronous. For async use IAsyncEmailNotificationClient.SendAsync.")]
        public bool SendSynchronous { get; set; }

        /// <summary>
        /// Attachments for this email message
        /// </summary>
        public AttachmentCollection Attachments { get; set; }
    }
}