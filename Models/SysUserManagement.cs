using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class ModelUserManagement
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
        public IFormFile UploadDoc { get; set; } // For uploading documents
        public IFormFile Video { get; set; } // For uploading video files
        public IFormFile ProfileImage { get; set; } // For uploading profile images
        public string UploadedDocumentName { get; set; }
        public string Token {  get; set; }
        public string UploadDocPath { get; set; }
        public string VideoPath { get; set; }
        public string ProfileImagePath { get; set; }
        public string? Password { get; set; }
    }
}
