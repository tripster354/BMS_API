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
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http.Headers;
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
        public async Task<ActionResult> CreateUser([FromForm] tblUserRequestModel user)
        {
            try
            {
                string fileName = null;

                foreach (IFormFile source in user.ProfileImage)
                {
                    // Get original file name to get the extension from it.
                    string orgFileName = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName;

                    // Create a new file name to avoid existing files on the server with the same names.
                    // fileName = DateTime.Now.ToFileTime() + Path.GetExtension(orgFileName);
                    fileName = DateTime.Now.Second + orgFileName;


                    string fullPath = GetFullPathOfFile(fileName.Replace("\"", ""));

                    // Create the directory.
                    Directory.CreateDirectory(Directory.GetParent(fullPath).FullName);

                    // Save the file to the server.
                    await using FileStream output = System.IO.File.Create(fullPath);
                    await source.CopyToAsync(output);
                }

                var response = new { FileName = fileName.Replace("\"", "") };



                tblUser tableUser = new tblUser();
                tableUser.RowGUID = user.RowGUID;
                tableUser.UserIDP = user.UserIDP;
                tableUser.FirstName = user.FirstName;
                tableUser.LastName = user.LastName;
                tableUser.FullName = user.FullName;
                tableUser.MobileNo = user.MobileNo;
                tableUser.EmailID = user.EmailID;
                tableUser.Password = user.Password;
                tableUser.ProfileImage = response.FileName;
                tableUser.Address = user.Address;
                tableUser.RegistrationDate = user.RegistrationDate;
                tableUser.SocialGoogle = user.SocialGoogle;
                tableUser.SocialFacebook = user.SocialFacebook;
                tableUser.SocialLinkedIn = user.SocialLinkedIn;
                tableUser.SocialInstagram = user.SocialInstagram;
                tableUser.SocialTweeter = user.SocialTweeter;
                tableUser.SocialTelegram = user.SocialTelegram;
                tableUser.SocialOther = user.SocialOther;
                tableUser.AboutMe = user.AboutMe;
                tableUser.TotalFollowers = user.TotalFollowers;
                tableUser.TotalConnection = user.TotalConnection;
                tableUser.RefrenceLink = user.RefrenceLink;
                tableUser.RefrenceLinkUsed = user.RefrenceLinkUsed;
                tableUser.IsActive = user.IsActive;
                tableUser.OTP = user.OTP;
                tableUser.LoginToken = user.LoginToken;
                tableUser.InterestIDs = user.InterestIDs;
                tableUser.EntryBy = user.EntryBy;
                tableUser.EntryDate = user.EntryDate;
                tableUser.IsDeleted = user.IsDeleted;

                var newUser = await _userService.CreateUserAsync(tableUser);

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

        private string GetFullPathOfFile(string fileName)
        {
            return $"{this.webHostEnvironment.WebRootPath}\\Uploads\\{fileName}";
        }

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
        public async Task<ActionResult> UpdateUser(int id, [FromBody] tblUserRequestModel user)
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

                string fileName = null;

                foreach (IFormFile source in user.ProfileImage)
                {
                    // Get original file name to get the extension from it.
                    string orgFileName = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName;

                    // Create a new file name to avoid existing files on the server with the same names.
                    // fileName = DateTime.Now.ToFileTime() + Path.GetExtension(orgFileName);
                    fileName = DateTime.Now.Second + orgFileName;


                    string fullPath = GetFullPathOfFile(fileName.Replace("\"", ""));

                    // Create the directory.
                    Directory.CreateDirectory(Directory.GetParent(fullPath).FullName);

                    // Save the file to the server.
                    await using FileStream output = System.IO.File.Create(fullPath);
                    await source.CopyToAsync(output);
                }

                var response = new { FileName = fileName.Replace("\"", "") };



                tblUser tableUser = new tblUser();
                tableUser.RowGUID = user.RowGUID;
                tableUser.UserIDP = user.UserIDP;
                tableUser.FirstName = user.FirstName;
                tableUser.LastName = user.LastName;
                tableUser.FullName = user.FullName;
                tableUser.MobileNo = user.MobileNo;
                tableUser.EmailID = user.EmailID;
                tableUser.Password = user.Password;
                tableUser.ProfileImage = response.FileName;
                tableUser.Address = user.Address;
                tableUser.RegistrationDate = user.RegistrationDate;
                tableUser.SocialGoogle = user.SocialGoogle;
                tableUser.SocialFacebook = user.SocialFacebook;
                tableUser.SocialLinkedIn = user.SocialLinkedIn;
                tableUser.SocialInstagram = user.SocialInstagram;
                tableUser.SocialTweeter = user.SocialTweeter;
                tableUser.SocialTelegram = user.SocialTelegram;
                tableUser.SocialOther = user.SocialOther;
                tableUser.AboutMe = user.AboutMe;
                tableUser.TotalFollowers = user.TotalFollowers;
                tableUser.TotalConnection = user.TotalConnection;
                tableUser.RefrenceLink = user.RefrenceLink;
                tableUser.RefrenceLinkUsed = user.RefrenceLinkUsed;
                tableUser.IsActive = user.IsActive;
                tableUser.OTP = user.OTP;
                tableUser.LoginToken = user.LoginToken;
                tableUser.InterestIDs = user.InterestIDs;
                tableUser.EntryBy = user.EntryBy;
                tableUser.EntryDate = user.EntryDate;
                tableUser.IsDeleted = user.IsDeleted;



                await _userService.UpdateUserAsync(id, tableUser);


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
