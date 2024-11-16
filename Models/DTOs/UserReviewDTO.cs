using System;

namespace BMS_API.Models.DTOs
{
    public class UserReviewDTO
    {
        public long ReviewID { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public DateTime ReviewDate { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
    }
}
