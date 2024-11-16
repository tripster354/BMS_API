using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS_DAL
{
    public interface IEmailNotificationBL
    {
        /// <summary>
        /// Sending Email Async
        /// </summary>
        /// <param name="email">Email id</param>
        /// <param name="subject">Mail subject</param>
        /// <param name="message">Message</param>
        /// <param name="contentId">Content image unique identifier</param>
        /// <param name="filePath">File location</param>
        /// <returns></returns>
        Task SendEmailAsync(string toEmail, string ccEmail, string subject, string message, Dictionary<string, string> attachments);

        /// <summary>
        /// Sending Email Async
        /// </summary>
        /// <param name="email">Email id</param>
        /// <param name="subject">Mail subject</param>
        /// <param name="message">Message</param>
        /// <param name="contentId">Content image unique identifier</param>
        /// <param name="filePath">File location</param>
        /// <returns></returns>
        Task SendEmailAsync(string[] toEmails, string[] ccEmails, string subject, string message, Dictionary<string, string> attachments);
    }
}
