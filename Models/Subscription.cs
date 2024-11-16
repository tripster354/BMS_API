using System;

namespace BMS_API.Models
{
    public partial class Subscription
    {
        public Int64? SubscriptionIDP { get; set; }
        public string? SubscriptionPackage { get; set; }
        public Int16? SubscriptionType { get; set; }
        public decimal? SubscriptionFeesMonthly { get; set; }
        public decimal? SubscriptionFeesYearly { get; set; }

        public bool? IsActive { get; set; }
  
    }
}
