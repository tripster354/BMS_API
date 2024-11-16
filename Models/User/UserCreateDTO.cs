namespace BMS_API.Models.User
{
    public class UserCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
    }
}
