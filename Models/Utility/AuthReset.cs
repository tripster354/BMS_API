using System;

namespace BudgetManagement.Models.Utility
{
    public class AuthLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Int32 UserType { get; set; }
        public string? RefrenceLink { get; set; }
    }
}
