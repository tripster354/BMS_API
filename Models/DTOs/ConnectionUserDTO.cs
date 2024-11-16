using System.Numerics;

namespace BMS_API.Models.DTOs
{
    public class ConnectionUserDTO
    {
        public long UserIDP {  get; set; }
        public string FullName {  get; set; }
        public string ProfileImage { get; set; }
    }
}
