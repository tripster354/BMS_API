namespace BMS_API.Models.DTOs
{
    public class ClubCommentPostDTO
    {
        public long ClubId { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }
    }
}
