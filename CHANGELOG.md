# Changelog

## [3.0.1]

### Changed
- Remove dependency to Microsoft.AspNet.Mvc from Geta.EmailNotifications
- Updates Amazon SDK for Geta.EmailNotifications.Amazon

### Fixed
- Fix missing dependencies in nuspec files

## [3.0.0]

### Changed
- Fix issue #2 migrate to netstandard2.0
- Add possiblity to use Views for emails for Amazon and Mailgun
- Remove URlHelperExtensions
- Add possibility to use Whitelists in async clients

### Fixed
- Fix broken whitelists introduced in 2.0.0

## [2.0.0]

### Changed
- Fix issue #1 replace obsolete SmtpClient to MailKit in Amazon, SendGrid and Postmark projects
- MailGun uses RestSharp instead of mnailgun library
- Adds possibility to send attachments to Amazon and Mailgun
- Upgrade to TargetFrameworkVersion v4.7.2

### Fixed
- Fix integration with Amazon and MailGun
- Fix issue #7 add changelog
