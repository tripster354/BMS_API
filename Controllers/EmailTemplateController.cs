using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models.Utility;
using BMS_API.Services.Interface;

namespace BudgetManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailTemplateController : CommonController
    {
        private readonly IEmailTemplateService _emailTemplateService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="authService"></param>
        public EmailTemplateController(BMSContext context, IEmailTemplateService emailTemplateService, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _emailTemplateService = emailTemplateService;
        }

        #region UPDATE
        // POST api/EmailTemplate/email-template-update
        [HttpPost]
        [Route("email-template-update")]
        public async Task<IActionResult> EmailTemplate_Update([FromBody] MstEmailTemplate mstEmail)
        {
            try
            {
                if (mstEmail == null) return BadRequest();

                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _emailTemplateService.ObjUser = objUser;

                string paramIdentityAction = "";
                Int64 paramIdentity = 0;

                paramIdentity = await _emailTemplateService.EmailTemplate_Update(mstEmail);
                paramIdentityAction = msgUpdated;
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;
                
                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch (Exception e)
            {
                await _emailTemplateService.ErrorLog(1, e.Message, $"Controller : EmailTemplate_Update", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion UPDATE

        #region GET
        // POST api/EmailTemplate/email-template-get
        [HttpPost]
        [Route("email-template-get")]
        public async Task<IActionResult> EmailTemplate_Get([FromBody] ModelCommonGet commonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _emailTemplateService.ObjUser = objUser;

                var strResponse = await _emailTemplateService.EmailTemplate_Get(commonGet.Id);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _emailTemplateService.ErrorLog(1, e.Message, $"Controller : EmailTemplate_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET

        #region GET-BY-TemplateType
        // POST api/EmailTemplate/email-template-get-by-template-type
        [HttpPost]
        [Route("email-template-get-by-template-type")]
        public async Task<IActionResult> EmailTemplate_GetByTemplateType([FromBody] ModelCommonGet commonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _emailTemplateService.ObjUser = objUser;

                var strResponse = await _emailTemplateService.EmailTemplate_GetByTemplateType(Convert.ToByte(commonGet.Id));
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _emailTemplateService.ErrorLog(1, e.Message, $"Controller : EmailTemplate_GetByTemplateType", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET

        #region GET ALL
        // POST api/EmailTemplate/email-template-get-all
        [HttpPost]
        [Route("email-template-get-all")]
        public async Task<IActionResult> EmailTemplate_GetAll()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _emailTemplateService.ObjUser = objUser;

                var strResponse = await _emailTemplateService.EmailTemplate_GetAll();
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _emailTemplateService.ErrorLog(1, e.Message, $"Controller : CompanyType_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET-ALL
    }
}
