using BMS_API.Services.Interface;
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
using BMS_API.Models.Utility;

namespace BMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class BannerController : CommonController
    {
        private readonly IBannerService _BannerService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BannerController(BMSContext context, IBannerService __BannerService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _BannerService = __BannerService;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region INSERT-UPDATE
        [HttpPost]
        [Route("banner-insert-update")]
        public async Task<IActionResult> Banner_InsertUpdate([FromForm] Banner modelBanner)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _BannerService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;

                string directoryPath = "";

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var file1 = files[0];
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Banner");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    if (file1.Length > 0)
                    {
                        var fileType = Path.GetExtension(file1.FileName);
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file1.FileName);
                        using (var fileStream = new FileStream(Path.Combine(directoryPath, fileName), FileMode.Create))
                        {
                            await file1.CopyToAsync(fileStream);
                        }
                        modelBanner.Attachment = fileName;
                    }
                }

                if (modelBanner.BannerIDP > 0)
                {
                    paramIdentity = await _BannerService.Banner_Update(modelBanner);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _BannerService.Banner_Insert(modelBanner);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstBanner_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("banner-get")]
        public async Task<IActionResult> Banner_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _BannerService.ObjUser = objUser;

                var strResponse = await _BannerService.Banner_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstBanner_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("banner-get-all")]
        public async Task<IActionResult> Banner_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _BannerService.ObjUser = objUser;

                var strResponse = await _BannerService.Banner_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstBanner_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("banner-active-inactive")]
        public async Task<IActionResult> Banner_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _BannerService.ObjUser = objUser;

                var strResponse = await _BannerService.Banner_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstBanner_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region APPROVE-REJECT
        [HttpPost]
        [Route("banner-approve-reject")]
        public async Task<IActionResult> Banner_ApproveReject([FromBody] ModelApproveReject modelApproveReject)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _BannerService.ObjUser = objUser;

                var strResponse = await _BannerService.Banner_ApproveReject(modelApproveReject.entityID, modelApproveReject.isApprove);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstBanner_Update_ApproveReject", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion APPROVE-REJECT


        #region DDL
        //[HttpPost]
        //[Route("banner-ddl")]
        //public async Task<IActionResult> Banner_DDL()
        //{
        //    try
        //    {
        //        GetAuth();
        //        if (objUser == null) return BadRequest(authFail);
        //        _BannerService.ObjUser = objUser;

        //        var strResponse = await _BannerService.Banner_DDL();
        //        return Ok(strResponse);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"Controller : uspmstBanner_DDL", 1);
        //        return BadRequest(new { status = 0, data = 0, message = msgError });
        //    }
        //}
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("banner-delete")]
        public async Task<IActionResult> Banner_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _BannerService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _BannerService.Banner_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstBanner_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE
    }
}
