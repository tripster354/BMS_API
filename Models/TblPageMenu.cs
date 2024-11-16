using System;

namespace BMS_API.Models
{
    public partial class TblPageMenu
    {
        public int PageMenuIDP { get; set; }
        public Guid RowGuid { get; set; }
        public string TitleMenu { get; set; }
        public string TitlePage { get; set; }
        public string PageContent { get; set; }
        public int Sequence { get; set; }
        public int ActiveType { get; set; }
        public int? PageMenuIDF { get; set; }
        public int? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
