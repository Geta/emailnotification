using System.Collections.Generic;
using System.Web.Mvc;
using Geta.EmailNotification.Common;
using MimeKit;

namespace Geta.EmailNotification
{
    public class EmailNotificationRequest : EmailNotificationRequestBase
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
        /// Key/value collection for placeholders
        /// </summary>
        public new ViewDataDictionary ViewData { get; set; }
    }
}