using BMS_API.Services.Interface.User;
using BMS_API.Services.Interface;
using BudgetManagement.Controllers;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models;
using BMS_API.Models.User;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BMS_API.Models.Utility;
using BMS_API.Models.Partner;
using BMS_API.Services.Interface.Partner;
using BMS_API.Services;
using BMS_API.Services.Partner;
using BMS_API.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Expressions;
using BMS_API.Models.DTOs;
using BMS_API.Services.User;

namespace BMS_API.Controllers.User
{
    [Route("[controller]")]
    [ApiController]

    public class ClubController : CommonController
    {
        private readonly IClubService _ClubService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ClubController(BMSContext context, IClubService __ClubService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _ClubService = __ClubService;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region INSERT-UPDATE
        [HttpPost]
        [Route("club-insert-update")]
        public async Task<IActionResult> Club_InsertUpdate([FromForm] Club modelClub)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ClubService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var file1 = files[0];
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "User");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    tempDirectoryPath = Path.Combine(directoryPath, "Temp");
                    if (!Directory.Exists(tempDirectoryPath))
                    {
                        Directory.CreateDirectory(tempDirectoryPath);
                    }
                    if (file1.Length > 0)
                    {
                        var fileType = Path.GetExtension(file1.FileName);
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file1.FileName);
                        using (var fileStream = new FileStream(Path.Combine(tempDirectoryPath, fileName), FileMode.Create))
                        {
                            await file1.CopyToAsync(fileStream);
                        }
                        modelClub.ClubBanner = fileName;
                    }
                }

                if (modelClub.ClubIDP > 0)
                {
                    paramIdentity = await _ClubService.Club_Update(modelClub);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _ClubService.Club_Insert(modelClub);
                    paramIdentityAction = msgInserted;
                }
                if (files.Count > 0 && paramIdentity > 0)
                {
                    // File move to particular folder
                    string newDirectoryPath = Path.Combine(directoryPath, objUser.UserID.ToString(), "Club");
                    if (!Directory.Exists(newDirectoryPath))
                    {
                        Directory.CreateDirectory(newDirectoryPath);
                    }
                    if (Directory.Exists(tempDirectoryPath))
                    {
                        foreach (var file in new DirectoryInfo(tempDirectoryPath).GetFiles())
                        {
                            file.MoveTo($@"{newDirectoryPath}\{file.Name}");
                        }
                    }
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Club_InsertUpdate", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE

        #region GET-ALL
        [HttpPost]
        [Route("trending-club-get-all")]
        public async Task<IActionResult> TrendingClub_GetAll()
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _ClubService.ObjUser = objUser;

            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ClubService.ObjUser = objUser;

                var strResponse = await _ClubService.TrendingClub_GetAll();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : TrendingClub_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL

        #region GET
        [HttpPost]
        [Route("club-get")]
        public async Task<IActionResult> Club_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ClubService.ObjUser = objUser;

                var strResponse = await _ClubService.Club_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Club_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET

        #region GET Club - skill - block
        [HttpPost]
        [Route("club-skill-block")]
        public async Task<IActionResult> GetClubBlockSkills()
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _ClubService.ObjUser = objUser;
            try
            {
                var clubSkills = await _ClubService.GetClubSkillsAsync();

                if (clubSkills == null || !clubSkills.Any())
                {
                    return Ok(new { status = 0, data = "No club skills found" });
                }
                return Ok(new { status = 1, data = clubSkills });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Club-skill-block", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }

        }
        #endregion

        #region Club - skill - booking
        [HttpPost]
        [Route("clubskill-booking")]
        public async Task<IActionResult> ClubSkillBooking([FromBody] ClubSkillsBookingRequest request)
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _ClubService.ObjUser = objUser;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Call the service method to book a skill
                var result = await _ClubService.ClubSkillBookAsync(request.ActivityID, request.UserID, request.SeatsBooked, request.BookingDate, request.PartnerID, request.ContactIDFs, request.EntryBy);

                if (result == null)
                {
                    return StatusCode(500, "An error occurred while processing the booking.");
                }

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"Controller : Club-skill-block", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Get - all - club - tutors
        [HttpGet]
        [Route("get-all-club-tutors")]
        public async Task<IActionResult> GetAllClubTutors()
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _ClubService.ObjUser = objUser;
            try
            {
                IEnumerable<ClubTutorDTO> tutors = await _ClubService.GetAllClubTutorsAsync();
                if (tutors == null)
                {
                    return StatusCode(500, "An error occurred while getting all club tutors");
                }
                return Ok(tutors);
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"Controller : All-club-tutors", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Get - club - tutor - ById
        [HttpGet("GetClubTutor/{tutorId}")]
        public async Task<IActionResult> GetClubTutorById(long tutorId)
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _ClubService.ObjUser = objUser;
            try
            {
                IEnumerable<ClubTutorDTO> tutors = await _ClubService.GetClubTutorByIdAsync(tutorId);
                if (tutors == null)
                {
                    return StatusCode(500, "An error occurred while getting club tutor");
                }
                return Ok(tutors);
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"Controller : Club-tutor-byId", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Club-like post
        [HttpPost]
        [Route("club-like-post")]
        public async Task<IActionResult> ClubLikePost([FromBody] ClubLikePostDTO likePostDTO)
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _ClubService.ObjUser = objUser;

            try
            {
                if (likePostDTO == null || likePostDTO.ClubId == 0 || likePostDTO.UserId == 0)
                {
                    return BadRequest("Invalid Input");
                }
                await _ClubService.LikeClubPostAsync(likePostDTO.ClubId, likePostDTO.UserId, likePostDTO.IsLiked);
                return Ok("Club post liked/unliked successfully");
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"Controller : club-like-post", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion

        #region Club-comment - post
        [HttpPost]
        [Route("club-comment-post")]
        public async Task<IActionResult> ClubCommentPost([FromBody] ClubCommentPostDTO clubComment)
        {
            GetAuth();
            if (objUser == null) return BadRequest(authFail);
            _ClubService.ObjUser = objUser;

            try
            {
                if (clubComment == null || clubComment.ClubId == 0 || clubComment.UserId == 0 || string.IsNullOrEmpty(clubComment.Comment))
                {
                    return BadRequest("Invalid input");
                }
                await _ClubService.CommentOnClubPostAsync(clubComment.ClubId, clubComment.UserId, clubComment.Comment);

                return Ok("comment added successfully");
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"Controller : club - comment - post", 1);
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }
        #endregion
    }
}