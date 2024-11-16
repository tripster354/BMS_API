using System;

namespace BMS_API.Models.DTOs
{
    public class ClubSkillsBookingRequest
    {
        public long ActivityID { get; set; }
        public long UserID { get; set; }
        public int SeatsBooked { get; set; }
        public DateTime BookingDate { get; set; }
        public long PartnerID { get; set; }
        public string ContactIDFs { get; set; }
        public int EntryBy { get; set; }
    }
}
