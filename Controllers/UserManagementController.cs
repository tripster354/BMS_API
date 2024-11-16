using BMS_API.Models.DTOs;
using BMS_API.Services.Interface;
using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using BudgetManagement.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using static BMS_API.Services.Interface.ICommon;

namespace BudgetManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class UserManagementController : CommonController
    {
        private readonly IUserManagementService _UserManagementService;
        private readonly IEmailTemplateService _EmailTemplateService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ISettingsService _settingsService;

        public UserManagementController(BMSContext context, IUserManagementService __UserManagementService, IAuthService authService, IWebHostEnvironment webHostEnvironment, ISettingsService settingsService, IEmailTemplateService emailTemplateService) : base(context, authService)
        {
            _context = context;
            _UserManagementService = __UserManagementService;
            this.webHostEnvironment = webHostEnvironment;
            _settingsService = settingsService;
            _EmailTemplateService = emailTemplateService;
        }

        #region INSERT-UPDATE

        [HttpPost]
        [Route("usermanagement-insert-update")]
        public async Task<IActionResult> UserManagement_InsertUpdate([FromForm] ModelUserManagement modelUserManagement)
        {
            try
            {
                string paramIdentityAction = "";
                Int64 paramIdentity = 0;

                // Handle file uploads
                string uploadDocPath = string.Empty;
                string videoPath = string.Empty;
                string profileImagePath = string.Empty;

                if (modelUserManagement.UploadDoc != null)
                {
                    // Save the uploaded document
                    uploadDocPath = await SaveFileAsync(modelUserManagement.UploadDoc);
                }

                if (modelUserManagement.Video != null)
                {
                    // Save the uploaded video
                    videoPath = await SaveFileAsync(modelUserManagement.Video);
                }

                if (modelUserManagement.ProfileImage != null)
                {
                    // Save the uploaded profile image
                    profileImagePath = await SaveFileAsync(modelUserManagement.ProfileImage);
                }

                if (modelUserManagement.UserIDP > 0)
                {
                    // Update logic here
                    paramIdentity = await _UserManagementService.UserManagement_Update(modelUserManagement);
                    paramIdentityAction = "Vendor updated successfully."; // Modify this message as needed
                }
                else
                {
                    // Insert logic here
                    paramIdentity = await _UserManagementService.UserManagement_Insert(modelUserManagement);
                    if (paramIdentity == 0)
                    {
                        paramIdentityAction = "Vendor not created.";
                    }
                    else
                    {
                        paramIdentityAction = "Vendor created successfully.";
                    }
                }

                // Check for duplicate email or mobile number
                if (paramIdentity == -1)
                {
                    paramIdentityAction = "Mobile or email already registered.";
                    return BadRequest(new { status = 201, data = new object[] { }, message = paramIdentityAction });
                }

                var vendorData = await _UserManagementService.GetVendorById(paramIdentity);
                if (vendorData != null)

                return Ok(new { status = 200, data = vendorData, message = paramIdentityAction });
                else
                {
                    return BadRequest(new { status = 201, data = new object[] { }, message = paramIdentityAction });
                }
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Vendor_InsertUpdate", 1);
                return BadRequest(new { status = 201, data = new object[] { }, message = "Error occurred during the operation." });
            }
        }

        // Helper method to save files
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                // Define the path where you want to save the file
                var filePath = Path.Combine("Your/Upload/Path", file.FileName);

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filePath; // Return the path of the saved file
            }
            return string.Empty;
        }

        #endregion INSERT-UPDATE

        #region GET
        [HttpPost]
        [Route("usermanagement-get")]
        public async Task<IActionResult> UserManagement_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                var strResponse = await _UserManagementService.UserManagement_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : UserManagement_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET

        #region GET-ALL
        [HttpPost]
        [Route("usermanagement-get-all")]
        public async Task<IActionResult> UserManagement_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                var strResponse = await _UserManagementService.UserManagement_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspsysUser_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("usermanagement-active-inactive")]
        public async Task<IActionResult> UserManagement_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                var strResponse = await _UserManagementService.UserManagement_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspsysUser_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DDL
        [HttpPost]
        [Route("usermanagement-ddl")]
        public async Task<IActionResult> UserManagement_DDL()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                var strResponse = await _UserManagementService.UserManagement_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspsysUser_DDL_By_RoleIDF", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DDL

        #region DELETE
        [HttpPost]
        [Route("usermanagement-delete")]
        public async Task<IActionResult> UserManagement_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                Int64 responseIdentity = await _UserManagementService.UserManagement_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : UserManagement_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE

        #region INVITE
        [HttpPost]
        [Route("user-invite")]
        public async Task<IActionResult> User_Invite([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                string strResponse = await _UserManagementService.UserManagement_Get_Invite(modelCommonGet.Id);
                var data = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponse);

                string strResponseEmail = await _EmailTemplateService.EmailTemplate_GetByTemplateType((Byte)EmailTemplateType.Registration);
                var dataEmail = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponseEmail);

                //EmailNotificationBL emailNotification = new EmailNotificationBL(_context, webHostEnvironment, _settingsService);

                /*
                string mailBody = "Hello " + data[0]["FirstName"].ToString() + data[0]["LastName"].ToString() + "<br>";
                mailBody += "You are invited to use our Budget Management System<br>";
                mailBody += "Here is your login credentials : <br>";
                mailBody += "URL : <a href=\"https://budget.fusioninformatics.net\">https://budget.fusioninformatics.net</a><br>";
                mailBody += "Username : " + data[0]["Email"].ToString() + "<br>";
                mailBody += "Password : " + data[0]["Password"].ToString() + "<br>";
                */
                string mailBody = dataEmail[0]["EmailContent"].ToString().Replace("#UserName", data[0]["FirstName"].ToString() + data[0]["LastName"].ToString());
                mailBody = mailBody.Replace("#URL", "<br><a href=\"https://budget.fusioninformatics.net\">https://budget.fusioninformatics.net</a><br>");
                mailBody = mailBody.Replace("#Email", data[0]["Email"].ToString() + "<br>");
                mailBody = mailBody.Replace("#Password", data[0]["Password"].ToString() + "<br>");
                //mailBody = "Password : 1111<br>";

                //strResponse = await emailNotification.Execute(data[0]["Email"].ToString(), null, "Invitation for Budget Management", mailBody);
                //await emailNotification.Execute(data[0]["Email"].ToString(), null, dataEmail[0]["EmailSubject"].ToString(), mailBody);
                //strResponse = await emailNotification.Execute("jaydeepsolanki0045@gmail.com", null, dataEmail[0]["EmailSubject"].ToString(), mailBody);

                return Ok(new { status = 1, data = strResponse, message = msgInvite });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : Employee_Invite", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INVITE

        #region UPDATE-MYPROFILE
        [HttpPost]
        [Route("user-update-myprofile")]
        public async Task<IActionResult> User_Update_MyProfile([FromForm] ModelUserManagement modelUserManagement)
        {
            try
            {
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";

                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var file1 = files[0];
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Employee");
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
                        // modelUserManagement.ProfileImage = fileName;
                    }
                }

                paramIdentity = await _UserManagementService.User_Update_MyProfile(modelUserManagement);

                if (files.Count > 0)
                {
                    // File move to particular folder
                    string newDirectoryPath = Path.Combine(directoryPath, modelUserManagement.UserIDP.ToString());
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
                await ErrorLog(1, e.Message, $"Controller : User_Update_MyProfile", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion UPDATES-MYPROFILE

        #region UPDATE-MYCREDENTIAL
        [HttpPost]
        [Route("user-update-mycredential")]
        public async Task<IActionResult> User_Update_MyCredential([FromBody] ModelUpdateCredential updateCredential)
        {
            try
            {
                Int64 paramIdentity = 0;

                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                paramIdentity = await _UserManagementService.User_Update_MyCredential(updateCredential);
                return Ok(new { status = 1, data = paramIdentity, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Update_MyCredential", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion UPDATES-MYCREDENTIAL

        #region UPDATE-MYNOTIFICATION
        [HttpPost]
        [Route("user-update-mynotification")]
        public async Task<IActionResult> User_Update_MyNotification([FromBody] ModelNotification modelNotification)
        {
            try
            {
                Int64 paramIdentity = 0;

                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _UserManagementService.ObjUser = objUser;

                paramIdentity = await _UserManagementService.User_Update_MyNotification(modelNotification);
                return Ok(new { status = 1, data = paramIdentity, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Update_MyNotification", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion UPDATES-MYNOTIFICATION

        [HttpPost]
        [Route("vendor-login")]
        public async Task<IActionResult> LoginWithMobileOTP([FromForm] VendorLoginRequest request)
        {

            try
            {
                var result = await _UserManagementService.LoginWithMobileOTP(request.LoginInput, request.Password);
                if (result == null)
                {
                    return BadRequest(new { status = 201, data = new object[] { }, message = "Invalid credentials" });
                }

                return Ok(new { status= 200, UserDetails = result, Message = "Login successful" });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = 201, data = new object[] { }, message = msgError });
            }
        }

        [HttpGet]
        [Route("acivityinterests")]
        public async Task<IActionResult> GetAllActivityInterests()
        {
            List<ActivityInterestDTO> interests = await _UserManagementService.GetAllInterestActivityNames();
            if (interests == null || interests.Count == 0)
            {
                return NotFound("No acitivity found");
            }
            return Ok(interests);
        }
    }
}