using System;

namespace BMS_API.Models
{
    public partial class ActivityBooked
    {
        public Int64? AvtivityBookedIDP { get; set; }
        public Int64? PartnerIDF { get; set; }
        public Int64? ActivityIDF { get; set; }
        public Int16? BookingStatus { get; set; }
        public string? ContactIDPs { get; set; }

    }
}
