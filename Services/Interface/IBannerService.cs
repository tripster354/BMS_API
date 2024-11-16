using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface IBannerService : ICommon
    {
        Task<Int64> Banner_Insert(Banner modelBanner);
        Task<Int64> Banner_Update(Banner modelBanner);
        Task<string> Banner_Get(Int64 bannerIDP);
        Task<string> Banner_GetAll(ModelCommonGetAll param);
        Task<Int64> Banner_ActiveInactive(Int64 bannerIDP, Boolean isActive);
        Task<Int64> Banner_ApproveReject(Int64 bannerIDP, Int32 isApprove);
        //Task<string> Banner_DDL();
        Task<Int64> Banner_Delete(Int64 bannerIDP);
    }
}
