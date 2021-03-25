namespace Geta.EmailNotification.Common
{
    /// <summary>
    /// Renders an email view.
    /// </summary>
    public interface IEmailViewRenderer
    {
        /// <summary>
        /// Renders an email view based on the provided view passed in email.ViewName.
        /// </summary>
        /// <param name="email">The email data to pass to the view.</param>
        /// <returns>The string result of rendering the email.</returns>
        string Render(EmailNotificationRequest email);
    }
}