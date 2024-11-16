using System;

namespace BMS_API.Models
{
    public partial class Role
    {
        public Int64? RoleIDP { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
