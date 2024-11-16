namespace BMS_API.Models.DTOs
{
    public class FollowerDTO
    {
        public long FollowerID { get; set; }
        public string FullName { get; set; }
        public string ProfileImage {  get; set; }
        public int TotalFollowers {  get; set; }
    }
}
