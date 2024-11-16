using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using BudgetManagement.Services;

namespace BudgetManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthCommonController : Controller
    {
        public BMSContext _context;
        public AuthorisedUser objUser;
        public string authFail = "Authentation Fail";

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public DateTime Indian_Current_DateTime
        {
            get
            {
                //Apply Indian date here
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                return indianTime;
            }
        }
        public AuthCommonController(BMSContext context)
        {
            _context = context;
        }
        [NonAction]
        public async void GetAuth()
        {
            // objUser = await UserAuth();
        }
        [NonAction]
        public async Task<Boolean> ErrorLog(Int64 userId, string errorMessage, string errorAction, Int32 errorCode)
        {
            /*
            SysErrorLog newError = new SysErrorLog();

            try
            {
                newError.UserIdf = userId;
                newError.RowGuid = Guid.NewGuid();
                newError.AppId = 1;
                newError.AppReleaseVersion = 1;
                newError.ErrorMessage = errorMessage;
                newError.ErrorCode = errorCode;
                newError.ErrorDate = Indian_Current_DateTime;//Indian Datetime

                _context.Add(newError);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
            */
            return true;
        }

        [NonAction]
        public async Task<Boolean> AuditLog(Int64 userId, string auditMessage)
        {
            /*
            SysAuditLog newAudit = new SysAuditLog();

            newAudit.UserIdf = userId;
            newAudit.RowGuid = Guid.NewGuid();
            newAudit.AppId = 1;
            newAudit.AppReleaseVersion = 1;
            newAudit.AuditMessage = auditMessage;
            newAudit.AuditDate = DateTime.Now;

            _context.Add(newAudit);
            await _context.SaveChangesAsync();
            */
            return true;
        }

        /*
         [NonAction]
        public async Task<AuthorisedUser> UserAuth()
        {
            try
            {

                long xToken = 0;
                byte xUerType = 0;
                long.TryParse(Request.Headers["Token"][0], out xToken);
                byte.TryParse(Request.Headers["UserType"][0], out xUerType);
                if (xToken > 0 && xUerType > 0)
                {
                    AuthorisedUser objUser = await _authService.Login_By_Token(xToken, (ICommon.UserType)xUerType);
                    return objUser;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        */
    }
}
