using Microsoft.AspNetCore.Hosting;
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using BudgetManagement.Models;
using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;

namespace BMS_DAL
{
    public class EmailNotificationBL
    {
        private readonly BMSContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISettingsService _settingsService;

        //public EmailNotificationBL()
        //{

        //}

        public EmailNotificationBL(BMSContext context, IWebHostEnvironment webHostEnvironment, ISettingsService settingsService)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _settingsService = settingsService;
        }


        public async Task<string> Execute(string toAddress, string[] ccAddresses, string subject, string message)
        {
            return await Execute(toAddress, ccAddresses, subject, message, null);
        }

        public async Task<string> Temp_SendEmail(ParamSMTP smtpData)
        {
            try
            {
                //string strResponse = "";
                //strResponse = await _settingsService.Admin_GetSettings();
                //var data = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponse);

                MailMessage mail = new MailMessage();

                mail.To.Add(new MailAddress(smtpData.MailTo));

                mail.From = new MailAddress(smtpData.SMTPEmail);
                mail.Subject = "BMS - Test Email " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                mail.Body = smtpData.Message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                try
                {
                    using (SmtpClient smtp = new SmtpClient(smtpData.SMTPServer, Convert.ToInt32(smtpData.SMTPPort)))
                    {
#if DEBUG
                        smtp.UseDefaultCredentials = false;
#endif
                        smtp.EnableSsl = smtpData.SMTPSSL;
                        //smtp.Port = smtpData.SMTPPort;
                        //smtp.EnableSsl = false;

                        smtp.Credentials = new NetworkCredential(smtpData.SMTPEmail, smtpData.SMTPPassword);
                        smtp.Send(mail);
                    }
                    return "Email send successfully";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<string> Execute(string toAddress, string[] ccAddresses, string subject, string message, string pdfDoc = null)
        {
            try
            {
                string strResponse = "";
                strResponse = await _settingsService.Admin_GetSettings();
                var data = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponse);

                MailMessage mail = new MailMessage();

                /*
                if (toAddresses != null)
                {
                    foreach (var toAddress in toAddresses)
                    {
                        if (!string.IsNullOrEmpty(toAddress))
                    }
                }
                if (ccAddresses != null)
                {
                    foreach (var ccAddress in ccAddresses)
                    {
                        if (!string.IsNullOrEmpty(ccAddress))
                            mail.CC.Add(new MailAddress(ccAddress));
                    }
                }
                */

                mail.To.Add(new MailAddress(toAddress));
                mail.From = new MailAddress(data[0]["SMTPEmailAddress"].ToString());
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                /*
                if (pdfDoc != null)
                {
                    try
                    {
                        mail.Attachments.Add(new Attachment(pdfDoc));
                    }
                    catch (Exception ex)
                    {
                        ////mail.Subject = subject + "___" + ex.Message;
                    }
                }
                */

                try
                {
                    using (SmtpClient smtp = new SmtpClient(data[0]["SMTPHost"].ToString(), Convert.ToInt32(data[0]["SMTPPort"])))
                    {
#if DEBUG
                        smtp.UseDefaultCredentials = false;
#endif
                        smtp.EnableSsl = Convert.ToBoolean(data[0]["SMTPSSL"]);
                        //smtp.EnableSsl = false;

                        smtp.Credentials = new NetworkCredential(data[0]["SMTPUserName"].ToString(), data[0]["SMTPPassword"].ToString());
                        smtp.Send(mail);
                    }
                    return "Email send successfully";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
