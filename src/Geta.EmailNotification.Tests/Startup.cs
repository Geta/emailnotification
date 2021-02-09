using Amazon;
using Amazon.SimpleEmail;
using Geta.EmailNotification.Amazon;
using Geta.EmailNotification.MailGun;
using Geta.EmailNotification.Postmark;
using Geta.EmailNotification.SendGrid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PostmarkDotNet;
using RestSharp;
using SendGrid;

namespace Geta.EmailNotification.Tests1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IEmailViewRenderer, EmailViewRenderer>();
            services.AddScoped<IMailMessageFactory, MailMessageFactory>();
            services.AddScoped<IPostmarkMessageFactory, PostmarkMessageFactory>();
            services.AddScoped<IMailGunMessageFactory, MailGunMessageFactory>();
            services.AddScoped<IAmazonMessageFactory, AmazonMessageFactory>();
            services.AddScoped<SendGridMessageFactory, SendGridMessageFactory>();
            services.AddScoped<IRestClient, RestClient>();
            
            //Async notification
            //services.AddScoped<IAsyncEmailNotificationClient, SendGridEmailNotificationClient>();
            //services.AddScoped<IAsyncEmailNotificationClient, PostmarkEmailNotificationClient>();
            services.AddScoped<IAsyncEmailNotificationClient, MailGunEmailNotificationClient>();
            //services.AddScoped<IAsyncEmailNotificationClient, AmazonEmailNotificationClient>();
            
            //Sync notification
            //services.AddScoped<IEmailNotificationClient, SendGridEmailNotificationClient>();
            //services.AddScoped<IEmailNotificationClient, PostmarkEmailNotificationClient>();
            //services.AddScoped<IEmailNotificationClient, MailGunEmailNotificationClient>();
            //services.AddScoped<IEmailNotificationClient, AmazonEmailNotificationClient>();

            // Whitelists test
            // services.AddScoped<MailGunEmailNotificationClient>();
            // services.AddScoped<WhitelistConfiguration>();
            // services.AddScoped<IAsyncEmailNotificationClient>(provider => 
            //     new WhitelistAsyncEmailNotificationClientDecorator(provider.GetRequiredService<MailGunEmailNotificationClient>(),
            //         provider.GetRequiredService<WhitelistConfiguration>()));
            //TODO: Add credentails to services
            services.AddScoped<IAmazonSimpleEmailService>(ctx => 
                new AmazonSimpleEmailServiceClient(
                    "access_key",
                    "secret_key",
                    RegionEndpoint.EUWest1));
            services.AddScoped(_ => new PostmarkClient("server_token"));
            services.AddScoped(_ => 
                new MailGunCredentials("api_key",
                    "api_base_url"));

            services.AddScoped<ISendGridClient>(ctx => 
                new SendGridClient("api_key",
                    "https://api.sendgrid.com", null,"v3", "/mail/send"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}