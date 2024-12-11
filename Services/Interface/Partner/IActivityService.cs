using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models.Partner;
using BMS_API.Models;
using BMS_API.Models.DTOs;

namespace BMS_API.Services.Interface.Partner
{
    public interface IActivityService : ICommon
    {
        Task<Int64> Activity_Insert(Activity modelActivity);
        Task<Int64> Activity_Update(Activity modelActivity);
        Task<string> Activity_Get(Int64 activityIDP);
        // Task<string> Activity_GetAll(Int64 actvityStatus);
        Task<object> Activity_GetAll(int activityStatus, int pageSize, int pageNumber);
        Task<string> Activity_GetAll_ByUser(SearchWithDisplayTypeWithInterest param);
        Task<Int64> Activity_ApproveReject(Int64 activityIDP, Int32 isActive);
        //Task<string> Activity_DDL();
        Task<Int64> Activity_Delete(Int64 activityIDP);

        Task<Int64> Activity_Booked(ActivityBooked modelactivitybook);

        Task<Int64> Activity_Booked_Cancelled(ActivityBookedCancelled modelactivitybookcancelled);

        Task<string> Activity_GetAll_ByUserIDF(Search modelsearch);

        Task<string> Activity_GetAll_BySearch(ActivitySearch activitySearch);

        Task<string> Activity_Request_Get_FullDetail(Int64 activityIDP);

        Task<string> Activity_Coupon_GetAll_Vendor(ActivityCoupon activityenddate);

        Task<Int64> Activity_Update_Favourite_ByUser(Int64 activityIDP);

        Task<string> Activity_GetAll_Favourite(Search modelsearch);

        Task<Int64> Activity_Update_Follower(ModelFollowerUpdate modelFollowerUpdate);

        Task<string> Activity_GetAll_Follower();

        Task<string> Suggested_Offer_Activity_GetAll_ByUserIDF(Search modelsearch);

        Task<ActivityListDTO> GetActivityById(long userId);

        Task<object> GetAllSkillInfoAsync();

    }
}
