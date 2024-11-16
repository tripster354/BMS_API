using System;

namespace BudgetManagement.Models.Utility
{
    public class ModelActiveInactive
    {
        /// <summary>
        /// Any main primary / forgine key in which user is right now
        /// </summary>
        public Int64 entityID { get; set; }

        public Boolean isActive { get; set; }
    }
}
