namespace BMS_API.Models.User
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string ProfileImage { get; set; }
        public string SkillNumber { get; set; }
        public string TotalFollower { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
