using System;

namespace BMS_API.Models
{
    public partial class Banner
    {
        public Int64? BannerIDP { get; set; }
        public string? BannerName { get; set; }
        public Int32? BannerTypeID { get; set; }
        public Int32? UserTypeID { get; set; }
        public string? Attachment { get; set; }
        public string? BannerURL { get; set; }
        public Int32? PositionID { get; set; }
        public string? CityIDFs { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
