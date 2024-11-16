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

namespace BMS_API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class RoleController : CommonController
    {
        private readonly IRoleService _RoleService;

        public RoleController(BMSContext context, IRoleService __RoleService, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _RoleService = __RoleService;
        }

        #region INSERT-UPDATE
        [HttpPost]
        [Route("role-insert-update")]
        public async Task<IActionResult> Role_InsertUpdate([FromForm] Role modelRole)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _RoleService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                if (modelRole.RoleIDP > 0)
                {
                    paramIdentity = await _RoleService.Role_Update(modelRole);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _RoleService.Role_Insert(modelRole);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstRole_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("role-get")]
        public async Task<IActionResult> Role_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _RoleService.ObjUser = objUser;

                var strResponse = await _RoleService.Role_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstRole_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("role-get-all")]
        public async Task<IActionResult> Role_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _RoleService.ObjUser = objUser;

                var strResponse = await _RoleService.Role_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstRole_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("role-active-inactive")]
        public async Task<IActionResult> Role_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _RoleService.ObjUser = objUser;

                var strResponse = await _RoleService.Role_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstRole_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DDL
        [HttpPost]
        [Route("role-ddl")]
        public async Task<IActionResult> Role_DDL()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _RoleService.ObjUser = objUser;

                var strResponse = await _RoleService.Role_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstRole_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("role-delete")]
        public async Task<IActionResult> Role_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _RoleService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _RoleService.Role_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstRole_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE
    }
}
