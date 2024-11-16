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
using BMS_API.Services.User;

namespace BMS_API.Controllers.User
{
    [Route("[controller]")]
    [ApiController]

    public class tblUserController : CommonController
    {
        private readonly ItblUserService _tblUserService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public tblUserController(BMSContext context, ItblUserService __tblUserService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _tblUserService = __tblUserService;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region GET-ALL
        [HttpPost]
        [Route("tbluser-get-all")]
        public async Task<IActionResult> tblUser_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.tblUser_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspUser_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region GET
        [HttpPost]
        [Route("tbluser-get")]
        public async Task<IActionResult> tblUser_Get()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.tblUser_Get(objUser.UserID);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspUser_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("tbluser-active-inactive")]
        public async Task<IActionResult> tblUser_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.tblUser_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : usptblUser_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE

        #region UPDATE-MYPROFILE
        [HttpPost]
        [Route("user-update-myprofile")]
        public async Task<IActionResult> User_Update_MyProfile([FromForm] tblUser modelUser)
        {
            try
            {
                Int64 paramIdentity = 0;
                string directoryPath = "";
                string tempDirectoryPath = "";

                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

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
                        modelUser.ProfileImage = fileName;
                    }
                }

                paramIdentity = await _tblUserService.User_Update_MyProfile(modelUser);

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
                await ErrorLog(1, e.Message, $"Controller : User_Update_MyProfile", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion UPDATES-MYPROFILE

        #region GET Full detail with booking
        [HttpPost]
        [Route("user-get-full-detail")]
        public async Task<IActionResult> User_Get_FullDetail(ModelCommonGet modelCommon)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Get_FullDetail(modelCommon.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Get_FullDetail", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET Full detail with booking


        #region Update Interest
        [HttpPost]
        [Route("user-update-interest")]
        public async Task<IActionResult> User_Update_Interest([FromBody] MyInterest modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Update_Interest(modelCommonGet);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Update_Interest", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Update Interest

        #region GET-ALL Interest  
        [HttpPost]
        [Route("user-get-all-interest")]
        public async Task<IActionResult> User_GetAll_Interest()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_GetAll_Interest();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_GetAll_Interest", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL Interest  


        #region GET-ALL Banner  
        [HttpPost]
        [Route("user-get-all-banner")]
        public async Task<IActionResult> User_GetAll_Banner()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_GetAll_Banner();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_GetAll_Banner", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL Banner  

        #region Insert Contact List 
        [HttpPost]
        [Route("user-insert-contact")]
        public async Task<IActionResult> User_Insert_Contact([FromForm] ContactList contactList2Sync)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                #region Save File to Directory
                var files = HttpContext.Request.Form.Files;

                var uploadPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "User", objUser.UserID.ToString(), "Contact");
                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
                uploadPath += "\\" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + "___" + files.Count.ToString() + "____" + ".txt";
                System.IO.File.AppendAllText(uploadPath, Convert.ToString(contactList2Sync.contactList));

                if (files.Count > 0)
                {
                    string directoryPath = "";
                    string[] imgListSaved = contactList2Sync.mobileNoForProfileImage.Split(',');
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "User", objUser.UserID.ToString());
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "User", objUser.UserID.ToString(), "Contact");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    for (int imgX = 0; imgX < files.Count; imgX++)
                    {
                        var file1 = files[imgX];

                        if (file1.Length > 0)
                        {
                            var fileName = RemoveSpecialChars(imgListSaved[imgX]) + ".png";
                            using (var fileStream = new FileStream(Path.Combine(directoryPath, fileName), FileMode.Create))
                            {
                                await file1.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
                #endregion Save File to Directory


                Int64 paramIdentity = await _tblUserService.User_Insert_Contact(contactList2Sync, connectionString);
                string paramIdentityAction = "";

                if (paramIdentity > 0)
                {
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Insert_Contact", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Insert Contact List

        #region User Activity 
        [HttpPost]
        [Route("user-activity-get-all")]
        public async Task<IActionResult> User_Activity_getAll([FromBody] SearchWithDisplayType searchWithDisplayType)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Activity_GetAll(searchWithDisplayType);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Activity_getAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion User Activity 


        #region User get Contact List 
        [HttpPost]
        [Route("user-contact-get-all")]
        public async Task<IActionResult> User_ContactList_getAll([FromBody] Search param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_ContactList_GetAll(param);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_ContactList_getAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion User get Contact List 

        #region User Post Feed Insert
        [HttpPost]
        [Route("user-feed-insert")]
        public async Task<IActionResult> User_Feed_InsertUpdate([FromForm] ModelFeed modelFeed)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

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
                    paramIdentity = await _tblUserService.User_Feed_Update(modelFeed);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _tblUserService.User_Feed_Insert(modelFeed, connectionString);
                    paramIdentityAction = msgInserted;
                }

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
                await ErrorLog(1, e.Message, $"Controller : User_Feed_InsertUpdate", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion User Post Feed Insert


        #region Get User Activity DDL
        [HttpPost]
        [Route("user-activity-ddl")]
        public async Task<IActionResult> User_Activity_DDL()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Activity_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Activity_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get User Activity DDL


        #region Get All Feed List
        [HttpPost]
        [Route("user-feed-get-all")]
        public async Task<IActionResult> User_Feed_Get_All([FromBody] ModelCommonGetAll param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Feed_Get_All(param);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Feed_Get_All", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get All Feed List


        #region User Update Feed comment
        [HttpPost]
        [Route("user-update-feed-comment")]
        public async Task<IActionResult> User_Update_Feed_Comment([FromBody] ModelFeedComment modelafeedcomment)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Update_Feed_Comment(modelafeedcomment);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Update_Feed_Comment", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion User Update Feed comment

        #region Get All Feed Comment
        [HttpPost]
        [Route("user-feed-get-all-comment")]
        public async Task<IActionResult> User_Feed_Get_All_Comment([FromBody] ModelFeedCommentAll param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Feed_Get_All_Comment(param);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Feed_Get_All_Comment", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get All Feed Comment


        #region DELETE
        [HttpPost]
        [Route("user-feed-delete")]
        public async Task<IActionResult> User_Feed_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _tblUserService.User_Feed_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Feed_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE


        #region  Update Feed Reaction
        [HttpPost]
        [Route("user-update-feed-reaction")]
        public async Task<IActionResult> User_Feed_Update_Reaction([FromBody] ModelFeedUpdateReaction modelfeed)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Feed_Update_Reaction(modelfeed);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Feed_Update_Reaction", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion  Update Feed Reaction


        #region Get Feed
        [HttpPost]
        [Route("user-feed-get")]
        public async Task<IActionResult> User_Feed_Get([FromBody] ModelCommonGet param)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Feed_Get(param.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Feed_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get Feed


        #region Get All Trending Tutor
        [HttpPost]
        [Route("user-get-all-trending-tutor")]
        public async Task<IActionResult> User_Get_All_Trending_Tutor()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Get_All_Trending_Tutor();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Get_All_Trending_Tutor", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get All Trending Tutor

        #region Get All Sugested User
        [HttpPost]
        [Route("user-get-all-sugested-user")]
        public async Task<IActionResult> User_Get_All_Suggested_User()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Get_All_Suggested_User();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Get_All_Suggested_User", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion Get All Sugested User

        #region GET subscription Status
        [HttpPost]
        [Route("user-get-club-subscription-status")]
        public async Task<IActionResult> User_Get_Club_Subscription_Status([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _tblUserService.ObjUser = objUser;

                var strResponse = await _tblUserService.User_Get_Club_Subscription_Status(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : User_Get_Club_Subscription_Status", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET subscription Status


        [NonAction]
        public string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", "+", " ", "-", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }
    }
}