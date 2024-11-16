using System;

namespace BMS_API.Models.DTOs
{
    public class ActivityListDTO
    {
        public long SkillId {  get; set; }
        public string? BannerAttachmentPath { get; set; }
        public string? ActivityName { get; set; }
        public string? ActivityAbout { get; set; }
        public string? Venue { get; set; }
       
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public int? TotalSeats { get; set; }
        
        public Decimal? Price { get; set; }
        public string? WebinarLink { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string? ActivityInterestName {  get; set; }
    }
}
