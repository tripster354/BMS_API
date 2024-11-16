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

    public class TicketIssueTypeController : CommonController
    {
        private readonly ITicketIssueTypeService _TicketIssueTypeService;

        public TicketIssueTypeController(BMSContext context, ITicketIssueTypeService __TicketIssueTypeService, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _TicketIssueTypeService = __TicketIssueTypeService;
        }


        #region INSERT-UPDATE
        [HttpPost]
        [Route("ticketissuetype-insert-update")]
        public async Task<IActionResult> TicketIssueType_InsertUpdate([FromForm] TicketIssueType modelTicketIssueType)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _TicketIssueTypeService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                if (modelTicketIssueType.TicketIssueTypeIDP > 0)
                {
                    paramIdentity = await _TicketIssueTypeService.TicketIssueType_Update(modelTicketIssueType);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _TicketIssueTypeService.TicketIssueType_Insert(modelTicketIssueType);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstTicketIssueType_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("ticketissuetype-get")]
        public async Task<IActionResult> TicketIssueType_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _TicketIssueTypeService.ObjUser = objUser;

                var strResponse = await _TicketIssueTypeService.TicketIssueType_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstTicketIssueType_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("ticketissuetype-get-all")]
        public async Task<IActionResult> TicketIssueType_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _TicketIssueTypeService.ObjUser = objUser;

                var strResponse = await _TicketIssueTypeService.TicketIssueType_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstTicketIssueType_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("ticketissuetype-active-inactive")]
        public async Task<IActionResult> TicketIssueType_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _TicketIssueTypeService.ObjUser = objUser;

                var strResponse = await _TicketIssueTypeService.TicketIssueType_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstTicketIssueType_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DDL
        [HttpPost]
        [Route("ticketissuetype-ddl")]
        public async Task<IActionResult> TicketIssueType_DDL()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _TicketIssueTypeService.ObjUser = objUser;

                var strResponse = await _TicketIssueTypeService.TicketIssueType_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstTicketIssueType_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("ticketissuetype-delete")]
        public async Task<IActionResult> TicketIssueType_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _TicketIssueTypeService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _TicketIssueTypeService.TicketIssueType_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstTicketIssueType_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE
    }
}
