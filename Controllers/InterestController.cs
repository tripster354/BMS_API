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

namespace BMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class InterestController : CommonController
    {
        private readonly IInterestService _InterestService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public InterestController(BMSContext context, IInterestService __InterestService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _InterestService = __InterestService;
            this.webHostEnvironment = webHostEnvironment;
        }


        #region INSERT-UPDATE
        [HttpPost]
        [Route("interest-insert-update")]
        public async Task<IActionResult> Interest_InsertUpdate([FromForm] Interest modelInterest)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _InterestService.ObjUser = objUser;

                string paramIdentityAction = " ";
                Int64 paramIdentity = 0;

                string directoryPath = "";

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var file1 = files[0];
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Interest");
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
                        modelInterest.InterestAttachment = fileName;
                    }
                }

                if (modelInterest.InterestIDP > 0)
                {
                    paramIdentity = await _InterestService.Interest_Update(modelInterest);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _InterestService.Interest_Insert(modelInterest);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstInterest_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("interest-get")]
        public async Task<IActionResult> Interest_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _InterestService.ObjUser = objUser;

                var strResponse = await _InterestService.Interest_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstInterest_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("interest-get-all")]
        public async Task<IActionResult> Interest_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _InterestService.ObjUser = objUser;

                var strResponse = await _InterestService.Interest_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstInterest_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("interest-active-inactive")]
        public async Task<IActionResult> Interest_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _InterestService.ObjUser = objUser;

                var strResponse = await _InterestService.Interest_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstInterest_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DDL
        [HttpPost]
        [Route("interest-ddl")]
        public async Task<IActionResult> Interest_DDL()
        {
            try
            {
                /*GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _InterestService.ObjUser = objUser;*/

                var strResponse = await _InterestService.Interest_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstInterest_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("interest-delete")]
        public async Task<IActionResult> Interest_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _InterestService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _InterestService.Interest_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstInterest_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE
    }
}