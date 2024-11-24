namespace BMS_API.Models.DTOs
{
    public class VendorLoginRequest
    {
        public string LoginInput { get; set; }
        public int Password {  get; set; }
    }

    public class VendorSendOtpRequest
    {
        public string MobileNumber { get; set; }
        //public int Otp { get; set; }
    }
}
