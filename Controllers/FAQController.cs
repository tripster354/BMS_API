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

    public class FAQController : CommonController
    {
        private readonly IFAQService _FAQService;

        public FAQController(BMSContext context, IFAQService __FAQService, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _FAQService = __FAQService;
        }


        #region INSERT-UPDATE
        [HttpPost]
        [Route("faq-insert-update")]
        public async Task<IActionResult> FAQ_InsertUpdate([FromForm] FAQ modelFAQ)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _FAQService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;
                if (modelFAQ.FAQIDP > 0)
                {
                    paramIdentity = await _FAQService.FAQ_Update(modelFAQ);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _FAQService.FAQ_Insert(modelFAQ);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstFAQ_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion INSERT-UPDATE


        #region GET
        [HttpPost]
        [Route("faq-get")]
        public async Task<IActionResult> FAQ_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _FAQService.ObjUser = objUser;

                var strResponse = await _FAQService.FAQ_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstFAQ_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET


        #region GET-ALL
        [HttpPost]
        [Route("faq-get-all")]
        public async Task<IActionResult> FAQ_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _FAQService.ObjUser = objUser;

                var strResponse = await _FAQService.FAQ_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstFAQ_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion GET-ALL


        #region ACTIVE-INACTIVE
        [HttpPost]
        [Route("faq-active-inactive")]
        public async Task<IActionResult> FAQ_ActiveInactive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _FAQService.ObjUser = objUser;

                var strResponse = await _FAQService.FAQ_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);
                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstFAQ_Update_ActiveInActive", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion ACTIVE-INACTIVE


        #region DDL
        [HttpPost]
        [Route("faq-ddl")]
        public async Task<IActionResult> FAQ_DDL()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _FAQService.ObjUser = objUser;

                var strResponse = await _FAQService.FAQ_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstFAQ_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DDL


        #region DELETE
        [HttpPost]
        [Route("faq-delete")]
        public async Task<IActionResult> FAQ_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _FAQService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _FAQService.FAQ_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstFAQ_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
            }
        }
        #endregion DELETE
    }
}
