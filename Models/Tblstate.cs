using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class Tblstate
    {
        public Tblstate()
        {
            TblCity = new HashSet<TblCity>();
        }

        public int StateIdp { get; set; }
        public Guid RowGuid { get; set; }
        public string StateName { get; set; }
        public int? CountryIdf { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime EnteredDate { get; set; }

        public virtual TblCountry CountryIdfNavigation { get; set; }
        public virtual ICollection<TblCity> TblCity { get; set; }
    }
}
