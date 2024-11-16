using BMS_API.Services.Interface;
using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using BudgetManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading.Tasks;

namespace BudgetManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StateController : CommonController
    {
        private readonly IStateServices _stateServices;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="stateServices"></param>
        public StateController(BMSContext context, IStateServices stateServices, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _stateServices = stateServices;
        }

        #region INSERT-UPDATE
        // POST api/State/state-insert-update
        [HttpPost]
        [Route("state-insert-update")]
        public async Task<IActionResult> State_InsertUpdate([FromBody] Tblstate tblstate)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _stateServices.ObjUser = objUser;

                string paramIdentityAction = " ";
                Int64 paramIdentity = 0;

                if (tblstate.StateIdp> 0)
                {
                    paramIdentity = await _stateServices.State_Update(tblstate);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _stateServices.State_Insert(tblstate);
                    paramIdentityAction = msgInserted;

                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;
                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch(Exception e)
            {
                await _stateServices.ErrorLog(1, e.Message, $"Controller : State_InsertUpdate", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion INSERT-UPDATE

        #region GET
        // POST api/State/state-get
        [HttpPost]
        [Route("state-get")]
        public async Task<IActionResult> State_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _stateServices.ObjUser = objUser;

                var strResponse = await _stateServices.State_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _stateServices.ErrorLog(1, e.Message, $"Controller : State_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET

        #region GET-ALL

        [HttpPost]
        [Route("state-get-all")]
        public async Task<IActionResult> State_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _stateServices.ObjUser = objUser;

                var strResponse = await _stateServices.State_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _stateServices.ErrorLog(1, e.Message, $"Controller : State_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        [Route("state-active-inactive")]
        [HttpPost]
        public async Task<IActionResult> State_Active_InActive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _stateServices.ObjUser = objUser;

                var strResponse = await _stateServices.State_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);

                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await _stateServices.ErrorLog(1, e.Message, $"Controller : State_Active_InActive", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE
        // POST api/State/state-delete
        [HttpPost]
        [Route("state-delete")]
        public async Task<IActionResult> State_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _stateServices.ObjUser = objUser;

                Int64 responseIdentity = await _stateServices.State_Delete(modelCommonGet.Id);
                
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;
                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch(Exception e)
            {
                await _stateServices.ErrorLog(1, e.Message, $"Controller : State_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DELETE

        #region DDL
        // POST api/State/state-get-ddl
        [HttpPost]
        [Route("state-get-ddl")]
        public async Task<IActionResult> State_DDL([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _stateServices.ObjUser = objUser;

                var strResponse = await _stateServices.State_DDL(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _stateServices.ErrorLog(1, e.Message, $"Controller : State_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DDL
    }
}
