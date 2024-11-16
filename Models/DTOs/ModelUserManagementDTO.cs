using Microsoft.AspNetCore.Http;
using System;

namespace BMS_API.Models.DTOs
{
    public class ModelUserManagementDTO
    {
        public Int64? UserIDP { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string InstaLink { get; set; } // Optional
        public string TwitterLink { get; set; } // Optional
        public string LinkedInLink { get; set; } // Optional
        public string WebSite { get; set; } // Optional
        public string ActivityInterestName { get; set; } // Optional
        public int? YearsOfExperience { get; set; } // Optional
        public string UploadedDocumentName { get; set; }
        public string Token { get; set; }
        public string UploadDocPath { get; set; }
        public string VideoPath { get; set; }
        public string ProfileImagePath { get; set; }
        public string? Password { get; set; }
    }
}
