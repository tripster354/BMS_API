using System;
using System.Collections.Generic;
using BMS_API.Services.Interface;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class AuthorisedUser
    {
        public Int64 UserID { get; set; }
        public string PersonName { get; set; }
        public ICommon.UserType UserType { get; set; }
        public Int64 CurrentToken { get; set; }
    }
}
