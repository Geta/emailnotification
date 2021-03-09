using System.Web.Mvc;
using Geta.EmailNotification.Common;

namespace Geta.EmailNotification
{
    public class EmailNotificationRequest : EmailNotificationRequestBase
    {
        public EmailNotificationRequest()
        {
            ViewData = new ViewDataDictionary(this);
        }
        
        /// <summary>
        /// Key/value collection for placeholders
        /// </summary>
        public new ViewDataDictionary ViewData { get; set; }
    }
}