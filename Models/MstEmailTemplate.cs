using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BudgetManagement.Models
{
    public partial class MstEmailTemplate
    {
        public int EmailTemplateIdp { get; set; }
        public Guid RowGuid { get; set; }
        public string TemplateName { get; set; }
        public bool? IsEmail { get; set; }
        public bool? IsPush { get; set; }
        public bool? IsSms { get; set; }
        public string EmailSubject { get; set; }
        public string EmailContent { get; set; }
        public string PushContent { get; set; }
        public string Smscontent { get; set; }
        public string TemplateVarible { get; set; }
        public string? ReceiverID { get; set; }
        public bool? IsActive { get; set; }
        public int? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
