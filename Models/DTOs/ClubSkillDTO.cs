using System;

namespace BMS_API.Models.DTOs
{
    public class ClubSkillDTO
    {
        public long SkillID { get; set; }
        public string SkillCoverImage {  get; set; }
        public string SkillName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string ProfileImage {  get; set; }
        public string UserName {  get; set; }
        public string Description {  get; set; }
        public int TotalSeats {  get; set; }
        public int AllocatedSeats {  get; set; }
    }
}
