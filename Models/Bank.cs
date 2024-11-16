using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class Bank
    {

        public int BankIDP { get; set; }
        
        public string BankName { get; set; }
        public string BankImage { get; set; }
        public bool? IsActive { get; set; }
        public int? IsDeleted { get; set; }
    }
}
