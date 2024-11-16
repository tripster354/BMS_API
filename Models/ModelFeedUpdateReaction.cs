using System;

namespace BMS_API.Models
{
    public partial class ModelFeedUpdateReaction
    {
        public Int64? FeedIDF { get; set; }

        public Int64? UserIDF { get; set; }
        public int? FeedLike { get; set; }

        public int? FeedSmile { get; set; }
        public int? FeedBook { get; set; }
        public int? FeedHeart { get; set; }
        public int? FeedNetural { get; set; }
        public int? FeedStar { get; set; }
        public int? FeedFavourite { get; set; }



    }
}
