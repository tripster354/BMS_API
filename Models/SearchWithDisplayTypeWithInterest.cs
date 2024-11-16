using System;

namespace BMS_API.Models
{
    public partial class SearchWithDisplayTypeWithInterest
    {
        public int DisplayTypeID { get; set; }
        public int InterestID { get; set; }
        public string? SearchKeyWord { get; set; }
        public Int64 ActivityID { get; set; }
    }
}
