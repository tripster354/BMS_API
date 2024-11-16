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

    public class SubscriptionController : CommonController
    {
        private readonly ISubscriptionService _SubscriptionService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public SubscriptionController(BMSContext context, ISubscriptionService __SubscriptionService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _SubscriptionService = __SubscriptionService;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region INSERT-UPDATE
        [HttpPost]
        [Route("subscription-insert-update")]
        public async Task<IActionResult> Subscription_InsertUpdate([FromForm] Subscription modelSubscription)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubscriptionService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                if (modelSubscription.SubscriptionIDP > 0)
                {
                    paramIdentity = await _SubscriptionService.Subscription_Update(modelSubscription);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _SubscriptionService.Subscription_Insert(modelSubscription);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubscription_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("subscription-get")]
        public async Task<IActionResult> Subscription_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubscriptionService.ObjUser = objUser;

                var strResponse = await _SubscriptionService.Subscription_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubscription_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("subscription-get-all")]
        public async Task<IActionResult> Subscription_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubscriptionService.ObjUser = objUser;

                var strResponse = await _SubscriptionService.Subscription_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubscription_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("subscription-active-inactive")]
        public async Task<IActionResult> Subscription_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubscriptionService.ObjUser = objUser;

                var strResponse = await _SubscriptionService.Subscription_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubscription_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DELETE
        [HttpPost]
        [Route("subscription-delete")]
        public async Task<IActionResult> Subscription_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _SubscriptionService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _SubscriptionService.Subscription_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstSubscription_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE

    }
}
