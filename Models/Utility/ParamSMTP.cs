using System;
using static BMS_API.Services.Interface.ICommon;

namespace BudgetManagement.Models.Utility
{
    public class ParamSMTP
    {
        /// <summary>
        /// Any main primary / forgine key in which user is right now
        /// </summary>
        public string SMTPServer { get; set; }
        public string SMTPEmail { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPPort { get; set; }
        public bool SMTPSSL { get; set; }
        public string Message { get; set; }
        public string MailTo { get; set; }

    }
}