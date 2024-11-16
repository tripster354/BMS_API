using System;

namespace BMS_API.Models.Utility
{
    public class ModelApproveReject
    {
        /// <summary>
        /// Any main primary / forgine key in which user is right now
        /// </summary>
        public Int64 entityID { get; set; }

        public Int32 isApprove { get; set; }
    }
}
