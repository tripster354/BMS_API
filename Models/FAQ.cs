using System;

namespace BMS_API.Models
{
    public partial class FAQ
    {
        public Int64? FAQIDP { get; set; }
        public string? FAQQuestion { get; set; }
        public string? FAQAnswer { get; set; }
        public bool? IsActive { get; set; }
    }
}
