namespace BMS_API.Models
{
    public class FollowUnfollowRequest
    {
        public long UserIDP {  get; set; }
        public long FollowedUserIDP {  get; set; }
        public int ActionType {  get; set; }
    }
}
