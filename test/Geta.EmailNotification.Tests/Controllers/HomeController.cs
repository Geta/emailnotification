using Geta.EmailNotification.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Geta.EmailNotification.Common;
using Geta.EmailNotification.Tests.Helpers;
using MimeKit;
using MimePart = MimeKit.MimePart;

namespace Geta.EmailNotification.Tests.Controllers
{
    /// <summary>
    /// Examples and tests using SmtpEmailNotificationClient, PostmarkEmailNotificationClient, AmazonEmailNotificationClient
    /// Using attachments, html and text emails
    /// Using views with placeholders for html body
    /// embed images and rewrite relative urls to absolute urls
    /// Show configuration of different clients
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IAsyncEmailNotificationClient[] _asyncClients;
        private readonly IEmailNotificationClient[] _syncClients;

        public HomeController(IAsyncEmailNotificationClient[] asyncClients, IEmailNotificationClient[] syncClients)
        {
            _asyncClients = asyncClients;
            _syncClients = syncClients;
        }

        public async Task<ActionResult> Index()
        {
            var results = new List<EmailNotificationResponseViewModel>();
                
            foreach (var asyncClient in _asyncClients)
            {
                var stream = StreamHelper.GenerateStreamFromString("This is a test text.");
                var attachment = new MimePart ("text", "plain") {
                    Content = new MimeContent (stream),
                    ContentDisposition = new ContentDisposition (ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "test_attachment.txt"
                };
               
                var testEmail = new EmailNotificationRequestBuilder()
                    .WithTo("zbigniew.winiarski@getadigital.com")
                    .WithFrom("zbigniew.winiarski@getadigital.com")
                    .WithSubject($"Async test e-mail using {asyncClient.ToString().Split('.').Last()}")
                    .WithAttachment(attachment)
                    .WithViewName("Test")
                    .WithViewData("test","value")
                    .Build();

                results.Add(new EmailNotificationResponseViewModel
                {
                    NotificationResponse = await asyncClient.SendAsync(testEmail),
                    AssemblyName = $"{asyncClient.ToString().Split('.').Last()}"
                });
            }

            foreach (var syncClient in _syncClients)
            {
                var testEmail = new EmailNotificationRequestBuilder()
                    .WithTo("zbigniew.winiarski@getadigital.com")
                    .WithFrom("zbigniew.winiarski@getadigital.com")
                    .WithSubject($"Sync test e-mail using {syncClient.ToString().Split('.').Last()}")
                    .Build();
                testEmail.HtmlBody = new HtmlString("This is a test email <strong>using HtmlBody</strong>.").ToString();

                results.Add(new EmailNotificationResponseViewModel
                {
                    NotificationResponse = syncClient.Send(testEmail),
                    AssemblyName = $"{syncClient.ToString().Split('.').Last()}"
                });
            }

            return View(results);
        }
    }
}