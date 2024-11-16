using BMS_API.Models.User;
using BMS_API.Services.Interface;
using BudgetManagement.Controllers;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models;
using BMS_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BMS_API.Models.Utility;

namespace BMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class SubAdminController : CommonController
    {
        private readonly ISubAdminService _SubAdminService;

        public SubAdminController(BMSContext context, ISubAdminService __SubAdminService, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _SubAdminService = __SubAdminService;
        }


        #region INSERT-UPDATE
        [HttpPost]
        [Route("subadmin-insert-update")]
        public async Task<IActionResult> SubAdmin_InsertUpdate([FromForm] SubAdmin modelSubAdmin)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubAdminService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                if (modelSubAdmin.SubAdminIDP > 0)
                {
                    paramIdentity = await _SubAdminService.SubAdmin_Update(modelSubAdmin, connectionString);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _SubAdminService.SubAdmin_Insert(modelSubAdmin, connectionString);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubAdmin_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("subadmin-get")]
        public async Task<IActionResult> SubAdmin_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubAdminService.ObjUser = objUser;

                var strResponse = await _SubAdminService.SubAdmin_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubAdmin_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("subadmin-get-all")]
        public async Task<IActionResult> SubAdmin_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubAdminService.ObjUser = objUser;

                var strResponse = await _SubAdminService.SubAdmin_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubAdmin_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("subadmin-active-inactive")]
        public async Task<IActionResult> SubAdmin_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubAdminService.ObjUser = objUser;

                var strResponse = await _SubAdminService.SubAdmin_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubAdmin_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DDL
        //[HttpPost]
        //[Route("subadmin-ddl")]
        //public async Task<IActionResult> SubAdmin_DDL()
        //{
        //    try
        //    {
        //        GetAuth();
        //        if (objUser == null) return BadRequest(authFail);
        //        _SubAdminService.ObjUser = objUser;

        //        var strResponse = await _SubAdminService.SubAdmin_DDL();
        //        return Ok(strResponse);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"Controller : uspmstSubAdmin_DDL", 1);
        //        return BadRequest(new { status = 0, data = 0, message = msgError });
        //    }
        //}
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("subadmin-delete")]
        public async Task<IActionResult> SubAdmin_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubAdminService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _SubAdminService.SubAdmin_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubAdmin_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE


        #region GET Permission By ModuleIDF
        [HttpPost]
        [Route("subadmin-get-permission-by-moduleIDF")]
        public async Task<IActionResult> SubAdmin_Get_Permission_By_ModuleIDF([FromBody] Model_Get_Permission paramGetPermission)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubAdminService.ObjUser = objUser;

                var strResponse = await _SubAdminService.SubAdmin_Get_Permission_By_ModuleIDF(paramGetPermission);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspSubAdmin_Get_Permission_By_ModuleIDF", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET Permission By ModuleIDF


    }
}
