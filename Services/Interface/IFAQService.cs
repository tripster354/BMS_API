using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface IFAQService : ICommon
    {
        Task<Int64> FAQ_Insert(FAQ modelFAQ);
        Task<Int64> FAQ_Update(FAQ modelFAQ);
        Task<string> FAQ_Get(Int64 fAQIDP);
        Task<string> FAQ_GetAll(ModelCommonGetAll param);
        Task<Int64> FAQ_ActiveInactive(Int64 fAQIDP, Boolean isActive);
        Task<string> FAQ_DDL();
        Task<Int64> FAQ_Delete(Int64 fAQIDP);
    }
}
