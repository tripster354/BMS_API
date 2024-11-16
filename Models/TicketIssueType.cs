using System;

namespace BMS_API.Models
{
    public partial class TicketIssueType
    {
        public Int64? TicketIssueTypeIDP { get; set; }
        public string? TicketIssueTypeName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
