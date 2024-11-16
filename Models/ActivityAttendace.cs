using System;

namespace BMS_API.Models
{
    public partial class ActivityAttendace
    {
        public Int64? ActivityIDF { get; set; }
        public Int64? UserIDF { get; set; }
        public Int64? ContactIDF { get; set; }
        public Int16? PresentStatus { get; set; }

    }
}
