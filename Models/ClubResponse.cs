using BMS_API.Models.User;
using System;
using System.Collections.Generic;

namespace BMS_API.Models
{
    public class ClubResponse
    {
        public Int64? ClubIDP { get; set; }
        public string? ClubName { get; set; }
        public string? ClubBanner { get; set; }
        public string? ClubVenue { get; set; }
        public string? ClubDescription { get; set; }
        public List<tblUser> UserInfoList {get; set;}
  
    }
}
