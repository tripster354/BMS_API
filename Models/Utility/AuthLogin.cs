using System;

namespace BudgetManagement.Models.Utility
{
    public class AuthReset
    {
        public Guid RowGUID { get; set; }
        public Byte UserType { get; set; }
        public string NewPassword { get; set; }
    }
}
