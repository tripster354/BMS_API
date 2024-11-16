namespace BMS_API.Models.DTOs
{
    public class CommentDto
    {
        public long FeedID { get; set; }
        public string ProfileImage { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public string CreatedDate { get; set; }
    }
}
