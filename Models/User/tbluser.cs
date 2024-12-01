using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BMS_API.Models.User
{
    public partial class tblUser
    {
        public Guid RowGUID { get; set; } = Guid.NewGuid();
        public Int64 UserIDP { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? MobileNo { get; set; }
        public string EmailID { get; set; }
        public string? Password { get; set; }
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? SocialGoogle { get; set; }
        public string? SocialFacebook { get; set; }
        public string? SocialLinkedIn { get; set; }
        public string? SocialInstagram { get; set; }
        public string? SocialTweeter { get; set; }
        public string? SocialTelegram { get; set; }
        public string? SocialOther { get; set; }
        public string? AboutMe { get; set; }
        public int? TotalFollowers { get; set; }
        public int? TotalConnection { get; set; }
        public string? RefrenceLink { get; set; }
        public string? RefrenceLinkUsed { get; set; }
        public bool IsActive { get; set; }
        public int OTP { get; set; }
        public long? LoginToken { get; set; }
        public string? InterestIDs { get; set; }
        public int? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool IsDeleted { get; set; }
    }



    public partial class tblUserRequestModel
    {
        public Guid RowGUID { get; set; } = Guid.NewGuid();
        public Int64 UserIDP { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? MobileNo { get; set; }
        public string EmailID { get; set; }
        public string? Password { get; set; }
        public IList<IFormFile> ProfileImage { get; set; }
        public string? Address { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string? SocialGoogle { get; set; }
        public string? SocialFacebook { get; set; }
        public string? SocialLinkedIn { get; set; }
        public string? SocialInstagram { get; set; }
        public string? SocialTweeter { get; set; }
        public string? SocialTelegram { get; set; }
        public string? SocialOther { get; set; }
        public string? AboutMe { get; set; }
        public int? TotalFollowers { get; set; }
        public int? TotalConnection { get; set; }
        public string? RefrenceLink { get; set; }
        public string? RefrenceLinkUsed { get; set; }
        public bool IsActive { get; set; }
        public int OTP { get; set; }
        public long? LoginToken { get; set; }
        public string? InterestIDs { get; set; }
        public int? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool IsDeleted { get; set; }
    }

}
