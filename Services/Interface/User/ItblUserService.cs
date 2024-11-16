using BMS_API.Models.Partner;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models;
using BMS_API.Models.User;
using BMS_API.Models;

namespace BMS_API.Services.Interface.User
{
    public interface ItblUserService : ICommon
    {
        //Task<string> tblUser_Get(Int64 userIDP);
        Task<string> tblUser_GetAll(ModelCommonGetAll param);
        Task<Int64> tblUser_ActiveInactive(Int64 userIDP, Boolean isActive);

        Task<string> tblUser_Get(Int64 userIDP);
        Task<Int64> User_Update_MyProfile(tblUser modelUser);
        //Task<Int64> tblUser_ApproveReject(Int64 userIDP, Int32 isApprove);
        //Task<Int64> tblUser_Delete(Int64 userIDP);

        Task<string> User_Get_FullDetail(Int64 activityID);

        Task<Int64> User_Update_Interest(MyInterest myinterest);

        Task<string> User_GetAll_Interest();

        Task<string> User_GetAll_Banner();

        Task<Int64> User_Insert_Contact(ContactList contactList, string connection);

        Task<string> User_Activity_GetAll(SearchWithDisplayType searchWithDisplayType);

        Task<string> User_ContactList_GetAll(Search param);

        Task<Int64> User_Feed_Insert(ModelFeed modelFeed, string connection);
        Task<Int64> User_Feed_Update(ModelFeed modelFeed);

        Task<string> User_Activity_DDL();

        Task<string> User_Feed_Get_All(ModelCommonGetAll param);

        Task<Int64> User_Update_Feed_Comment(ModelFeedComment modelafeedcomment);

        Task<string> User_Feed_Get_All_Comment(ModelFeedCommentAll param);

        Task<Int64> User_Feed_Delete(Int64 feedIDP);

        Task<Int64> User_Feed_Update_Reaction(ModelFeedUpdateReaction modelfeed);

        Task<string> User_Feed_Get(Int64 FeedIDP);

        Task<string> User_Get_All_Trending_Tutor();

        Task<string> User_Get_All_Suggested_User();

        Task<string> User_Get_Club_Subscription_Status(Int64 clubIDP);

    }
}
