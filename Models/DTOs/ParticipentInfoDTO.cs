using System;

namespace BMS_API.Models.DTOs
{
    public class ParticipentInfoDTO
    {
        public string ParticipantName {  get; set; }
        public string ParticiantImage {  get; set; }
        public string SkillName {  get; set; }
        public DateTime ParticipantJoinDate { get; set; }
    }
}
