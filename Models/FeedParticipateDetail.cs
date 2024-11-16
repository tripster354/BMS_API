namespace BMS_API.Models
{
    public class FeedParticipateDetail
    {
        public long UserID { get; set; }          // Represents the UserID
        public long ContactID { get; set; }       // Represents the ContactID
        public string? Comment { get; set; }      // Optional comment for the participant
        public int FeedLike { get; set; }         // Represents the likes on the feed
        public int FeedSmile { get; set; }        // Represents the smile reactions
        public int FeedBook { get; set; }         // Represents the book reactions
        public int FeedHeart { get; set; }        // Represents the heart reactions
        public int FeedNetural { get; set; }      // Represents neutral reactions
        public int FeedStar { get; set; }         // Represents star ratings
        public int FeedFavourite { get; set; }    // Represents favourite markings
    }
}
