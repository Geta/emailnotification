using PostmarkDotNet;
using SendGrid;
using Geta.EmailNotification.Postmark;
using Geta.EmailNotification.MailGun;
using System.Web.Mvc;
using Amazon;
using Amazon.SimpleEmail;
using Geta.EmailNotification.Amazon;
using Geta.EmailNotification.Common;
using Geta.EmailNotification.SendGrid;
using RestSharp;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Geta.EmailNotification.Tests.DependencyResolution
{
    public class DefaultRegistry : Registry {

        public DefaultRegistry() {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
					scan.With(new ControllerConvention());
                });

            For<IEmailViewRenderer>().Use(() => new EmailViewRenderer(new ViewEngineCollection { new RazorViewEngine() }));
            For<IMailMessageFactory>().Use<MailMessageFactory>();
            For<IPostmarkMessageFactory>().Use<PostmarkMessageFactory>();
            For<IAmazonMessageFactory>().Use<AmazonMessageFactory>();
            // For<IAsyncEmailNotificationClient>().Use<SendGridEmailNotificationClient>();
            // For<IAsyncEmailNotificationClient>().Use<PostmarkEmailNotificationClient>();
            For<IAsyncEmailNotificationClient>().Use<AmazonEmailNotificationClient>();
            // For<IAsyncEmailNotificationClient>().Use<MailGunEmailNotificationClient>();
            
            // For<IEmailNotificationClient>().Use<SendGridEmailNotificationClient>();
            // For<IEmailNotificationClient>().Use<PostmarkEmailNotificationClient>();
            // For<IEmailNotificationClient>().Use<MailGunEmailNotificationClient>();
            For<IEmailNotificationClient>().Use<AmazonEmailNotificationClient>();
            For<IRestClient>().Use(ctx => new RestClient());
	        // TODO: update with real keys to test
            For<IAmazonSimpleEmailService>().Use(ctx => 
                new AmazonSimpleEmailServiceClient(
                    "access_key",
                    "secret_key",
                    RegionEndpoint.EUWest1));
            For<PostmarkClient>().Use(ctx => 
                new PostmarkClient("server_token", "base_url", 30));
            For<MailGunCredentials>().Use(ctx => 
                new MailGunCredentials("api_key","api_base_url"));
            For<ISendGridClient>().Use(ctx => 
                new SendGridClient("api_key", "https://api.sendgrid.com", null,"v3", "/mail/send"));
        }
    }
}
