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
    public class CityController : CommonController
    {
        private readonly ICityServices _cityServices;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cityServices"></param>
        public CityController(BMSContext context, ICityServices cityServices, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _cityServices = cityServices;
        }

        #region INSERT-UPDATE
        // POST api/City/city-insert-update
        [HttpPost]
        [Route("city-insert-update")]
        public async Task<IActionResult> City_InsertUpdate([FromBody] TblCity tblCity)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _cityServices.ObjUser = objUser;

                string paramIdentityAction = " ";
                Int64 paramIdentity = 0;

                if (tblCity.CityIdp> 0)
                {
                    paramIdentity = await _cityServices.City_Update(tblCity);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _cityServices.City_Insert(tblCity);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;
                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch(Exception e)
            {
                await _cityServices.ErrorLog(1, e.Message, $"Controller : City_InsertUpdate", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion INSERT-UPDATE

        #region GET
        // POST api/City/city-get
        [HttpPost]
        [Route("city-get")]
        public async Task<IActionResult> City_Get([FromBody]ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _cityServices.ObjUser = objUser;

                var strResponse = await _cityServices.City_Get(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _cityServices.ErrorLog(1, e.Message, $"Controller : City_Get", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion City_Get

        #region City_GetAll

        [HttpPost]
        [Route("city-get-all")]
        public async Task<IActionResult> City_GetAll([FromBody]ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _cityServices.ObjUser = objUser;

                var strResponse = await _cityServices.City_GetAll(modelCommonGetAll);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _cityServices.ErrorLog(1, e.Message, $"Controller : City_GetAll", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion City_GetAll

        #region ACTIVE-INACTIVE
        [Route("city-active-inactive")]
        [HttpPost]
        public async Task<IActionResult> City_Active_InActive([FromBody] ModelActiveInactive modelActiveInactive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _cityServices.ObjUser = objUser;

                var strResponse = await _cityServices.City_ActiveInactive(modelActiveInactive.entityID, modelActiveInactive.isActive);

                return Ok(new { status = 1, data = strResponse, message = msgUpdated });
            }
            catch (Exception e)
            {
                await _cityServices.ErrorLog(1, e.Message, $"Controller : City_Active_InActive", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE
        // POST api/City/city-delete
        [HttpPost]
        [Route("city-delete")]
        public async Task<IActionResult> City_Delete([FromBody]ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _cityServices.ObjUser = objUser;

                Int64 responseIdentity = await _cityServices.City_Delete(modelCommonGet.Id);

                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;
                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await _cityServices.ErrorLog(1, e.Message, $"Controller : City_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DELETE

        #region DDL
        // POST api/City/city-get-all
        [HttpPost]
        [Route("city-get-ddl")]
        public async Task<IActionResult> City_DDL(ModelCommonGet modelCommonGet)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _cityServices.ObjUser = objUser;

                var strResponse = await _cityServices.City_DDL(modelCommonGet.Id);
                return Ok(strResponse);
            }
            catch(Exception e)
            {
                await _cityServices.ErrorLog(1, e.Message, $"Controller : City_DDL", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DDL

        #region DDL-FinyAny
        [HttpPost]
        [Route("city-ddl-finyAny")]
        public async Task<IActionResult> City_DDL_FinyAny(ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _cityServices.ObjUser = objUser;

                var strResponse = await _cityServices.City_DDL_FindAny(modelCommonGetAll.SearchKeyWord);
                return Ok(strResponse);
            }
            catch (Exception e)
            {
                await _cityServices.ErrorLog(1, e.Message, $"Controller : City_DDL_FindAny", 1);
                return BadRequest(new { status = 0, data = 0, message = e.Message });
            }
        }
        #endregion DDL

    }
}
