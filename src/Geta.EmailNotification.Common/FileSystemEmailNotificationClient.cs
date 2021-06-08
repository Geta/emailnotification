using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Geta.EmailNotification.Common
{
    public class FileSystemEmailNotificationClient : IEmailNotificationClient, IAsyncEmailNotificationClient
    {
        private readonly IMailMessageFactory _mailMessageFactory;
        private readonly string _pickupDirectoryLocation;

        public FileSystemEmailNotificationClient(IMailMessageFactory mailMessageFactory, string pickupDirectoryLocation)
        {
            _mailMessageFactory = mailMessageFactory;
            _pickupDirectoryLocation = pickupDirectoryLocation;
        }
        
        public EmailNotificationResponse Send(EmailNotificationRequest request)
        {
            return SaveToPickupDirectory(request);
        }

        public Task<EmailNotificationResponse> SendAsync(EmailNotificationRequest request)
        {
            return Task.FromResult(Send(request));
        }
        
        private EmailNotificationResponse SaveToPickupDirectory(EmailNotificationRequest request)
        {
            var response = new EmailNotificationResponse();

            try
            {
                var mail = _mailMessageFactory.Create(request);
                var path = Path.Combine(_pickupDirectoryLocation, Guid.NewGuid() + ".eml");

                using (var stream = new FileStream(path, FileMode.CreateNew))
                {
                    mail.WriteTo(stream);
                    response.IsSent = true;
                }
            }
            catch (Exception e)
            {
                var emails = string.Join(",", 
                    request?.To?.Select(x => x.Address) ?? Enumerable.Empty<string>());
                response.Message = $"Failed to store email in the pickup directory '{_pickupDirectoryLocation}'." +
                                   "Details:" +
                                   $"To = '{emails}'," +
                                   $"Subject = '{request?.Subject}'," +
                                   $"Exception ='{e.Message}'";
            }

            return response;
        }
    }
}