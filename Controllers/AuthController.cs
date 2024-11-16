using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models.Utility;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using BMS_API.Services.Interface;
using BMS_API.Models.Utility;
using BMS_API.Services;
using BudgetManagement.Services;
using static BMS_API.Services.Interface.ICommon;
using System.Runtime.CompilerServices;
using BMS_DAL;

namespace BudgetManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : CommonController
    {

        public ISettingsService _settingsService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmailTemplateService _EmailTemplateService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public AuthController(BMSContext context, IAuthService authService, IWebHostEnvironment webHostEnvironment, ISettingsService settingsService, IEmailTemplateService emailTemplateService) : base(context, authService)
        {
            _context = context;
            _authService = authService;
            _settingsService = settingsService;
            this.webHostEnvironment = webHostEnvironment;
            _EmailTemplateService = emailTemplateService;
        }
        #region auth login
        // POST Auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] AuthLogin authLogin)
        {
            try
            {
                if (authLogin == null) return BadRequest(new { msg = "Invalid login request." });

                // Get the login response from the service
                var loginResponse = await _authService.Login(authLogin, "");

                // Check if login was successful or not
                if (loginResponse.Status == 0)
                {
                    return BadRequest(new {status=201, data = new object[] { }, msg = loginResponse.Message });
                }

                return Ok(new
                {
                    status = loginResponse.Status,
                    data = loginResponse.Data,
                    message = loginResponse.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Something went wrong trying to login.", error = ex.Message });
            }
        }

        #endregion auth login


        [HttpPost]
        [Route("admin-update-password")]
        public async Task<IActionResult> Admin_Update_Password([FromBody] SysAdminCredential adminInfo)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);

                Int64 strResponse = await _authService.Auth_Admin_Change_Password(adminInfo.UserName, adminInfo.Password, adminInfo.FirstName, adminInfo.LastName, adminInfo.OldPassword);
                return Ok(strResponse);
            }
            catch
            {
                return BadRequest("Something went wrong trying to login.");
            }
        }

        [HttpPost]
        [Route("admin-update-settings")]
        public async Task<IActionResult> Admin_Update_Settings([FromBody] SysAdmin adminInfo)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);

                bool strResponse = await _authService.Auth_Admin_Change_Settings(adminInfo);
                return Ok(strResponse);
            }
            catch
            {
                return BadRequest("Something went wrong trying to login.");
            }
        }

        [HttpPost]
        [Route("admin-get-setting")]
        public async Task<IActionResult> Admin_GetSettings()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);

                string strResponse = await _settingsService.Admin_GetSettings();
                return Ok(strResponse);
            }
            catch
            {
                return BadRequest("Something went wrong trying to login.");
            }
        }

        [HttpPost]
        [Route("password-forgot")]
        public async Task<IActionResult> PasswordForgot([FromBody] AuthLogin userInfo)
        {
            try
            {
                string strResponse = await _authService.Auth_PasswordForgot(userInfo.UserName, userInfo.UserType);

                if (strResponse == "")
                {
                    return BadRequest("User not found");
                    //return Ok(0);
                }

                var data = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponse);

                if (data[0]["UserType"].ToString() == "1")
                {
                    EmailNotificationBL emailNotification = new EmailNotificationBL(_context, webHostEnvironment, _settingsService);

                    string strResponseEmail = await _EmailTemplateService.EmailTemplate_GetByTemplateType((Byte)EmailTemplateType.ForgotPassword);
                    var dataEmail = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponseEmail);

                    string mailSubject = dataEmail[0]["EmailSubject"].ToString();

                    string helperX = "";
                    string mailBody = dataEmail[0]["EmailContent"].ToString();
                    helperX = mailBody;
                    helperX = helperX.Replace("#UserName", data[0]["DisplayName"].ToString());
                    helperX = helperX.Replace("#key", data[0]["UserType"].ToString() + "&r=" + data[0]["RowGUID"].ToString());
                    mailBody = helperX;

                    strResponse = await emailNotification.Execute(userInfo.UserName, null, mailSubject, mailBody);
                }

                //return Ok(strResponse);
                return Ok(new { status = 1, data = 1, message = "User found", r = data[0]["RowGUID"].ToString(), ut = data[0]["UserType"].ToString() });

            }
            catch
            {
                return BadRequest("Something went wrong trying to password forgot.");
                //return Ok(0);
            }
        }

        [HttpPost]
        [Route("password-reset")]
        public async Task<IActionResult> PasswordReset([FromBody] AuthReset newPass)
        {
            try
            {
                bool IsDone = await _authService.Auth_PasswordChange(newPass);
                return Ok(IsDone);
            }
            catch
            {
                return BadRequest("Something went wrong trying to login.");
            }
        }

        [HttpPost]
        [Route("otp-varification")]
        public async Task<IActionResult> OTPVarification([FromBody] AuthOTPVarification oTPVarification)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _authService.ObjUser = objUser;

                bool IsDone = await _authService.Auth_OTPVarification(oTPVarification);
                return Ok(IsDone);
            }
            catch
            {
                return BadRequest("Something went wrong trying to otp-varification.");
            }
        }

        [HttpPost]
        [Route("otp-request")]
        public async Task<IActionResult> OTPRequest()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _authService.ObjUser = objUser;

                string strResponse = await _authService.Auth_OTPRequest();

                if (strResponse != "")
                {
                    var resData = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponse.ToString());
                    if (resData.Count != 0)
                    {
                        if (resData[0]["OTP"].ToString() != "")
                        {
                            string strResponseEmail = await _EmailTemplateService.EmailTemplate_GetByTemplateType((Byte)EmailTemplateType.LoginOTP);
                            var dataEmail = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponseEmail);

                            //var data = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponse);
                            string mailBody = dataEmail[0]["EmailContent"].ToString().Replace("#OTP", resData[0]["OTP"].ToString());

                            EmailNotificationBL emailNotification = new EmailNotificationBL(_context, webHostEnvironment, _settingsService);
                            await emailNotification.Execute(resData[0]["Email"].ToString(), null, dataEmail[0]["EmailSubject"].ToString(), mailBody);
                        }
                    }

                    //string strResponseEmail = await _EmailTemplateService.EmailTemplate_GetByTemplateType((Byte)EmailTemplateType.LoginOTP);
                    //var dataEmail = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponseEmail);

                    //var data = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(strResponse);

                    ////string mailBody = "One Time Password is : " + data[0]["OTP"].ToString();
                    //string mailBody = dataEmail[0]["EmailContent"].ToString().Replace("#OTP", data[0]["OTP"].ToString());   
                    //data[0]["Email"].ToString();
                    //EmailNotificationBL emailNotification = new EmailNotificationBL(_context, webHostEnvironment, _settingsService);
                    //await emailNotification.Execute(data[0]["Email"].ToString(), null, dataEmail[0]["EmailSubject"].ToString(), mailBody);
                    //await emailNotification.Execute("jaydeepsolanki0045@gmail.com", null, "One-time password for login", mailBody);
                }

                return Ok(true);
            }
            catch
            {
                return BadRequest("Something went wrong trying to login.");
            }
        }

        [HttpPost]
        [Route("log-out")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _authService.ObjUser = objUser;

                bool IsDone = await _authService.Auth_Logout();
                return Ok(IsDone);
            }
            catch
            {
                return BadRequest("Something went wrong trying to logout.");
            }
        }
    }
}
