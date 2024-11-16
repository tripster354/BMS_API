using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;
using BMS_API.Models.DTOs;
using System.Collections.Generic;

namespace BMS_API.Services.Interface
{
    public interface IDashboardService : ICommon
    {
        Task<string> Dashboard_Detail();
        Task<string> GetUpcomingActivities();
        //Task<object> GetAllTrendingSkills(Int64 skills);
        Task<object> GetAllTrendingSkillsAsync(Int64 activityStatus);
        Task<string> TrendingSkillLikeDislikes(long skillId, long userId, bool likeStatus);
        Task<string> BookActivity(long activityId, long userId, int seatCount, string contactIdFs, int bookingStatus = 1);
        Task<List<BannerDTO>> GetBannersAsync();
        Task<List<SkillDTO>> GetInterestedSkillsAsync();
        Task<List<SuggestedOffersDTO>> GetSuggestedOffersAsync(int status, float? discountedPrice = null);
        Task<List<TrendingTeachersDTO>> GetTrendingTeachersAsync();
        Task<List<ClubDTO>> GetTrendingClubsAsync();
        Task<List<StudentDTO>> GetStudentlISTAsync();
        Task<List<FollowerDTO>> GetFollowerListAsync(long userId); 
        Task<List<ConnectionUserDTO>> GetSuggestedConnectionsAsync(long userId);
        Task<string> FollowUnfollowUserAsync(long userId, long followedUserId, int actionType);

        public Task<(IEnumerable<ParticipentInfoDTO> Participants, int TotalBookings, int TotalActivities, int TotalPartners)> GetParticipantInfoAsync();

    }
}
