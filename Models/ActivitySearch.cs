using System;

namespace BMS_API.Models
{
    public partial class ActivitySearch
    {
        public string? InterestIDPs { get; set; }
        public Int64? TimeSlot { get; set; }
        public Int64? LocationMeter { get; set; }
        public Int64? StartPrice { get; set; }
        public Int64? EndPrice { get; set; }
        public Int64? PeopleCount { get; set; }

        public float? Rating { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? SearchKeyWord { get; set; }
        public int IsMyActivity { get; set; }
        public int DisplayTypeID { get; set; }
        public int InterestID { get; set; }

        public Int64? ActivityID { get; set; }

    }
}
