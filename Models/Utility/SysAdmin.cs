using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class SysAdmin
    {
        public int? AdminIDP { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SMTPEmailAddress { get; set; }
        public string? SMTPUserName { get; set; }
        public string? SMTPPassword { get; set; }
        public string? SMTPHost { get; set; }
        public Nullable<Int32> SMTPPort { get; set; }
        public Nullable<bool> SMTPSSL { get; set; }
        public Nullable<Int32> OTP { get; set; }
        public string? GoogleMapKey { get; set; }
        public string? GoogleLocationKey { get; set; }
        public string? PaymentGatewayKey { get; set; }
        public string? RevenueCommission { get; set; }
        public string? CDNPath { get; set; }
    }
}
