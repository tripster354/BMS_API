namespace BMS_API.Models.DTOs
{
    public class ClubLikePostDTO
    {
        public long ClubId {  get; set; }
        public long UserId { get; set; }
        public bool IsLiked { get; set; }

    }
}
