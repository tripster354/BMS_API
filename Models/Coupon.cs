using System;

namespace BMS_API.Models
{
    public partial class Coupon
    {
        public Int64? CouponIDP { get; set; }
        public string? PromoCode { get; set; }
        public string? Attachment { get; set; }
        public Int32? DiscountTypeID { get; set; }
        public float? DiscountValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CouponCode { get; set; }
        public Int64? CouponCodeGenerateNo { get; set; }
        public bool? IsActive { get; set; }
    }
}
