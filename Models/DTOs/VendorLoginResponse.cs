namespace BMS_API.Models.DTOs
{
    public class VendorLoginResponse
    {
        public string Message { get; set; }
        public long UserID {  get; set; }
        public string FullName {  get; set; }
        public string Email {  get; set; }
        public string MobileNumber {  get; set; }
        public int UserType {  get; set; }
        public string CurrentToken {  get; set; }
        public string InstaLink {  get; set; }
        public string TwitterLink {  get; set; }
        public string LinkedInLink {  get; set; }
        public string Website {  get; set; }
        public string ActivityInterestName {  get; set; }
        public int YearsOfExperience {  get; set; }
    }
}
