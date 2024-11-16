using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class SettingsSMTP
	{
		public string SMTPEmailAddress { get; set; }
		public string SMTPUserName { get; set; }
		public string SMTPPassword { get; set; }
		public string SMTPHost { get; set; }
		public Nullable<Int32> SMTPPort { get; set; }
		public Nullable<bool> SMTPSSL { get; set; }
        public Nullable<Int32> CostCenterStaringSeries { get; set; }
        public Nullable<Int32> CostCenterGapSeries { get; set; }
        public string Currency { get; set; }
    }
}
