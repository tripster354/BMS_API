using BMS_API.Services.Interface;
using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BudgetManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BankController : CommonController
    {
        private readonly lBankServices _bankServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        
        public BankController(BMSContext context, lBankServices bankServices, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _bankServices = bankServices;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region INSERT-UPDATE
        
        [HttpPost]
        [Route("bank-insert-update")]
        public async Task<IActionResult> Bank_InsertUpdate([FromForm] Bank bank)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);

                string paramIdentityAction = " ";
                Int64 paramIdentity = 0;

                string directoryPath = "";
                string tempDirectoryPath = "";

                // File upload
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var file1 = files[0];
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Bank");
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
                        bank.BankImage = fileName;
                    }
                }
                _bankServices.ObjUser = objUser;
                if (bank.BankIDP > 0)
                {
                    paramIdentity = await _bankServices.Bank_Update(bank);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _bankServices.Bank_Insert(bank);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;

               

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch(Exception e)
            {
                await _bankServices.ErrorLog(1, e.Message, $"Controller : Bank_InsertUpdate", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion INSERT-UPDATE

        #region GET
        [HttpPost]
        [Route("bank-get")]
        public async Task<IActionResult> Bank_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _bankServices.ObjUser = objUser;

                var strResponse = await _bankServices.Bank_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _bankServices.ErrorLog(1, e.Message, $"Controller : Bank_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET

        #region GET-ALL
        
        [HttpPost]
        [Route("bank-get-all")]
        public async Task<IActionResult> Bank_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _bankServices.ObjUser = objUser;

                var strResponse = await _bankServices.Bank_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _bankServices.ErrorLog(1, e.Message, $"Controller : Bank_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        [Route("bank-active-inactive")]
        [HttpPost]
        public async Task<IActionResult> Bank_Active_InActive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _bankServices.ObjUser = objUser;

                var strResponse = await _bankServices.Bank_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);

                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await _bankServices.ErrorLog(1, e.Message, $"Controller : Bank_Active_InActive", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE
        
        [HttpPost]
        [Route("bank-delete")]
        public async Task<IActionResult> Bank_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _bankServices.ObjUser = objUser;

                Int64 responseIdentity = await _bankServices.Bank_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;
                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch(Exception e)
            {
                await _bankServices.ErrorLog(1, e.Message, $"Controller : Bank_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DELETE

        #region DDL
        [HttpPost]
        [Route("bank-get-ddl")]
        public async Task<IActionResult> Bank_DDL()
        {
            try
            {
                //GetAuth();
                //if (objUser == null) return BadRequest(authFail);
                //_countryServices.ObjUser = objUser;

                var strResponse = await _bankServices.Bank_DDL();
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _bankServices.ErrorLog(1, e.Message, $"Controller : Bank_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DDL

      
    }
}
