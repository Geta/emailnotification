using System.IO;
using Amazon.SimpleEmail.Model;
using MimeKit;

namespace Geta.EmailNotification.Amazon.Extensions
{
    public static class AmazonExtensions
    {
        public static SendRawEmailRequest ToAmazonMessage(this MimeMessage message)
        {
            using (var messageStream = new MemoryStream())
            {
                message.WriteTo(messageStream);
                
                return new SendRawEmailRequest
                {
                    RawMessage = new RawMessage { Data = messageStream }
                };
            }
        }
    }
}