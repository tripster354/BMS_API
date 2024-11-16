using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface IInterestService : ICommon
    {
        Task<Int64> Interest_Insert(Interest modelInterest);
        Task<Int64> Interest_Update(Interest modelInterest);
        Task<string> Interest_Get(Int64 interestIDP);
        Task<string> Interest_GetAll(ModelCommonGetAll param);
        Task<Int64> Interest_ActiveInactive(Int64 interestIDP, Boolean isActive);
        Task<string> Interest_DDL();
        Task<Int64> Interest_Delete(Int64 interestIDP);
    }
}
