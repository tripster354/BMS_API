using System;

namespace BMS_API.Models.DTOs
{
    public class FeedDto
    {
        public long FeedID { get; set; }
        public bool IsOwner { get; set; }
        public string BannerImage { get; set; }
        public string Title { get; set; }
        public long ActivityID { get; set; }
        public string NumberLike { get; set; }
        public string NumberSmile { get; set; }
        public string NumberBook { get; set; }
        public string NumberHeart { get; set; }
        public string NumberNeutral { get; set; }
        public string NumberStar { get; set; }
        public string NumberFavourite { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public string ActivityTitle { get; set; }
        public long InterestID { get; set; }

        // Reaction counts
        public int FeedLike { get; set; }
        public int FeedSmile { get; set; }
        public int FeedBook { get; set; }
        public int FeedHeart { get; set; }
        public int FeedNeutral { get; set; }
        public int FeedStar { get; set; }
        public int FeedFavourite { get; set; }

        // Activity owner details
        public string ActivityOwnerName { get; set; }
        public string ActivityOwnerProfileImage { get; set; }
    }
}
