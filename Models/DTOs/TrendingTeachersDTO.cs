namespace BMS_API.Models.DTOs
{
    public class TrendingTeachersDTO
    {
        public long TeacherId { get; set; }
        public string ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public int TotalSkillsPublished { get; set; }  // M
        public long TotalBookings { get; set; }  // N
        public int TotalPosts { get; set; }  // L
        public int TrendingScore { get; set; }  // Z = M + N + L
        public string ProfileImagePath {  get; set; }
    }
}
