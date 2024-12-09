using System;

namespace BMS_API.Models.DTOs
{
    public class SuggestedOffersDTO
    {
        public long SkillId { get; set; }
        public string OfferCoverImage { get; set; }
        public string OfferPercentage {  get; set; }
        public string Title {  get; set; }
        public string DateTime {  get; set; }
        public string Date {  get; set; }
        public string ProfileImage {  get; set; }
        public string Name {  get; set; }
    }
}
