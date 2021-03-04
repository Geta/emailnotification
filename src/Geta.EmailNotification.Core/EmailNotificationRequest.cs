using System.Collections.Generic;
using Geta.EmailNotification.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MimeKit;
using AttachmentCollection = MimeKit.AttachmentCollection;

namespace Geta.EmailNotification.Core
{
    public class EmailNotificationRequest : EmailNotificationRequestBase
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
        /// Key/value collection for placeholders
        /// </summary>
        public new ViewDataDictionary ViewData { get; set; }
    }
}