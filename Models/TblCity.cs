using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class TblCity
    {
        public int CityIdp { get; set; }
        public Guid? RowGuid { get; set; }
        public string CityName { get; set; }
        public int? StateIdf { get; set; }
        public Int64? CountryIdf { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? EnteredDate { get; set; }

        public virtual Tblstate StateIdfNavigation { get; set; }
    }
}
