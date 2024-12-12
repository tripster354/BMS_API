using BMS_API.Models.User;
using BMS_API.Services.Interface.Partner;
using BMS_API.Services.Interface;
using BudgetManagement.Controllers;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models;
using BMS_API.Models.Partner;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using BMS_API.Models.Utility;
using BMS_API.Models;
using BMS_API.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net.Http.Headers;

namespace BMS_API.Controllers.Partner
{
    [Route("[controller]")]
    [ApiController]

    public class ActivityController : CommonController
    {
        private readonly IActivityService _ActivityService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ActivityController(IWebHostEnvironment hostEnvironment, BMSContext context, IActivityService __ActivityService, IWebHostEnvironment webHostEnvironment, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _ActivityService = __ActivityService;
            this.webHostEnvironment = webHostEnvironment;
            _hostEnvironment = hostEnvironment;
        }

        #region ADMIN-PUSH-BANNER
        [HttpPost]
        [Route("admin-push-banner")]
        public async Task<IActionResult> AdminPushBanner([FromForm] Activity modelActivity)
        {
            try
            {
                //GetAuth();
                //if (objUser == null) return BadRequest(authFail);
                //_ActivityService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";

                //File upload logic
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var file1 = files[0];
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Partener");
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
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file1.FileName);
                        using (var fileStream = new FileStream(Path.Combine(tempDirectoryPath, fileName), FileMode.Create))
                        {
                            await file1.CopyToAsync(fileStream);
                        }
                        modelActivity.BannerAttachment = fileName;
                    }
                }
                //Add or updte the activity
                if (modelActivity.ActivityIDP > 0)
                {
                    paramIdentity = await _ActivityService.Activity_Update(modelActivity);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _ActivityService.Activity_Insert(modelActivity);
                    paramIdentityAction = msgInserted;
                    
                }
                // Check if the activity was created/updated successfully
                if (paramIdentity <= 0)
                {
                    return BadRequest(new { status = 201, data = new object[] { }, message = "Activity not created." });
                }
                if (files.Count > 0 && paramIdentity > 0)
                {
                    string newDiretoryPath = Path.Combine(directoryPath, objUser.UserID.ToString(), "Activity");
                    if (!Directory.Exists(newDiretoryPath))
                    {
                        Directory.CreateDirectory(newDiretoryPath);
                    }
                    if (Directory.Exists(tempDirectoryPath))
                    {
                        foreach (var file in new DirectoryInfo(tempDirectoryPath).GetFiles())
                        {
                            file.MoveTo($@"{newDiretoryPath}\{file.Name}");
                        }
                    }
                }
                //Generate a Book Skill button link
                if (modelActivity.SkillID > 0)
                {
                    modelActivity.BookSkillButton = $"https://yourapp.com/skills/expand/{modelActivity.SkillID}";
                }

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction, buttonUrl = modelActivity.BookSkillButton });
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"Controller : AdminPushBanner", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }

        #endregion

        #region INSERT-UPDATE
        [HttpPost]
        [Route("activity-insert-update")]
        public async Task<IActionResult> Activity_InsertUpdate([FromForm] ActivityRequestModel modelActivity)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                // Check for required fields
                // Validate required fields
                var missingFields = new List<string>();

                //if (string.IsNullOrWhiteSpace(modelActivity.BannerAttachment)) missingFields.Add("BannerAttachment");
                if (string.IsNullOrWhiteSpace(modelActivity.ActivityTitle)) missingFields.Add("ActivityTitle");
                if (string.IsNullOrWhiteSpace(modelActivity.ActivityAbout)) missingFields.Add("ActivityAbout");
                if (string.IsNullOrWhiteSpace(modelActivity.Venue)) missingFields.Add("Venue");
                if (modelActivity.StartDateTime == null) missingFields.Add("StartDateTime");
                if (modelActivity.EndDateTime == null) missingFields.Add("EndDateTime");
                if (modelActivity.TotalSeats == null) missingFields.Add("TotalSeats");
                if (modelActivity.Price == null) missingFields.Add("Price");
                if (string.IsNullOrWhiteSpace(modelActivity.WebinarLink)) missingFields.Add("WebinarLink");
                if (modelActivity.StartTimeActual == null) missingFields.Add("StartDateTimeActual");
                if (modelActivity.EndTimeActual == null) missingFields.Add("EndDateTimeActual");
                if (string.IsNullOrWhiteSpace(modelActivity.ActivityInterestName)) missingFields.Add("ActivityInterestName");

                if (missingFields.Any())
                {
                    return BadRequest(new
                    {
                        status = 201,
                        data = new object[] { },
                        message = $"{string.Join(", ", missingFields)} field(s) missing, please input these fields."
                    });
                }

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";


                //[HttpPost]
                //public async Task<ActionResult> UploadBrowseFiles(IList<IFormFile> files)
                //{
                var response = new { FileName = "" };

                if (modelActivity.BannerAttachment != null)
                {
                    string fileName = null;


                    foreach (IFormFile source in modelActivity.BannerAttachment)
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

                     response = new { FileName = fileName.Replace("\"", "") };
                }
                
                //ViewBag.FileName = response.FileName;

                Activity activityRequestModel = new Activity();
                    activityRequestModel.ActivityIDP = modelActivity.ActivityIDP;
                    activityRequestModel.InterestIDF = modelActivity.InterestIDF;
                    activityRequestModel.BannerAttachment = response.FileName;
                    activityRequestModel.ActivityTitle = modelActivity.ActivityTitle;
                    activityRequestModel.ActivityAbout = modelActivity.ActivityAbout;
                    activityRequestModel.Venue = modelActivity.Venue;
                    activityRequestModel.Longitude = modelActivity.Longitude;
                    activityRequestModel.Latitude = modelActivity.Latitude;
                    activityRequestModel.GeoLocation = modelActivity.GeoLocation;
                    activityRequestModel.StartDateTime = modelActivity.StartDateTime;
                    activityRequestModel.EndDateTime = modelActivity.EndDateTime;
                    activityRequestModel.TotalSeats = modelActivity.TotalSeats;
                    activityRequestModel.Price = modelActivity.Price;
                    activityRequestModel.WebinarLink = modelActivity.WebinarLink;
                    activityRequestModel.CouponIDF = modelActivity.CouponIDF;
                    activityRequestModel.SkillID = modelActivity.SkillID;
                    activityRequestModel.BookSkillButton = modelActivity.BookSkillButton;
                    activityRequestModel.StartTimeActual = modelActivity.StartTimeActual;
                    activityRequestModel.EndTimeActual = modelActivity.EndTimeActual;
                    activityRequestModel.ActivityInterestName = modelActivity.ActivityInterestName;


                if (modelActivity.ActivityIDP > 0)
                {
                    paramIdentity = await _ActivityService.Activity_Update(activityRequestModel);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _ActivityService.Activity_Insert(activityRequestModel);
                    if (paramIdentity == 0)
                    {
                        return BadRequest(new { status = 201, data = new object[] { }, message = "Activity not created." });
                    }
                    else
                    {
                        paramIdentityAction = msgInserted; // Activity was successfully inserted
                    }
                }
                if (modelActivity.BannerAttachment.Count > 0 && paramIdentity > 0)
                {
                    // File move to particular folder
                    string newDirectoryPath = Path.Combine(directoryPath, objUser.UserID.ToString(), "Activity");
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
                if (paramIdentity == -1)
                {
                    paramIdentityAction = msgDuplicate;

                    return BadRequest(new { status = 201, data = new object[] { }, message = "Duplicate activity." });
                }
               

               var activityData = await _ActivityService.GetActivityById(paramIdentity);

                return Ok(new { status = 200, data = activityData, message = "activity created successfully." });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspActivity_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }


        private string GetFullPathOfFile(string fileName)
        {
            return $"{_hostEnvironment.WebRootPath}\\Uploads\\{fileName}";
        }
        #endregion INSERT-UPDATE


       

        #region GET Activity_Name
        [HttpGet]
        [Route("Activity_Name")]
        public async Task<IActionResult> Activity_Name()
        {
            try
            {
                //GetAuth();
                //if (objUser == null) return BadRequest(authFail);
                //_ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.GetAllSkillInfoAsync();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspActivity_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET

        #region GET
        [HttpPost]
        [Route("activity-get")]
        public async Task<IActionResult> Activity_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                //GetAuth();
                //if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspActivity_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET

        #region GET
        [HttpPost]
        [Route("activity-request-get-full-detail")]
        public async Task<IActionResult> Activity_Get_Fulldetail([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_Request_Get_FullDetail(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_Get_Fulldetail", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET

        //#region ActivityById
        //[HttpPost]
        //[Route("activity-get-By-Id")]
        //public async Task<IActionResult> activitygetById(long ActivityId)
        //{
        //    try
        //    {
        //        GetAuth();
        //        if (objUser == null) return BadRequest(authFail);
        //        _ActivityService.ObjUser = objUser;

        //        var strResponse = await _ActivityService.GetActivityById(ActivityId);
        //        return Ok(strResponse);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"Controller : uspActivity_GetAll", 1);
        //        return BadRequest(new { status = 201, data = new object[] { }, message = msgError });
        //    }
        //}

        //#endregion

        #region GET-ALL
        [HttpPost]
        [Route("activity-get-all")]
        public async Task<IActionResult> Activity_GetAll([FromForm] int page, [FromForm] int per_page, [FromForm] int status = 0)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_GetAll(status, per_page, page);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspActivity_GetAll", 1);
                return BadRequest(new { status =201, data = new object[] { }, message = msgError });
            }
        }
        //public async Task<IActionResult> Activity_GetAll([FromBody] ModelCommonGet modelCommonGet)
        //{
        //    try
        //    {
        //        GetAuth();
        //        if (objUser == null) return BadRequest(authFail);
        //        _ActivityService.ObjUser = objUser;

        //        var strResponse = await _ActivityService.Activity_GetAll(modelCommonGet.Id);
        //        return Ok(strResponse);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"Controller : uspActivity_GetAll", 1);
        //        return BadRequest(new { status = 0, data = 0, message = msgError });
        //    }
        //}
        #endregion GET-ALL


        #region GET-ALL_ By User
        [HttpPost]
        [Route("activity-get-all-user")]
        public async Task<IActionResult> Activity_GetAll_ByUser([FromBody] SearchWithDisplayTypeWithInterest param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_GetAll_ByUser(param);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_GetAll_ByUser", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL_ By User




        #region  APPROVE-REJECT
        [HttpPost]
        [Route("activity-approve-disapprove")]
        public async Task<IActionResult> Activity_ApproveDisApprove([FromBody] ModelApproveReject modelApproveReject)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_ApproveReject(modelApproveReject.entityID, modelApproveReject.isApprove);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_ApproveDisApprove", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion  APPROVE-REJECT

        #region GET-ALL_ By User IDF
        [HttpPost]
        [Route("activity-get-all-byuseridf")]
        public async Task<IActionResult> Activity_GetAll_ByUserIDF([FromBody] Search modelsearch)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_GetAll_ByUserIDF(modelsearch);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_GetAll_ByUserIDF", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL_ By User  IDF


        #region GET-ALL_ By User IDF of Suggested Offer
        [HttpPost]
        [Route("suggested-offer-activity-get-all-byuseridf")]
        public async Task<IActionResult> Sugested_Offer_Activity_GetAll_ByUserIDF([FromBody] Search modelsearch)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Suggested_Offer_Activity_GetAll_ByUserIDF(modelsearch);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Sugested_Offer_Activity_GetAll_ByUserIDF", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL By User IDF of Suggested Offer


        #region  Activity Booked
        [HttpPost]
        [Route("activity-booked")]
        public async Task<IActionResult> Activity_Booked([FromBody] ActivityBooked modelactivitybook)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_Booked(modelactivitybook);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_Booked", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Activity Booked

        #region GET-ALL_ By SearchF
        [HttpPost]
        [Route("activity-get-all-search")]
        public async Task<IActionResult> Activity_GetAll_BySearch(ActivitySearch activitySearch)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_GetAll_BySearch(activitySearch);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_GetAll_BySearch", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL_ By User  IDF


        #region  Activity Booked
        [HttpPost]
        [Route("activity-booked-cancelled")]
        public async Task<IActionResult> Activity_Booked_Cancelled([FromBody] ActivityBookedCancelled modelactivitybookcancelled)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_Booked_Cancelled(modelactivitybookcancelled);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_Booked_Cancelled", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Activity Booked

        #region GET-ALL_ By For Vendor Coupon
        [HttpPost]
        [Route("activity-coupon-get-all-vendor")]
        public async Task<IActionResult> Activity_Coupon_GetAll_Vendor(ActivityCoupon activityenddate)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_Coupon_GetAll_Vendor(activityenddate);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_Coupon_GetAll_Vendor", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL_ By For Vendor Coupon

        #region DDL
        //[HttpPost]
        //[Route("activity-ddl")]
        //public async Task<IActionResult> Activity_DDL()
        //{
        //    try
        //    {
        //        GetAuth();
        //        if (objUser == null) return BadRequest(authFail);
        //        _ActivityService.ObjUser = objUser;

        //        var strResponse = await _ActivityService.Activity_DDL();
        //        return Ok(strResponse);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"Controller : uspActivity_DDL", 1);
        //        return BadRequest(new { status = 0, data = 0, message = msgError });
        //    }
        //}
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("activity-delete")]
        public async Task<IActionResult> Activity_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _ActivityService.Activity_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspActivity_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE


        #region Update Favourite Activity
        [HttpPost]
        [Route("activity-update-favourite")]
        public async Task<IActionResult> Activity_Update_Favourite_ByUser([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_Update_Favourite_ByUser(modelCommonGet.Id);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_Update_Favourite_ByUser", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Update Favourite Activity

        #region GET-ALL Favourite Activity 
        [HttpPost]
        [Route("activity-get-all-favourite")]
        public async Task<IActionResult> Activity_GetAll_Favourite([FromBody] Search modelsearch)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_GetAll_Favourite(modelsearch);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_GetAll_Favourite", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL Favourite Activity


        #region Update Followers Activity
        [HttpPost]
        [Route("activity-update-followers")]
        public async Task<IActionResult> Activity_Update_Follower([FromBody] ModelFollowerUpdate modelFollowerUpdate)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_Update_Follower(modelFollowerUpdate);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_Update_Follower", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Update Followers Activity

        #region GET-ALL User Followers
        [HttpPost]
        [Route("activity-get-all-follower")]
        public async Task<IActionResult> Activity_GetAll_Follower()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _ActivityService.ObjUser = objUser;

                var strResponse = await _ActivityService.Activity_GetAll_Follower();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_GetAll_Follower", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion  GET-ALL User Followers


    }
}