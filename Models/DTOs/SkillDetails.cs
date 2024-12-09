using Microsoft.CodeAnalysis;
using System;

namespace BMS_API.Models.DTOs
{
    public class SkillDetails
    {
        public int? LikeStatus { get; set; }
        public string SkillName { get; set; }
        public string? ProfileImage { get; set; }
        public string FullName { get; set; }
        public double? Rating { get; set; }
        public double? Price { get; set; }
        public string Location { get; set; }
        public int? RemainingSeats { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
