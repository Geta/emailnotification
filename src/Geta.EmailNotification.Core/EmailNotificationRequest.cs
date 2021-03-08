using Geta.EmailNotification.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Geta.EmailNotification.Core
{
    public class EmailNotificationRequest : EmailNotificationRequestBase
    {
        public EmailNotificationRequest()
        {
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
        }

        /// <summary>
        /// Key/value collection for placeholders
        /// </summary>
        public new ViewDataDictionary ViewData { get; set; }
    }
}