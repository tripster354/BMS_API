using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models.Partner;
using BMS_API.Models.User;
using BMS_API.Models;

namespace BMS_API.Services.Interface.Partner
{
    public interface ItblPartnerService : ICommon
    {
        Task<Int64> tblPartner_Registration(tblPartner modeltblPartner);
        //Task<Int64> tblPartner_Update(tblPartner modeltblPartner);
        Task<string> tblPartner_Get(Int64 partnerIDP);
        Task<string> tblPartner_GetAll(ModelCommonGetAll param);
        Task<Int64> tblPartner_ActiveInactive(Int64 partnerIDP, Boolean isActive);
        Task<Int64> tblPartner_ApproveReject(Int64 partnerIDP, Int32 isApprove);
        Task<Int64> tblPartner_Delete(Int64 partnerIDP);

        Task<Int64> Partner_Update_MyProfile(tblPartner modeltblPartner);

        Task<string> Partner_Get_Dashboard(Int64 partnerIDP, PartnerDashboard dashboard);

        Task<string> Partner_Get_All_Booking(Int64 partnerIDP, int displayTypeID);
        Task<string> Partner_Get_FullDetail_Booking(Int64 activityID);

        Task<Int64> Activity_UpdateActualTime(ActivityActualTime modelactivity);
        Task<Int64> Activity_UpdateAttendance(ActivityAttendace modelactivityattendace);

        Task<Int64> Feed_Insert(ModelFeed modelFeed, string connection);

        Task<Int64> Feed_Update(ModelFeed modelFeed);

        Task<string> Partner_Activity_Participate_List(SearchParticipateFeed param);

        Task<string> Partner_Activity_DDL();

        Task<string> Partner_Feed_Get_All(ModelCommonGetAll param);

        Task<Int64> Partner_Update_Feed_Comment(ModelFeedComment modelafeedcomment);

        Task<string> Partner_Feed_Get_All_Comment(ModelFeedCommentAll param);

        Task<Int64> Feed_Delete(Int64 feedIDP);

        Task<Int64> Feed_Update_Reaction(ModelFeedUpdateReaction modelfeed);

        Task<string> Partner_Feed_Get(Int64 FeedIDP);

        Task<string> Partner_Get_Club_Subscription_Status(Int64 clubIDP);
    }
}
