using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models.Utility
{
    public partial class ModelNotification
    {
        public long UserIdp { get; set; }
        public bool NotificationInApp { get; set; }
        public bool NotificationEmail { get; set; }
        public bool NotificationSMS { get; set; }
    }
}
