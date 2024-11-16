using BMS_API.Models.DTOs;
using BMS_API.Models.User;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS_API.Services.Interface.User
{
    public interface IUserService
    {
        Task<string> GetAllUsersAsync();
        Task<string> GetUserByIdAsync(int id);
        Task<tblUser> CreateUserAsync(tblUser user);
        Task UpdateUserAsync(int id, tblUser user);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<UserReviewDTO>> GetUserReviewsAsync(int activityId);
        Task<bool> UpdateUserProfileAsync(UserProfileDTO userProfileDTO);

    }
}
