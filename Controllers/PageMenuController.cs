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

    public class PageMenuController : CommonController
    {
        private readonly IPageMenuService _pageMenuService;

        public PageMenuController(BMSContext context, IPageMenuService pageMenuService, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _pageMenuService = pageMenuService;
        }

        #region pagemenu insert-update
        [HttpPost]
        [Route("pagemenu-insert-update")]
        public async Task<IActionResult> PageMenu_InsertUpdate([FromBody] TblPageMenu tblPageMenu)
        {
            try
            {
                if (tblPageMenu == null) return BadRequest();

                GetAuth();
                if (objUser == null) return BadRequest(authFail);

                string paramIdentityAction = "";
                Int32 paramIdentity = 0;

                _pageMenuService.ObjUser = objUser;

                if (tblPageMenu.PageMenuIDP > 0)
                {
                    paramIdentity = await _pageMenuService.PageMenu_Update(tblPageMenu);
                    paramIdentityAction = msgUpdated;
                }
                else
                {
                    paramIdentity = await _pageMenuService.PageMenu_Insert(tblPageMenu);
                    paramIdentityAction = msgInserted;
                }
                if (paramIdentity == -1) paramIdentityAction = msgDuplicate;
                if (paramIdentity == 0) paramIdentityAction = msgError;

                return Ok(new { status = 1, data = paramIdentity, message = paramIdentityAction });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspPageMenu_Insert", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
                //return BadRequest("Something went wrong trying to insert pagemenu.");
            }
        }
        #endregion pagemenu insert-update

        #region pagemenu delete
        [HttpPost]
        [Route("pagemenu-delete")]
        public async Task<IActionResult> PageMenu_Delete([FromBody] ModelCommonGet paramPageInfo)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _pageMenuService.ObjUser = objUser;

                Int32 responseIdentity = (int)await _pageMenuService.PageMenu_Delete(paramPageInfo.Id);
                string responseMsg = (responseIdentity > 0) ? msgDeleted : msgDeleted_Fail;

                return Ok(new { status = 1, data = responseIdentity, message = responseMsg });
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"Controller : uspmstPageMenu_Delete", 1);
                return BadRequest(new { status = 0, data = 0, message = msgError });
                //return BadRequest("Something went wrong trying to delete pagemenu.");
            }
        }
        #endregion pagemenu delete

        #region pagemenu get
        [HttpPost]
        [Route("pagemenu-get")]
        public async Task<IActionResult> PageMenu_Get([FromBody] ModelCommonGet paramPageInfo)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _pageMenuService.ObjUser = objUser;

                var paramDepartment = await _pageMenuService.PageMenu_Get(paramPageInfo.Id);
                return Ok(paramDepartment);
            }
            catch
            {
                return BadRequest("Something went wrong trying to get pagemenu.");
            }
        }
        #endregion pagemenu get

        #region pagemenu get all
        [HttpPost]
        [Route("pagemenu-get-all")]
        public async Task<IActionResult> PageMenu_GetAll()
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _pageMenuService.ObjUser = objUser;

                var paramDepartmentList = await _pageMenuService.PageMenu_GetAll();
                return Ok(paramDepartmentList);
            }
            catch
            {
                return BadRequest("Something went wrong trying to get pagemenu");
            }
        }
        #endregion pagemenu get all

        #region pagemenu  active inactive
        [HttpPost]
        [Route("pagemenu-active-inactive")]
        public async Task<IActionResult> PageMenu_Active_InActive([FromBody] ModelActiveInactive paramPageInfoActive)
        {
            try
            {
                GetAuth();
                if (objUser == null) return BadRequest(authFail);
                _pageMenuService.ObjUser = objUser;

                Int32 ParamIdentity = await _pageMenuService.PageMenu_Active_InActive(paramPageInfoActive.entityID, paramPageInfoActive.isActive);
                return Ok(ParamIdentity);
            }
            catch
            {
                return BadRequest("Something went wrong trying to active inactive pagemenu.");
            }
        }
        #endregion pagemenu  active inactive

        #region pagemenu get detail
        [HttpPost]
        [Route("pagemenu-get-detail")]
        public async Task<IActionResult> PageMenu_GetDetial([FromBody] ParamPageInfoDetail paramPageInfoDetail)
        {
            try
            {
                var paramDepartment = await _pageMenuService.PageMenu_GetDetail(paramPageInfoDetail.pageName);
                return Ok(paramDepartment);
            }
            catch
            {
                return BadRequest("Something went wrong trying to get pagemenu detail.");
            }
        }
        #endregion pagemenu get detail

    }
}
