namespace BMS_API.Models.DTOs
{
    public class TrendingSkillsDTO
    {
        public int SkillId {  get; set; }
        public string SkillImage { get; set; }
        public string SkillName { get; set; }
        public string StartDateTime { get; set; }
        public string Description { get; set; }
        public int? AllocatedSeats { get; set; }
        public int? TotalSeats { get; set; }
        public double? PricePerSession { get; set; }
        public int? RemainingSeats { get; set; }
        //public string GalleryImage { get; set; }
        public string ReviewText { get; set; }
        public int? Rating { get; set; }
        public int? TotalReviews { get; set; }
        public string PostDescription { get; set; }
        public int? Likes { get; set; }
        public int? Dislikes { get; set; }
        public string Comments { get; set; }
        public string StudentName { get; set; }
        public string StudentImage { get; set; }
        public int? TotalConnection { get; set; }
        public int? TotalBookings { get; set; }
        public int? TotalClicks {  get; set; }
        public int? TotalFavourites {  get; set; }
        public int? TrendRank { get; set; }

    }
}
