using System.Collections.Generic;
using MimeKit;

namespace Geta.EmailNotification.Common
{
    public class EmailNotificationRequest
    {
        public EmailNotificationRequest()
        {
            ViewData = new Dictionary<string, object>();
        }
        /// <summary>
        /// From email address
        /// </summary>
        public MailboxAddress From { get; set; }

        /// <summary>
        /// To email address'
        /// </summary>
        public List<MailboxAddress> To { get; set; } = new List<MailboxAddress>();

        /// <summary>
        /// Copy email address'
        /// </summary>
        public List<MailboxAddress> Cc { get; set; } = new List<MailboxAddress>();

        /// <summary>
        /// Blind copy email address'
        /// </summary>
        public List<MailboxAddress> Bcc { get; set; } = new List<MailboxAddress>();

        /// <summary>
        /// Reply to email address'
        /// </summary>
        public List<MailboxAddress> ReplyTo { get; set; } = new List<MailboxAddress>();

        public string Subject { get; set; }

        /// <summary>
        /// Text content for fallback or text only emails
        /// </summary>
        public string Body { get; set; }
        
        /// <summary>
        /// HTML content for HTML emails
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// Razor view path and name (without .cshtml). Example: Emails/Test
        /// </summary>
        public string ViewName { get; set; }
        
        /// <summary>
        /// Key/value collection for placeholders
        /// </summary>
        public IDictionary<string, object> ViewData { get; set; }
        
        /// <summary>
        /// Model used in view
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Attachments for this email message
        /// </summary>
        public AttachmentCollection Attachments { get; set; } = new AttachmentCollection();
    }
}