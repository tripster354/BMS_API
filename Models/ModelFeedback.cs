using System;
using System.Collections.Generic;

namespace BMS_API.Models
{
    public class ModelFeedback
    {
        public Int64? FeedIDP { get; set; }
        public Int64? PartnerIDF { get; set; } 
        public Int64? UserIDF { get; set; } 
        public string? BannerAttachment { get; set; } 
        public string? Title { get; set; } 
        public Int64? ActivityIDF { get; set; } 
        public int NumberLike { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public string? ParticipateList { get; set; } 
        public string? Description {  get; set; }
        public string? Category {  get; set; }

        public List<FeedParticipateDetail> ActData { get; set; } = new List<FeedParticipateDetail>(); // For the details of participants
        public int EntryBy { get; set; } // To track who is adding the feed
        public string? Comment { get; set; } // Optional comment
        public int FeedLike { get; set; } = 0; // Default value
        public int FeedSmile { get; set; } = 0; // Default value
        public int FeedBook { get; set; } = 0; // Default value
        public int FeedHeart { get; set; } = 0; // Default value
        public int FeedNetural { get; set; } = 0; // Default value
        public int FeedStar { get; set; } = 0; // Default value
        public int FeedFavourite { get; set; } = 0; // Default value
    }
}
