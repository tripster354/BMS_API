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
using BMS_API.Services;
using Microsoft.Data.SqlClient;
using BMS_API.Models.User;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using BMS_API.Models;
using BMS_API.Services.Partner;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;

namespace BMS_API.Controllers.Partner
{
    [Route("[controller]")]
    [ApiController]

    public class tblPartnerController : CommonController
    {
        private readonly ItblPartnerService _tblPartnerService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IWebHostEnvironment _hostEnvironment;

        public tblPartnerController(IWebHostEnvironment hostEnvironment, BMSContext context, ItblPartnerService __tblPartnerService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _tblPartnerService = __tblPartnerService;
            this.webHostEnvironment = webHostEnvironment;
            _hostEnvironment = hostEnvironment;
        }


        #region Registration
        [HttpPost]
        [Route("tblpartner-registration")]
        public async Task<IActionResult> tblPartner_Registration([FromForm] tblPartnerRequest Partner)
        {
            try
            {
                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                Int64 applicationStatus = 0;

                string directoryPath = "";
                string tempDirectoryPath = "";
                string UploadedfileName = null;
                foreach (IFormFile source in Partner.ProfileImage)
                {
                    // Get original file name to get the extension from it.
                    string orgFileName = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName;

                    // Create a new file name to avoid existing files on the server with the same names.
                    // fileName = DateTime.Now.ToFileTime() + Path.GetExtension(orgFileName);
                    UploadedfileName = DateTime.Now.Second + orgFileName;


                    string fullPath = GetFullPathOfFile(UploadedfileName.Replace("\"", ""));

                    // Create the directory.
                    Directory.CreateDirectory(Directory.GetParent(fullPath).FullName);

                    // Save the file to the server.
                    await using FileStream output = System.IO.File.Create(fullPath);
                    await source.CopyToAsync(output);
                }

                var response = new { FileName = UploadedfileName.Replace("\"", "") };

                tblPartner modeltblPartner = new tblPartner();
                modeltblPartner.PartnerIDP = Partner.PartnerIDP;
                modeltblPartner.FullName = Partner.FullName;
                modeltblPartner.MobileNo = Partner.MobileNo;
                modeltblPartner.EmailID = Partner.EmailID;
                modeltblPartner.Password = Partner.Password;
                modeltblPartner.ProfileImage = response.FileName;
                modeltblPartner.ActivityTypeID = Partner.ActivityTypeID;
                modeltblPartner.KYCAttachment1 = Partner.KYCAttachment1;
                modeltblPartner.KYCAttachment2 = Partner.KYCAttachment2;
                modeltblPartner.KYCAttachment3 = Partner.KYCAttachment3;
                modeltblPartner.KYCAttachment4 = Partner.KYCAttachment4;
               modeltblPartner.VideoAttachment = Partner.VideoAttachment;
                modeltblPartner.SocialFacebook = Partner.SocialFacebook;
                modeltblPartner.SocialLinkedIn = Partner.SocialLinkedIn;
                modeltblPartner.SocialTweeter = Partner.SocialTweeter;
                modeltblPartner.SocialTelegram = Partner.SocialTelegram;
                modeltblPartner.SocialOther = Partner.SocialOther;
                modeltblPartner.SocialInstagram = Partner.SocialInstagram;
                modeltblPartner.RefrenceLink = Partner.RefrenceLink;
                modeltblPartner.YearofExperience = Partner.YearofExperience;
                modeltblPartner.ApplicationStatus = Partner.ApplicationStatus;
                modeltblPartner.BankDetail = Partner.BankDetail;

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file1 = files[i];
                        directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Partner");
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
                            if(file1.Name == "KYCAttachment1") modeltblPartner.KYCAttachment1 = fileName;
                            if (file1.Name == "KYCAttachment2") modeltblPartner.KYCAttachment2 = fileName;
                            if (file1.Name == "KYCAttachment3") modeltblPartner.KYCAttachment3 = fileName;
                            if (file1.Name == "KYCAttachment4") modeltblPartner.KYCAttachment4 = fileName;
                            if (file1.Name == "VideoAttachment") modeltblPartner.VideoAttachment = fileName;
                        }
                    }
                }

                try
                {
                    SqlParameter paramPartnerIDP = new SqlParameter
                    {
                        ParameterName = "@PartnerIDP",
                        SqlDbType = System.Data.SqlDbType.BigInt,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    SqlParameter ProfileImage = new SqlParameter("@ProfileImage", (object)modeltblPartner.ProfileImage ?? DBNull.Value);
                    SqlParameter paramFullName = new SqlParameter("@FullName", (object)modeltblPartner.FullName ?? DBNull.Value);
                    SqlParameter paramMobileNo = new SqlParameter("@MobileNo", (object)modeltblPartner.MobileNo ?? DBNull.Value);
                    SqlParameter paramEmailID = new SqlParameter("@EmailID", (object)modeltblPartner.EmailID ?? DBNull.Value);
                    SqlParameter paramPassword = new SqlParameter("@Password", (object)modeltblPartner.Password ?? DBNull.Value);
                    SqlParameter paramActivityTypeID = new SqlParameter("@ActivityTypeID", (object)modeltblPartner.ActivityTypeID ?? DBNull.Value);
                    SqlParameter paramKYCAttachment1 = new SqlParameter("@KYCAttachment1", (object)modeltblPartner.KYCAttachment1 ?? DBNull.Value);
                    SqlParameter paramKYCAttachment2 = new SqlParameter("@KYCAttachment2", (object)modeltblPartner.KYCAttachment2 ?? DBNull.Value);
                    SqlParameter paramKYCAttachment3 = new SqlParameter("@KYCAttachment3", (object)modeltblPartner.KYCAttachment3 ?? DBNull.Value);
                    SqlParameter paramKYCAttachment4 = new SqlParameter("@KYCAttachment4", (object)modeltblPartner.KYCAttachment4 ?? DBNull.Value);
                    SqlParameter paramVideoAttachment = new SqlParameter("@VideoAttachment", (object)modeltblPartner.VideoAttachment ?? DBNull.Value);

                    SqlParameter paramSocialFacebook = new SqlParameter() { ParameterName = "@SocialFacebook", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.SocialFacebook == "null" ? "" : modeltblPartner.SocialFacebook };
                    SqlParameter paramSocialLinkedIn = new SqlParameter() { ParameterName = "@SocialLinkedIn", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.SocialLinkedIn == "null" ? "" : modeltblPartner.SocialLinkedIn };
                    SqlParameter paramSocialInstagram = new SqlParameter() { ParameterName = "@SocialInstagram", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.SocialInstagram == "null" ? "" : modeltblPartner.SocialInstagram };
                    SqlParameter paramrReferenceLink = new SqlParameter() { ParameterName = "@RefrenceLink", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.RefrenceLink == "null" ? "" : modeltblPartner.RefrenceLink };
                    SqlParameter paramSocialTweeter = new SqlParameter() { ParameterName = "@SocialTweeter", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.SocialTweeter == "null" ? "" : modeltblPartner.SocialTweeter };
                    SqlParameter paramExperience = new SqlParameter() { ParameterName = "@Experience", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.YearofExperience == "null" ? DBNull.Value : modeltblPartner.YearofExperience };

                    //SqlParameter paramApplicationStatus = new SqlParameter("@ApplicationStatus", modeltblPartner.ApplicationStatus);
                    SqlParameter paramApplicationStatus = new SqlParameter
                    {
                        ParameterName = "@ApplicationStatus",
                        SqlDbType = System.Data.SqlDbType.TinyInt,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    var paramSqlQuery = "EXECUTE dbo.uspPartner_Registration @PartnerIDP OUTPUT,@ProfileImage, @FullName, @MobileNo, @EmailID, @Password, @ActivityTypeID, @KYCAttachment1, @KYCAttachment2, @KYCAttachment3, @KYCAttachment4, @VideoAttachment, @SocialFacebook, @SocialLinkedIn, @SocialInstagram,@RefrenceLink,@SocialTweeter,@Experience, @ApplicationStatus OUTPUT";
                    await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramPartnerIDP, ProfileImage, paramFullName, paramMobileNo, paramEmailID, paramPassword, paramActivityTypeID, paramKYCAttachment1, paramKYCAttachment2, paramKYCAttachment3, paramKYCAttachment4, paramVideoAttachment, paramSocialFacebook, paramSocialLinkedIn, paramSocialInstagram, paramrReferenceLink, paramSocialTweeter, paramExperience, paramApplicationStatus);

                    if (Convert.ToInt32(paramApplicationStatus.Value) == 0 || Convert.ToInt32(paramApplicationStatus.Value) == 1 || Convert.ToInt32(paramApplicationStatus.Value) == 2 || Convert.ToInt32(paramApplicationStatus.Value) == 3 || Convert.ToInt32(paramApplicationStatus.Value) == 4)
                    {
                        applicationStatus = Convert.ToInt64(paramApplicationStatus.Value);
                        paramIdentity = Convert.ToInt64(paramPartnerIDP.Value);
                        paramIdentityAction = "Registration successfully.";
                    }
                    else
                    {
                        applicationStatus = Convert.ToInt64(paramApplicationStatus.Value);
                        paramIdentity = Convert.ToInt64(paramPartnerIDP.Value);
                    }
                }
                catch (Exception e)
                {
                    await ErrorLog(1, e.Message, $"uspPartner_Registration", 1);
                }

                //paramIdentity = await _tblPartnerService.tblPartner_Registration(modeltblPartner);
                //paramIdentityAction = "Registration successfully.";

                // File move to particular folder
                if (files.Count > 0)
                {
                    string newDirectoryPath = Path.Combine(directoryPath, paramIdentity.ToString());
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

                if (applicationStatus == 1) paramIdentityAction = msgDuplicate;
                if (applicationStatus == 2) paramIdentityAction = "Already registered but pending verification.";
                if (applicationStatus == 3) paramIdentityAction = "Already registered but Rejected.";
                if (applicationStatus == 4) paramIdentityAction = "Already Registered And Approved And Inactive.";

                return Ok(new { status = 1, id = paramIdentity, data = applicationStatus, message = paramIdentityAction });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPartner_Registration", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Registration
        private string GetFullPathOfFile(string fileName)
        {
            return $"{_hostEnvironment.WebRootPath}\\Uploads\\{fileName}";
        }

        #region GET
        [HttpPost]
        [Route("tblpartner-get")]
        public async Task<IActionResult> tblPartner_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.tblPartner_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPartner_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("tblpartner-get-all")]
        public async Task<IActionResult> tblPartner_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.tblPartner_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPartner_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("tblpartner-active-inactive")]
        public async Task<IActionResult> tblPartner_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.tblPartner_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPartner_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DDL
        //[HttpPost]
        //[Route("tblpartner-ddl")]
        //public async Task<IActionResult> tblPartner_DDL()
        //{
        //    try
        //    {
        //        GetAuth();
        //        if (objUser == null) return BadRequest(authFail);
        //        _tblPartnerService.ObjUser = objUser;

        //        var strResponse = await _tblPartnerService.tblPartner_DDL();
        //        return Ok(strResponse);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"Controller : uspPartner_DDL", 1);
        //        return BadRequest(new { status = 0, data = 0, message = msgError });
        //    }
        //}
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("tblpartner-delete")]
        public async Task<IActionResult> tblPartner_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _tblPartnerService.tblPartner_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPartner_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE

        #region APPROVE-REJECT
        [HttpPost]
        [Route("tblpartner-approve-reject")]
        public async Task<IActionResult> tblPartner_ApproveReject([FromBody] ModelApproveReject modelApproveReject)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.tblPartner_ApproveReject(modelApproveReject.entityID, modelApproveReject.isApprove);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : usptblPartner_Update_ApproveReject", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion APPROVE-REJECT

        #region UPDATE-MYPROFILE
        [HttpPost]
        [Route("partner-update-myprofile")]
        public async Task<IActionResult> Partner_Update_MyProfile([FromForm] tblPartner modeltblPartner)
        {
            try
            {
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";

                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file1 = files[i];
                        directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Partner");
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
                            //modeltblPartner.ProfileImage = fileName;
                            if (file1.Name == "KYCAttachment1") modeltblPartner.KYCAttachment1 = fileName;
                            if (file1.Name == "KYCAttachment2") modeltblPartner.KYCAttachment2 = fileName;
                            if (file1.Name == "KYCAttachment3") modeltblPartner.KYCAttachment3 = fileName;
                            if (file1.Name == "KYCAttachment4") modeltblPartner.KYCAttachment4 = fileName;
                            if (file1.Name == "VideoAttachment") modeltblPartner.VideoAttachment = fileName;
                            if (file1.Name == "ProfileImage") modeltblPartner.ProfileImage = fileName;
                        }
                    }
                }

                paramIdentity = await _tblPartnerService.Partner_Update_MyProfile(modeltblPartner);

                if (files.Count > 0)
                {
                    // File move to particular folder
                    string newDirectoryPath = Path.Combine(directoryPath, paramIdentity.ToString());
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
                return Ok(new { status = 1, data = paramIdentity, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Update_MyProfile", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion UPDATES-MYPROFILE

        #region GET Mobile
        [HttpPost]
        [Route("partner-get")]
        public async Task<IActionResult> tblPartner_Get()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.tblPartner_Get(objUser.UserID);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPartner_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET Mobile


        #region GET Mobile Dashboard summary
        [HttpPost]
        [Route("partner-get-dashboard")]
        public async Task<IActionResult> Partner_Get_Dashboard(PartnerDashboard dashboard)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Get_Dashboard(objUser.UserID, dashboard);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Get_Dashboard", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET Mobile Dashboard summary




        #region GET all Booking 
        [HttpPost]
        [Route("partner-get-all-booking")]
        public async Task<IActionResult> Partner_Get_All_Booking(ModelCommonGet modelCommon)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Get_All_Booking(objUser.UserID, modelCommon.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Get_All_Booking", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET all Booking 

        #region GET Full detail booking
        [HttpPost]
        [Route("partner-get-full-detail-booking")]
        public async Task<IActionResult> Partner_Get_FullDetail_Booking(ModelCommonGet modelCommon)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Get_FullDetail_Booking(modelCommon.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Get_FullDetail_Booking", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET Full detail booking

        #region  update Activity actual time
        [HttpPost]
        [Route("partner-update-activity-actualtime")]
        public async Task<IActionResult> Activity_UpdateActualTime([FromBody] ActivityActualTime modelactivity)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Activity_UpdateActualTime(modelactivity);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_UpdateActualTime", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion  update Activity actual time


        #region  update Activity attendance
        [HttpPost]
        [Route("partner-update-activity-attendance")]
        public async Task<IActionResult> Activity_UpdateAttendance([FromBody] ActivityAttendace modelactivityattendace)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Activity_UpdateAttendance(modelactivityattendace);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Activity_UpdateAttendance", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion  update Activity attendance



        #region Post Feed Insert
        [HttpPost]
        [Route("partner-feed-insert")]
        public async Task<IActionResult> Feed_InsertUpdate([FromForm] ModelFeed modelFeed)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var file1 = files[0];
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Feed");
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
                        modelFeed.BannerAttachment = fileName;
                    }
                }

                if (modelFeed.FeedIDP > 0)
                {
                    paramIdentity = await _tblPartnerService.Feed_Update(modelFeed);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _tblPartnerService.Feed_Insert(modelFeed, connectionString);
                    paramIdentityAction = msgInserted;
                }


                /*paramIdentity = await _tblPartnerService.Feed_Insert(modelFeed, connectionString);
                paramIdentityAction = msgInserted;*/
                
                if (files.Count > 0 && paramIdentity > 0)
                {
                    // File move to particular folder
                    string newDirectoryPath = Path.Combine(directoryPath, paramIdentity.ToString());
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
                await ErrorLog(1, e.Message, $"Controller : Feed_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Post Feed Insert



        #region Get Activity Participate List
        [HttpPost]
        [Route("partner-activity-get-participate-list")]
        public async Task<IActionResult> Partner_Activity_Participate_List([FromBody] SearchParticipateFeed param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Activity_Participate_List(param);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Activity_Participate_List", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get Activity Participate List


        #region Get Activity DDL
        [HttpPost]
        [Route("partner-activity-ddl")]
        public async Task<IActionResult> Partner_Activity_DDL()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Activity_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Activity_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get Activity DDL


        #region Get All Feed List
        [HttpPost]
        [Route("partner-feed-get-all")]
        public async Task<IActionResult> Partner_Feed_Get_All([FromBody] ModelCommonGetAll param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Feed_Get_All(param);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Feed_Get_All", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get All Feed List


        #region  Update Feed comment
        [HttpPost]
        [Route("partner-update-feed-comment")]
        public async Task<IActionResult> Partner_Update_Feed_Comment([FromBody] ModelFeedComment modelafeedcomment)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Update_Feed_Comment(modelafeedcomment);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Update_Feed_Comment", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Update Feed comment

        #region Get All Feed Comment
        [HttpPost]
        [Route("partner-feed-get-all-comment")]
        public async Task<IActionResult> Partner_Feed_Get_All_Comment([FromBody] ModelFeedCommentAll param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Feed_Get_All_Comment(param);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Feed_Get_All_Comment", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get All Feed Comment


        #region DELETE
        [HttpPost]
        [Route("partner-feed-delete")]
        public async Task<IActionResult> Feed_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _tblPartnerService.Feed_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Feed_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE


        #region  Update Feed Reaction
        [HttpPost]
        [Route("partner-update-feed-reaction")]
        public async Task<IActionResult> Feed_Update_Reaction([FromBody] ModelFeedUpdateReaction modelfeed)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Feed_Update_Reaction(modelfeed);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Feed_Update_Reaction", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion  Update Feed Reaction


        #region Get Feed
        [HttpPost]
        [Route("partner-feed-get")]
        public async Task<IActionResult> Partner_Feed_Get([FromBody] ModelCommonGet param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Feed_Get(param.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Feed_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get Feed

        #region GET subscription Status
        [HttpPost]
        [Route("partner-get-club-subscription-status")]
        public async Task<IActionResult> Partner_Get_Club_Subscription_Status([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblPartnerService.ObjUser = objUser;

                var strResponse = await _tblPartnerService.Partner_Get_Club_Subscription_Status(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Partner_Get_Club_Subscription_Status", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET subscription Status

    }
}