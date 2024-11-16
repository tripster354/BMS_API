using System;

namespace BMS_API.Models
{
    public partial class SubAdmin
    {
        public Int64? SubAdminIDP { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public Int64? RoleIDF { get; set; }
        public bool? IsActive { get; set; }
        public string? tblPermission { get; set; }
    }
}
