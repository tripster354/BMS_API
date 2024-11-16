using System;

namespace BMS_API.Models
{
    public partial class Poll
    {
        public Int64? PollIDP { get; set; }
        public string? Question { get; set; }
        public string? Opt1 { get; set; }
        public string? Opt2 { get; set; }
        public string? Opt3 { get; set; }
        public string? Opt4 { get; set; }
        public Int64? Opt1Count { get; set; }
        public Int64? Opt2Count { get; set; }
        public Int64? Opt3Count { get; set; }
        public Int64? Opt4Count { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
