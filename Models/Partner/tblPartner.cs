using System;

namespace BMS_API.Models.Partner
{
    public partial class tblPartner
    {
        public Int64 PartnerIDP { get; set; }
        public string? FullName { get; set; }
        public string? MobileNo { get; set; }
        public string? EmailID { get; set; }
        public string? Password { get; set; }

        public string? ProfileImage { get; set; }
       // public Int32? ActivityTypeID { get; set; }

        public string? ActivityTypeID { get; set; }
        public string? KYCAttachment1 { get; set; }
        public string? KYCAttachment2 { get; set; }
        public string? KYCAttachment3 { get; set; }
        public string? KYCAttachment4 { get; set; }
        public string? VideoAttachment { get; set; }
        public string? SocialFacebook { get; set; }
        public string? SocialLinkedIn { get; set; }
        public string SocialTweeter { get; set; }
        public string SocialTelegram { get; set; }
        public string SocialOther { get; set; }
        public string? SocialInstagram { get; set; }
        public string? RefrenceLink { get; set; }

        public string? YearofExperience { get; set; }
        public Int32? ApplicationStatus { get; set; }

        public string BankDetail { get; set; }
    }
}
