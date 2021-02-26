using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MimeKit;
using AttachmentCollection = MimeKit.AttachmentCollection;

namespace Geta.EmailNotification
{
    public class EmailNotificationRequest
    {
        public EmailNotificationRequest()
        {
            Attachments = new AttachmentCollection();
            To = new List<MailboxAddress>();
            Cc = new List<MailboxAddress>();
            Bcc = new List<MailboxAddress>();
            ReplyTo = new List<MailboxAddress>();
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
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
        public HtmlString HtmlBody { get; set; }

        /// <summary>
        /// Text content for fallback or text only emails
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Razor view path and name (without .cshtml). Example: Emails/Test
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Key/value collection for placeholders
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }

        /// <summary>
        /// Attachments for this email message
        /// </summary>
        public AttachmentCollection Attachments { get; set; }
    }
}