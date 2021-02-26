using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Geta.EmailNotification.Tests.Helpers;
using Geta.EmailNotification.Tests.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Geta.EmailNotification.Tests.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<IAsyncEmailNotificationClient> _asyncClients;
        private readonly IEnumerable<IEmailNotificationClient> _syncClients;

        public HomeController(IEnumerable<IAsyncEmailNotificationClient> asyncClients, 
            IEnumerable<IEmailNotificationClient> syncClients)
        {
            _asyncClients = asyncClients;
            _syncClients = syncClients;
        }

        public async Task<IActionResult> Index()
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
                    .WithViewName("Emails/Test")
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
                testEmail.HtmlBody = new HtmlString("This is a test email <strong>using HtmlBody</strong>.");

                results.Add(new EmailNotificationResponseViewModel
                {
                    NotificationResponse = syncClient.Send(testEmail),
                    AssemblyName = $"{syncClient.ToString().Split('.').Last()}"
                });
            }

            return View(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}