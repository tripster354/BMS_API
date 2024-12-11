namespace BMS_API.Models
{
    public class Posts
    {
        public long PostID { get; set; }
        public int? LikeStatus { get; set; }
        public string SkillName { get; set; }
        public string PostImage { get; set; }
        public string PostDescription { get; set; }
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
    }


    public class PostsDetails
    {
        public long PostID { get; set; }
        public int? LikeStatus { get; set; }
        public string PostImage { get; set; }
    }
}
