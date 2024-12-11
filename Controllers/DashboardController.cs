using BMS_API.Services.Interface;
using BudgetManagement.Controllers;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models;
using BMS_API.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;

namespace BMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class DashboardController : CommonController
    {
        private readonly IDashboardService _DashboardService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DashboardController(BMSContext context, IDashboardService __DashboardService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _DashboardService = __DashboardService;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region Stats
        [Route("dashboard-detail")]
        [HttpPost]
        public async Task<IActionResult> Dashboard_Detail()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _DashboardService.ObjUser = objUser;

                var strResponse = await _DashboardService.Dashboard_Detail();

                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await _DashboardService.ErrorLog(1, e.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion Stats

        #region Dashboard-All-UpcomingActivities
        [Route("dashboard-upcoming-activities")]
        [HttpPost]
        public async Task<IActionResult> GetUpcomingActivities()
        {

            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _DashboardService.ObjUser = objUser;

            try
            {
                var strResponse = await _DashboardService.GetUpcomingActivities();

                return Ok(new { status = 1, data = strResponse });
            }
            catch (Exception e)
            {
                await _DashboardService.ErrorLog(1, e.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion



        #region TrendingSkills
        [Route("dashboard-trending-skills")]
        [HttpPost]
        public async Task<IActionResult> GetAllTrendingSkills(Int64 actvityStatus)
        {
            //GetAuth();
            //if (objUser == null) return BadRequest(authFail);
            //_DashboardService.ObjUser = objUser;

            try
            {
                dynamic response = await _DashboardService.GetAllTrendingSkillsAsync(actvityStatus);
                if (response.data == null || !((IEnumerable<object>)response.data).Any())
                {
                    return Ok(new { status = 201, data = "No data found" });
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                await _DashboardService.ErrorLog(201, e.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion

        #region TrendingSkills
        [Route("dashboard-skills-details")]
        [HttpPost]
        public async Task<IActionResult> GetSkillDetails()
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _DashboardService.ObjUser = objUser;

            try
            {
                dynamic response = await _DashboardService.SkillsDetailsAsync(objUser.UserID);
                if (response.data == null || !((IEnumerable<object>)response.data).Any())
                {
                    return Ok(new { status = 201, data = "No data found" });
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                await _DashboardService.ErrorLog(201, e.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion



        #region SkillsDetailsById
        [Route("dashboard-skills-details-By-Id")]
        [HttpPost]
        public async Task<IActionResult> SkillsDetailsById(long ActivityId)
        {
            //GetAuth();
            //if (objUser == null) return BadRequest(authFail);
            //_DashboardService.ObjUser = objUser;

            try
            {
                dynamic response = await _DashboardService.SkillsDetailsByIdAsync(ActivityId);
                if (response.data == null || !((IEnumerable<object>)response.data).Any())
                {
                    return Ok(new { status = 201, data = "No data found" });
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                await _DashboardService.ErrorLog(201, e.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion


        #region Skill-Likes/Dislikes
        [Route("dashboard-skills-likes-dislikes")]
        [HttpPost]
        public async Task<IActionResult> SkillLikeDislke([FromBody] LikeDislikeRequest request)
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _DashboardService.ObjUser = objUser;

            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid request data");
                }

                var result = await _DashboardService.TrendingSkillLikeDislikes(request.SkillId, request.UserId, request.LikeStatus);

                if (result.Contains("Error"))
                {
                    return StatusCode(500, result);
                }
                return Ok(new { status = 1, data = 1, message = result });
            }
            catch (Exception e)
            {
                await _DashboardService.ErrorLog(1, e.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 1, data = 0, message = e.Message });
            }
        }
        #endregion

        #region Dashboar-BookActivity
       
        [HttpPost]
        [Route("dashboard-skills-book")]
        public async Task<IActionResult> BookActivity([FromBody] BookingActivityRequest request)
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _DashboardService.ObjUser = objUser;

            try
            {
                if (request == null || request.ActivityId <= 0 || request.UserId <= 0 || request.SeatCount <= 0 || string.IsNullOrEmpty(request.ContactIDFs))
                {
                    return BadRequest("Invalid request data");
                }

                var result = await _DashboardService.BookActivity(request.ActivityId, request.UserId, request.SeatCount, request.ContactIDFs);
                if (result.Contains("Error"))
                {
                    return StatusCode(500, result);
                }
                return Ok(new { StatusCode = 1, data = 1, message = result });
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 1, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Dashboard-Banners-GetAll
        [Route("dashboard-banners-all")]
        [HttpPost]
        public async Task<IActionResult> GetBanners()
        {
            GetAuth();
            if (objUser == null) return BadRequest("banners not found");
            _DashboardService.ObjUser = objUser;

            try
            {
                var banners = await _DashboardService.GetBannersAsync();
                if (banners == null || !banners.Any())
                {
                    return NotFound(new { StatusCode = 404, message = "No banners found" });
                }
                return Ok(new { statusCode = 200, data = banners });
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 1, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Dashboard - Skill-Interests
        [Route("dashboard-skill-interested")]
        [HttpPost]
        public async Task<IActionResult> GetAllSkillInterested()
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;

            try
            {
                var skils = await _DashboardService.GetInterestedSkillsAsync();
                if (skils == null || !skils.Any())
                {
                    return NotFound(new { StatusCode = 404, message = "No interested skills found" });
                }
                return Ok(new { status = 200, data = skils });
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 1, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Dashboard - Suggested offers
        [Route("dashboard-suggested-offers")]
        [HttpGet]
        public async Task<IActionResult> GetSuggestedOffers([FromQuery] int activityStatus, [FromQuery] float? discountedPrice = null)
        {
            //GetAuth();
            //if (objUser == null) return BadRequest("Interested skiils not found");
            //_DashboardService.ObjUser = objUser;
            try
            {
                var suggestedOffers = await _DashboardService.GetSuggestedOffersAsync(activityStatus, discountedPrice);

                if (suggestedOffers == null || suggestedOffers.Count == 0)
                {
                    return NotFound("No suggested offers found");
                }
                return Ok(suggestedOffers);
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Dashboard-GetAll-TrendingTeachers
        [Route("dashboard-trending-teachers")]
        [HttpGet]
        public async Task<IActionResult> GetTrendingTeachers()
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;

            try
            {
                var trendingTeachers = await _DashboardService.GetTrendingTeachersAsync();
                if (trendingTeachers == null || !trendingTeachers.Any())
                {
                    return NotFound("No trending teachers available");
                }
                return Ok(trendingTeachers);
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Dashboard - GetAll - TrendingClubs
        [Route("Dashboard-trending-clubs")]
        [HttpGet]
        public async Task<IActionResult> GetTrendingClubs()
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;
            try
            {
                var trendingClubs = await _DashboardService.GetTrendingClubsAsync();
                if (trendingClubs == null || !trendingClubs.Any())
                {
                    return NotFound("No trending clubs available");
                }
                return Ok(trendingClubs);
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Dashboard - GetStudents-List
        [Route("Dashboard-students-list")]
        [HttpGet]
        public async Task<IActionResult> GetStudentList()
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;

            try
            {
                var students = await _DashboardService.GetStudentlISTAsync();
                if (students == null || !students.Any())
                {
                    return NotFound("No students found");
                }
                return Ok(students);
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Dashboard - student - follwers List
        [Route("dashboard-followers-list")]
        [HttpGet]
        public async Task<IActionResult> GetFollowersList(long studentId)
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;

            try
            {
                var followers = await _DashboardService.GetFollowerListAsync(studentId);
                if (followers == null || !followers.Any())
                {
                    return NotFound("No followers found");
                }
                return Ok(followers);
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, error = ex.Message });
            }
        }
        #endregion

        #region Dashboard- suggested - connections
        [Route("dashboard-suggested-connections")]
        [HttpGet]
        public async Task<IActionResult> GetSuggestedConnections(long userID)
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;
            try
            {
                var suggestedConnections = await _DashboardService.GetSuggestedConnectionsAsync(userID);
                if (suggestedConnections == null || !suggestedConnections.Any())
                {
                    return NotFound("No suggested connections found");
                }
                return Ok(suggestedConnections);
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Dashboard_Detail", 1);
                return BadRequest(new { status = 0, data = 0, error = ex.Message });
            }
        }
        #endregion

        #region Dashboard - follow-unfollow-users
        [Route("follow-unfollow-user")]
        [HttpPost]
        public async Task<IActionResult> FollowUnfollowUser([FromBody] FollowUnfollowRequest request)
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;

            if (request == null || request.UserIDP == 0 || request.FollowedUserIDP == 0)
            {
                return BadRequest("Invalid UserIds provided");
            }
            if (request.ActionType != 1 && request.ActionType != 0)
            {
                return BadRequest("Invalid action type.  Use 1 for follow and 0 for unfollow");
            }
            string result = await _DashboardService.FollowUnfollowUserAsync(request.UserIDP, request.FollowedUserIDP, request.ActionType);
            if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new { message = request.ActionType == 1 ? "User followed successfully" : "User unfollowed successfully" });
            }
            else
            {
                return StatusCode(500, "An error occurred while processing the request");
            }
        }
        #endregion

        #region Participant - Info 
        [HttpGet]
        [Route("participant - info")]
        public async Task<IActionResult> GetParticipantInfo()
        {
            GetAuth();
            if (objUser == null) return BadRequest("Interested skiils not found");
            _DashboardService.ObjUser = objUser;

            try
            {
                var (participants, totalBookings, totalActivities, totalPartners) = await _DashboardService.GetParticipantInfoAsync();
                if (participants == null || !participants.Any())
                {
                    return NotFound("No participants found");
                }
                return Ok(new
                {
                    status = 1,
                    message = "Participants info retrived successfully",
                    data = new
                    {
                        participants,
                        totalBookings,
                        totalActivities,
                        totalPartners
                    }
                });
            }
            catch (Exception ex)
            {
                await _DashboardService.ErrorLog(1, ex.Message, $"Controller : Participant_detail", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Get-User
        [HttpGet]
        [Route("Get-User")]
        public async Task<IActionResult> GetUser()
        {
            /* GetAuth();//*
            if (objUser == null) return BadRequest("User not found");
            //_DashboardService.ObjUser = objUser;*/
            try
            {
                var User = await _DashboardService.User_Get();
                if (User == null || !User.Any())
                {
                    return NotFound("No User found");
                }
                return Ok(new
                {
                    status = 1,
                    message = "User info retrived successfully",
                    data = new
                    {
                        User,
                    }
                });
            }
            catch (Exception ex)
            {

            }

            return View();
        }
        #endregion
    }
}