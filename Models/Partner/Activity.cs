﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace BMS_API.Models.Partner
{
    public partial class Activity
    {
        public Int64? ActivityIDP { get; set; }
        public Int64? InterestIDF { get; set; }
        public string? BannerAttachment { get; set; }
        public string? ActivityTitle { get; set; }
        public string? ActivityAbout { get; set; }
        public string? Venue { get; set; }
        public Decimal? Longitude { get; set; }
        public Decimal? Latitude { get; set; }
        public string? GeoLocation { get; set; }
        public string? StartDateTime { get; set; }
        public string? EndDateTime { get; set; }
        public int? TotalSeats { get; set; }
        public int? AllocatedSeats { get; set; }
        public Decimal? Price { get; set; }
        public string? WebinarLink { get; set; }
        public Int64? CouponIDF { get; set; }
        public long? SkillID{  get; set; } 
        public string? BookSkillButton {  get; set; } 
        public TimeSpan? StartTimeActual {  get; set; }
        public TimeSpan? EndTimeActual { get; set; }
        public string? ActivityInterestName {  get; set; }
        public long? VendorId { get; set; }
    }



    public partial class ActivityDetails
    {
        public Int64? ActivityIDP { get; set; }
        public Int64? InterestIDF { get; set; }
        public string? Banner { get; set; }
        public string? ActivityTitle { get; set; }
        public string? CategoryName { get; set; }
    }
}
