# Geta Email Notification

* Master<br>
![](http://tc.geta.no/app/rest/builds/buildType:(id:GetaPackages_GetaEmailNotification_00ci),branch:master/statusIcon)

## Description
This project contains three providers for sending emails. The default is using normal SMTP, in addition we have one for SendGrid, one for Postmark, one for MailGun and one for Amazon SES. All are using the same interface and can easily be switched in and out as required.

We recommend using SendGrid for your application email needs.

See Geta.EmailNotification.Tests for some sample code.

## Features
- Support for Razor Views
- Strongly typed access and ViewBag available (see Tests projects, Views/Emails/Test.cshtml)
- Support for BCC, CC, multiple address'
- Fluent API to build email requests with EmailNotificationRequestBuilder
- Support for text and HTML emails
- Support for attachments
- Extension method to easily convert relative to absolute URLs (@Url.Absolute("~/Content/dummy.png")). Use appSettings with key: Geta.EmailNotification.BaseUrl to override the base URL used.
- You can choose to send emails synchronized or async.

## Installation
Available through Geta's NuGet feed:
- Geta.EmailNotification
- Geta.EmailNotification.Sendgrid
- Geta.EmailNotification.Postmark
- Geta.EmailNotification.Amazon
- Geta.EmailNotification.MailGun


To use SMTP you need to configure it using StructureMap:
```csharp
For<IEmailViewRenderer>().Use<EmailViewRenderer>();
For<IEmailNotificationClient>().Use<SmtpEmailNotificationClient>();
For<IMailMessageFactory>().Use<MailMessageFactory>();
```

If you're using Postmark, MailGun or SendGrid then look further down in this documentation for instructions.

## EmailNotificationRequest and EmailNotificationResponse

### EmailNotificationRequest
```csharp
public class EmailNotificationRequest
{
	public EmailNotificationRequest()
	{
		this.Attachments = new List<Attachment>();
		this.To = new MailAddressCollection();
		this.Cc = new MailAddressCollection();
		this.Bcc = new MailAddressCollection();
		this.ReplyTo = new MailAddressCollection();
		this.ViewData = new ViewDataDictionary(this);
	}

	/// <summary>
	/// From email address
	/// </summary>
	public MailAddress From { get; set; }

	/// <summary>
	/// To email address'
	/// </summary>
	public MailAddressCollection To { get; set; }

	/// <summary>
	/// Copy email address'
	/// </summary>
	public MailAddressCollection Cc { get; set; }

	/// <summary>
	/// Blind copy email address'
	/// </summary>
	public MailAddressCollection Bcc { get; set; }

	/// <summary>
	/// Reply to email address'
	/// </summary>
	public MailAddressCollection ReplyTo { get; set; }

	public string Subject { get; set; }

	/// <summary>
	/// HTML content for HTML emails
	/// </summary>
	public IHtmlString HtmlBody { get; set; }

	/// <summary>
	/// Text content for fallback or text only emails
	/// </summary>
	public string Body { get; set; }

	/// <summary>
	/// Razor view name (without .cshtml)
	/// </summary>
	public string ViewName { get; set; }

	/// <summary>
	/// Key/value collection for placeholders
	/// </summary>
	public ViewDataDictionary ViewData { get; set; }

	/// <summary>
	/// By default we try and send asynchronous
	/// </summary>
	public bool SendSynchronous { get; set; }

	/// <summary>
	/// Attachments for this email message
	/// </summary>
	public List<Attachment> Attachments { get; set; }
}
```

### EmailNotificationResponse
```csharp
public class EmailNotificationResponse
{
	/// <summary>
	/// True if sent, false otherwise
	/// </summary>
	public bool IsSent { get; set; }

	/// <summary>
	/// Any exception or error code/details that the different services return will be added here
	/// </summary>
	public string Message { get; set; }
}
```

Examples
--------
```html
@using Geta.EmailNotification

@model EmailNotificationRequest
<html>
<body>
    <p>Sent: @ViewBag.Date</p>
    <p>
        Dear @Model.To.First().Address
    </p>
    <p>
        <a href="@Url.Absolute("~/")">This is a link</a>
    </p>
    <p>
        <img src="@Url.Absolute("~/Content/dummy.png")" alt="" />
    </p>
</body>
</html>
```

### Creating EmailNotificationRequest directly.

```csharp
var client = new SmtpEmailNotificationClient(emailViewRenderer);
var email = new EmailNotificationRequest();
email.To.Add(new MailAddress("frederik@geta.no"));
email.From = new MailAddress("no-reply@example.com");
email.ReplyTo.Add(new MailAddress("reply-to@example.com"));
email.Bcc.Add(new MailAddress("andre@geta.no"));
email.Cc.Add(new MailAddress("maris@geta.no"));
email.Cc.Add(new MailAddress("mattias@geta.no"));

mail.Subject = "Test email";

email.ViewName = "Test";
email.ViewData.Add("Date", DateTime.UtcNow);

client.Send(email);
```

### Creating EmailNotificationRequest using EmailNotificationRequestBuilder.

```csharp
var client = new SmtpEmailNotificationClient(emailViewRenderer);
var email = new EmailNotificationRequestBuilder()
                .WithTo("frederik@geta.no")
                .WithFrom("no-reply@example.com")
                .WithReplyTo("reply-to@example.com")
                .WithBcc("andre@geta.no")
                .WithCc("maris@geta.no")
                .WithCc("mattias@geta.no")
                .WithSubject("Test email")
                .WithViewName("Test")
                .WithViewData("Date", DateTime.UtcNow)
                .Build();
client.Send(email);
```

### Reusing existing EmailNotificationRequestBuilder with EmailNotificationRequestBuilder.

```csharp
var client = new SmtpEmailNotificationClient(emailViewRenderer);
var template = new EmailNotificationRequestBuilder()
                .WithSubject("This is template")
                .WithFrom("no-reply@example.com")
                .WithViewName("Test")
                .WithViewData("Date", DateTime.UtcNow)
                .Build();
var email1 = new EmailNotificationRequestBuilder(template)
                .WithTo("maris@geta.no")
                .Build();
var email2 = new EmailNotificationRequestBuilder(template)
                .WithTo("mattias@geta.no")
                .Build();
client.Send(email1);
client.Send(email2);
```

## Email whitelist

It is possible to enable an email whitelist feature. The email whitelist feature makes sure that emails are sent only to emails and domains added into the whitelist. This is useful for test environments where sending emails to all email recipients is prohibited.

### Configuration

First of all, decorate _IEmailNotificationClient_ with _WhitelistEmailNotificationClientDecorator_. It can be achieved in the _StructureMap_ configuration.

```
For<IEmailNotificationClient>().DecorateAllWith<WhitelistEmailNotificationClientDecorator>();
For<IEmailNotificationClient>().Use<SmtpEmailNotificationClient>();
```

Next step, configure a whitelist in the _appSettings_. Add an item with a key _EmailNotification:Whitelist_ and add whitelist values. Whitelist values can be emails or domains starting with @ sign. Separate those with semicolon (;).

```
<add key="EmailNotification:Whitelist" value="info@example.com;@geta.no" />
```

## Postmark
When using the Postmark library you need to configure it using StructureMap.

```csharp
For<IEmailNotificationClient>().Use<PostmarkEmailNotificationClient>();
For<PostmarkClient>().Use(() => new PostmarkClient(Configuration.Postmark.ApiKey, Configuration.Postmark.ApiBaseUri, Configuration.Postmark.TimeoutInSeconds));
For<IPostmarkMessageFactory>().Use<PostmarkMessageFactory>();
```

You can get the Postmark API key from https://account.postmarkapp.com. BaseUri is set to: https://api.postmarkapp.com and timeout to 30 seconds by default.

## MailGun
Define and configure using StructureMap

```csharp
For<IEmailNotificationClient>().Use<MailGunEmailNotificationClient>();
For<IMailgunClient>()
                .Use(ctx => new MailgunClient(SiteConfiguration.MailGun.Domain, SiteConfiguration.MailGun.ApiKey, 3));
   
```

ApiKey and Domain can be found in MailGun Settings. BaseDomain sets test/live.

## SendGrid
To use SendGrid in your project, you need to create a new sendgrid subuser and generate an api key:
1. Go to https://sendgrid.com/ and login/create account.
2. In settings, go to subuser management and create a new subuser.
3. Switch to that subuser account and create an Api Key with the privileges that you deem necessary.
4. Store that api key in your favorite secure setting and use it to define and configure through StructureMap:
```csharp
For<IAsyncEmailNotificationClient>().Use<SendGridEmailNotificationClient>(); // For async calls
For<IEmailNotificationClient>().Use<SendGridEmailNotificationClient>(); // For sync calls
For<ISendGridClient>().Use(ctx => new SendGridClient(<<Your_API_Key>>, "https://api.sendgrid.com", null,"v3", "/mail/send")); //the "/mail/send" url path is one of many endpoints that you can use. Set null for default.
// Dont forget to register your viewengines and mailfactory as defined above:
For<IEmailViewRenderer>().Use<EmailViewRenderer>(() => new EmailViewRenderer(new ViewEngineCollection { new RazorViewEngine() })); // if necessary to create a new view engine
For<IMailMessageFactory>().Use<MailMessageFactory>();
```
5. Check test project and follow the steps used to create sync or async e-mails.

### Sender signatures ###
Watch this tutorial to learn how to set up your sender signatures (link branding): [Sendgrid - Link branding](https://sendgrid.com/docs/User_Guide/Settings/Sender_authentication/How_to_set_up_link_branding.html)

## Package Maintainer
https://github.com/digintsys

https://github.com/marisks

https://github.com/frederikvig

