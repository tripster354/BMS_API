namespace BMS_API.Models
{
    public class LikeDislikeRequest
    {
        public long SkillId {  get; set; }
        public long UserId {  get; set; }
        public bool LikeStatus {  get; set; }
    }
}
