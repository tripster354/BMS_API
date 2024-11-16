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
    public class CountryController : CommonController
    {
        private readonly lCountryServices _countryServices;
        private readonly IWebHostEnvironment webHostEnvironment;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="countryServices"></param>
        public CountryController(BMSContext context, lCountryServices countryServices, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _countryServices = countryServices;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region INSERT-UPDATE
        // POST api/Country/country-insert-update
        [HttpPost]
        [Route("country-insert-update")]
        public async Task<IActionResult> Country_InsertUpdate([FromForm] TblCountry tblCountry)
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
                    directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Assets", "Country");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    tempDirectoryPath = Path.Combine(directoryPath, "Temp");
                    if (!Directory.Exists(tempDirectoryPath))
                    {
                        Directory.CreateDirectory(tempDirectoryPath);
                    }
                    if (file1.Length > 0)
                    {
                        var fileType = Path.GetExtension(file1.FileName);
                        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file1.FileName);
                        using (var fileStream = new FileStream(Path.Combine(tempDirectoryPath, fileName), FileMode.Create))
                        {
                            await file1.CopyToAsync(fileStream);
                        }
                        tblCountry.CountryFlag = fileName;
                    }
                }
                _countryServices.ObjUser = objUser;
                if (tblCountry.CountryIdp > 0)
                {
                    paramIdentity = await _countryServices.Country_Update(tblCountry);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _countryServices.Country_Insert(tblCountry);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;

                if (files.Count > 0)
                {
                    string newDirectoryPath = Path.Combine(directoryPath, paramIdentity.ToString());
                    if (!Directory.Exists(newDirectoryPath))
                    {
                        Directory.CreateDirectory(newDirectoryPath);
                    }
                    if (Directory.Exists(tempDirectoryPath))
                    {
                        foreach (var file in new DirectoryInfo(tempDirectoryPath).GetFiles())
                        {
                            file.MoveTo($@"{newDirectoryPath}\{file.Name}");
                        }
                    }
                }

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch(Exception e)
            {
                await _countryServices.ErrorLog(1, e.Message, $"Controller : Country_InsertUpdate", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion INSERT-UPDATE

        #region GET
        // POST api/Country/country-get
        [HttpPost]
        [Route("country-get")]
        public async Task<IActionResult> Country_Get([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _countryServices.ObjUser = objUser;

                var strResponse = await _countryServices.Country_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _countryServices.ErrorLog(1, e.Message, $"Controller : Country_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET

        #region GET-ALL
        // POST api/Country/country-get-all
        [HttpPost]
        [Route("country-get-all")]
        public async Task<IActionResult> Country_GetAll([FromBody] ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _countryServices.ObjUser = objUser;

                var strResponse = await _countryServices.Country_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _countryServices.ErrorLog(1, e.Message, $"Controller : Country_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        [Route("country-active-inactive")]
        [HttpPost]
        public async Task<IActionResult> Country_Active_InActive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _countryServices.ObjUser = objUser;

                var strResponse = await _countryServices.Country_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);

                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await _countryServices.ErrorLog(1, e.Message, $"Controller : Country_Active_InActive", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE
        // POST api/Country/country-delete
        [HttpPost]
        [Route("country-delete")]
        public async Task<IActionResult> Country_Delete([FromBody] ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _countryServices.ObjUser = objUser;

                Int64 responseIdentity = await _countryServices.Country_Delete(modelCommonGet.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;
                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch(Exception e)
            {
                await _countryServices.ErrorLog(1, e.Message, $"Controller : Country_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DELETE

        #region DDL
        // POST api/Country/country-get-ddl
        [HttpPost]
        [Route("country-get-ddl")]
        public async Task<IActionResult> Country_DDL()
        {
            try
            {
                //GetAuth();
                //if (objUser == null) return BadRequest(authFail);
                //_countryServices.ObjUser = objUser;

                var strResponse = await _countryServices.Country_DDL();
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _countryServices.ErrorLog(1, e.Message, $"Controller : Country_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DDL

        #region Currency-DDL
        // POST api/Country/country-get-ddl
        [HttpPost]
        [Route("countrycurrency-get-ddl")]
        public async Task<IActionResult> CountryCurrency_DDL()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _countryServices.ObjUser = objUser;

                var strResponse = await _countryServices.CountryCurrency_DDL();
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await _countryServices.ErrorLog(1, e.Message, $"Controller : CountryCurrency_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DDL
    }
}
