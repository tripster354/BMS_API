using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class TblCountry
    {
        public TblCountry()
        {
            Tblstate = new HashSet<Tblstate>();
        }

        public int CountryIdp { get; set; }
        public Guid? RowGuid { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryFlag { get; set; }
        public string CountryCurrency { get; set; }
        public int? Isdcode { get; set; }
        public bool? IsActive { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? EnteredDate { get; set; }

        public virtual ICollection<Tblstate> Tblstate { get; set; }
    }
}
