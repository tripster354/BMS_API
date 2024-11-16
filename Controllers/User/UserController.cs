using BMS_API.Models.DTOs;
using BMS_API.Models.User;
using BMS_API.Services;
using BMS_API.Services.Interface;
using BMS_API.Services.Interface.User;
using BMS_API.Services.User;
using BudgetManagement.Controllers;
using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using BudgetManagement.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Threading.Tasks;

namespace BMS_API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CommonController
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment webHostEnvironment;
        public UserController(BMSContext context, IUserService userService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _userService = userService;
            this.webHostEnvironment = webHostEnvironment;
        }
        #region Create-User
        [HttpPost]
        [Route("create-user")]
        public async Task<ActionResult> CreateUser(tblUser user)
        {
            try
            {
                var newUser = await _userService.CreateUserAsync(user);

                if (newUser == null)
                {
                    // Duplicate entry case
                    return Ok(new { status = 201, data = new object[] { }, message = "Mobile or Email are already registered" });
                }
                return Ok(new { statis = 200, UserDetails = newUser, message = "user created successfully." });
                // Successful creation case
                //return Ok(new
                //{
                //    status = 200,
                //    data = new
                //    {
                //        FullName = newUser.FullName,
                //        EmailID = newUser.EmailID,
                //        //LoginToken = newUser.LoginToken
                //    },
                //    message = "User successfully created"
                //});
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, data = new object[] { }, message = ex.Message });
            }
        }

#endregion CreateUser

#region Gell-All-Users
[HttpPost]
        [Route("user-get-all")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                GetAuth();
                if (objUser == null)
                {
                    return BadRequest(authFail);
                }

                var users = await _userService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return Ok(new { status = 1, data = users, message = "No users found" });
                }
                return Ok(new { status = 1, data = users, message = "Users retrived successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Get-User-ById
        [HttpPost]
        [Route("user-get-by-Id")]
        public async Task<ActionResult> GetUserById(int id)
        {
            try
            {
                GetAuth();
                if (objUser == null)
                {
                    return BadRequest(authFail);
                }
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    return Ok(new { status = 1, data = user, message = "No user found" });
                }
                return Ok(new { status = 1, data = user, message = "User retrived successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Update-User
        [HttpPost]
        [Route("user-update/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] tblUser user)
        {
            try
            {
                GetAuth();
                if (objUser == null)
                {
                    return BadRequest(authFail);
                }
                if (id != user.UserIDP)
                {
                    return BadRequest(new { status = 0, data = 0, message = "User Id mismatch" });
                }
                await _userService.UpdateUserAsync(id, user);


                return Ok(new { status = 1, data = 0, message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region user-delete
        [HttpDelete]
        [Route("user-delete/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            GetAuth();
            if (objUser == null)
            {
                return BadRequest(authFail);
            }

            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new { status = 1, data = 0, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region user-reviews
        [HttpGet]
        [Route("User-reviews/{activityId}")]
        public async Task<IActionResult> GetAllUserReviews(int activityId)
        {
            GetAuth();
            if (objUser == null)
            {
                return BadRequest(authFail);
            }

            try
            {
                var reviews = await _userService.GetUserReviewsAsync(activityId);
                if (reviews == null || !reviews.Any())
                {
                    return Ok(new { status = 1, data = reviews, message = "No reviews found" });
                }
                return Ok(new { status = 1, data = reviews, message = "Users reviews retrived successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region user-profile-update
        [HttpPost]
        [Route("user-profile-update")]
        public async Task<IActionResult> ProfileUpdateAsync([FromBody] UserProfileDTO userProfile)
        {
            GetAuth();
            if (objUser == null)
            {
                return BadRequest(authFail);
            }
            try
            {
                var isUpdated = await _userService.UpdateUserProfileAsync(userProfile);

                if (isUpdated)
                {
                    return Ok(new { status = 1, message = "Profile updated successfully" });
                }
                else
                {
                    return BadRequest(new { status = 0, message = "Profile update failed" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, message = ex.Message });
            }
        }
        #endregion
    }
}
