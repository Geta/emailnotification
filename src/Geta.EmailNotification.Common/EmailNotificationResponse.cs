namespace Geta.EmailNotification.Common
{
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
}