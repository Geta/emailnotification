using Geta.EmailNotification.SendGrid;
using PostmarkDotNet;
using SendGrid;
using Typesafe.Mailgun;
using Geta.EmailNotification.Postmark;
using Geta.EmailNotification.MailGun;
using System.Web.Mvc;
using Amazon;
using Amazon.SimpleEmail;
using Geta.EmailNotification.Amazon;
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
            // For<IAsyncEmailNotificationClient>().Use<SendGridEmailNotificationClient>();
            // For<IAsyncEmailNotificationClient>().Use<PostmarkEmailNotificationClient>();
            For<IAsyncEmailNotificationClient>().Use<AmazonEmailNotificationClient>();
            
            //For<IEmailNotificationClient>().Use<SendGridEmailNotificationClient>();
            //For<IEmailNotificationClient>().Use<PostmarkEmailNotificationClient>();
            // For<IEmailNotificationClient>().Use<MailGunEmailNotificationClient>();
             For<IEmailNotificationClient>().Use<AmazonEmailNotificationClient>();
	    
	        // TODO: update with real keys to test
            For<IAmazonSimpleEmailService>().Use(ctx => 
                new AmazonSimpleEmailServiceClient(
                "access_key",
                "secret_key",
                RegionEndpoint.USEast1));
            For<PostmarkClient>().Use(ctx => new PostmarkClient("token", "https://api.postmarkapp.com", 30));
            // For<IMailgunClient>().Use(ctx => new MailgunClient("url", "key", 3));
            For<ISendGridClient>().Use(ctx => 
                new SendGridClient("api_key", "https://api.sendgrid.com", null,"v3", "/mail/send"));
        }

    }
}
