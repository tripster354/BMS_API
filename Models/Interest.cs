using System;

namespace BMS_API.Models
{
    public partial class Interest
    {
        public Int64? InterestIDP { get; set; }
        public string? InterestName { get; set; }
        public string? InterestAttachment { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
