using System;

namespace BMS_API.Models.User
{
    public partial class Club
    {
        public Int64? ClubIDP { get; set; }
        public Int64? InterestIDF { get; set; }
        public string? ClubName { get; set; }
        public string? ClubBanner { get; set; }
        public string? ClubVenue { get; set; }
        public string? ClubDescription { get; set; }
        
    }
}
