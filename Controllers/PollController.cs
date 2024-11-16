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

    public class PollController : CommonController
    {
        private readonly IPollService _PollService;

        public PollController(BMSContext context, IPollService __PollService, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _PollService = __PollService;
        }

        #region INSERT-UPDATE
        [HttpPost]
        [Route("poll-insert-update")]
        public async Task<IActionResult> Poll_InsertUpdate([FromForm] Poll modelPoll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _PollService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                if (modelPoll.PollIDP > 0)
                {
                    paramIdentity = await _PollService.Poll_Update(modelPoll);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _PollService.Poll_Insert(modelPoll);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == 2) paramIdentityAction = "Poll already exist in this duration.";
                if (paramIdentity == 1) paramIdentityAction = msgDuplicate;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstPoll_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("poll-get")]
        public async Task<IActionResult> Poll_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _PollService.ObjUser = objUser;

                var strResponse = await _PollService.Poll_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstPoll_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("poll-get-all")]
        public async Task<IActionResult> Poll_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _PollService.ObjUser = objUser;

                var strResponse = await _PollService.Poll_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstPoll_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("poll-active-inactive")]
        public async Task<IActionResult> Poll_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _PollService.ObjUser = objUser;

                var strResponse = await _PollService.Poll_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstPoll_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DDL
        //[HttpPost]
        //[Route("poll-ddl")]
        //public async Task<IActionResult> Poll_DDL()
        //{
        //    try
        //    {
        //        GetAuth();
        //        if (objUser == null) return BadRequest(authFail);
        //        _PollService.ObjUser = objUser;

        //        var strResponse = await _PollService.Poll_DDL();
        //        return Ok(strResponse);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"Controller : uspmstPoll_DDL", 1);
        //        return BadRequest(new { status = 0, data = 0, message = msgError });
        //    }
        //}
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("poll-delete")]
        public async Task<IActionResult> Poll_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _PollService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _PollService.Poll_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstPoll_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE
    }
}
