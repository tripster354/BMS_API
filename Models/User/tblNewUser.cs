using Microsoft.AspNetCore.Http;
using System;

namespace BMS_API.Models.User
{
    public class tblNewUser
    {
        public Guid RowGUID { get; set; } = Guid.NewGuid();
        public Int64 UserIDP { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? MobileNo { get; set; }
        public string EmailID { get; set; }
        public string? Password { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Address { get; set; }
        public string AboutMe { get; set; }
        public string SocialGoogle { get; set; }
        public string SocialFacebook { get; set; }
        public string SocialLinkedIn { get; set; }
        public string SocialInstagram { get; set; }
        public string SocialTweeter { get; set; }
        public string SocialTelegram { get; set; }
        public string SocialOther { get; set; }
        public bool IsActive { get; set; }
    }
}
