using BMS_API.Models.Partner;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models;
using BMS_API.Models.User;
using BMS_API.Models;
using System.Collections.Generic;
using BMS_API.Models.DTOs;

namespace BMS_API.Services.Interface.User
{
    public interface IClubService : ICommon
    {
        Task<Int64> Club_Insert(Club modelClub);
        Task<Int64> Club_Update(Club modelClub);
        Task<string> Club_Get(Int64 clubIDP);
        Task<string> TrendingClub_GetAll();
        Task<List<ClubSkillDTO>> GetClubSkillsAsync();
        Task<string> ClubSkillBookAsync(long activityId, long userId, int seatsBooked, DateTime bookingDate, long partnerId, string contactIdFs, int entryBy);
        Task<IEnumerable<ClubTutorDTO>> GetAllClubTutorsAsync();
        Task<IEnumerable<ClubTutorDTO>>GetClubTutorByIdAsync(long tutorId);
        Task LikeClubPostAsync(long clubId, long userId, bool IsLiked);
        Task CommentOnClubPostAsync(long clubId, long userId, string comment);

    }
}
