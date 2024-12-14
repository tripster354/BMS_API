using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using BudgetManagement.Models;
using BMS_API.Services.Interface;

namespace BudgetManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommonController : Controller
    {
        public BMSContext _context;
        public IAuthService _authService;
        public AuthorisedUser objUser;
        public AuthorisedUser objvendor;
        public string authFail = "Authentation Fail";
        public string msgInserted = "Inserted successfully";
        public string msgUpdated = "Updated successfully";
        public string msgDeleted = "Deleted successfully";
        public string msgDuplicate = "Duplicate record found!";
        public string msgInvite = "Invitation sent successfully";
        public string msgDeleted_Fail = "Can not delete, due to other reference dependence";
        public string msgError = "Oops! Seems like some problem.";
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public string connectionString = "Data Source=43.204.37.101;Initial Catalog=dbBookMySkill;Persist Security Info=True;User ID=sa;Password=cId1NzffgIg;";

        public DateTime Indian_Current_DateTime
        {
            get
            {
                //Apply Indian date here
                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                return indianTime;
            }
        }
        public CommonController(BMSContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        [NonAction]
        public async Task GetAuth()
        {
            if (Request.Headers.ContainsKey("UserType"))
            {
                if ((Request.Headers["UserType"].FirstOrDefault()) == "4")
                {
                    objUser = await VendorAuth();
                }
                else
                {
                    objUser = await UserAuth();
                }
            }
            else
            {
                // Log if userTypeValue is empty
                Console.WriteLine("UserType header is not present or is empty.");
            }
        }

        [NonAction]
        public async Task<Boolean> ErrorLog(Int64 userId, string errorMessage, string errorAction, Int32 errorCode)
        {
            try
            {
                /* SysErrorLog newError = new SysErrorLog();
                
                newError.UserIdf = userId;
                newError.RowGuid = Guid.NewGuid();
                newError.AppId = 1;
                newError.AppReleaseVersion = 1;
                newError.ErrorMessage = errorMessage;
                newError.ErrorCode = errorCode;
                newError.ErrorAction = errorAction;
                newError.ErrorDate = Indian_Current_DateTime;//Indian Datetime
                _context.Add(newError);
                await _context.SaveChangesAsync();
                */
            }
            catch (Exception e)
            {

            }
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

        [NonAction]
        public async Task<AuthorisedUser> UserAuth()
        {
            try
            {
                long xToken = 0;
                byte xUserType = 0;

                // Check if headers "Token" and "UserType" exist and are not empty
                if (Request.Headers.ContainsKey("Token") && !string.IsNullOrEmpty(Request.Headers["Token"].FirstOrDefault()) &&
                    Request.Headers.ContainsKey("UserType") && !string.IsNullOrEmpty(Request.Headers["UserType"].FirstOrDefault()))
                {
                    // Safely try to parse the values
                    long.TryParse(Request.Headers["Token"].FirstOrDefault(), out xToken);
                    byte.TryParse(Request.Headers["UserType"].FirstOrDefault(), out xUserType);

                    // Ensure token and user type have valid values
                    if (xToken > 0 && xUserType > 0)
                    {
                        AuthorisedUser objUser = await _authService.Login_By_Token(xToken, (ICommon.UserType)xUserType);
                        return objUser;
                    }
                }

                // Return null if validation fails
                return null;
            }
            catch (Exception e)
            {
                // Log the exception here if needed
                return null;
            }
        }
        [NonAction]
        public async Task<AuthorisedUser> VendorAuth()
        {
            try
            {
                long token = 0; // Change to string
                byte xUserType = 0;

                if (Request.Headers.ContainsKey("Token") && !string.IsNullOrEmpty(Request.Headers["Token"].FirstOrDefault()) &&
                    Request.Headers.ContainsKey("UserType") && !string.IsNullOrEmpty(Request.Headers["UserType"].FirstOrDefault()))
                {
                    //token = Request.Headers["Token"].FirstOrDefault(); // Keep it as string
                    long.TryParse(Request.Headers["Token"].FirstOrDefault(), out token);
                    byte.TryParse(Request.Headers["UserType"].FirstOrDefault(), out xUserType);

                    if (token > 0 && xUserType == 4) // Check if token is not null or empty
                    {
                        // Pass the token as a string
                        AuthorisedUser objVendor = await _authService.VendorLogin_By_Token(token, (ICommon.UserType)xUserType);
                        return objVendor;
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                // Optionally log the exception here
                return null;
            }
        }


        [NonAction]
        public string CreateRandomPassword()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string caractere = "!-_*+&$";
            const string number = "0123456789";

            string[] all = new[] { upper, lower, caractere, number };

            const int PwdLength = 6;
            const int CharCategories = 4;
            const int CharCatLimit = PwdLength / CharCategories;

            Random rand = new Random();

            var password = GetRandCountRanCharsFromStr(upper, CharCatLimit, rand)
                            .Concat(GetRandCountRanCharsFromStr(lower, CharCatLimit, rand))
                            .Concat(GetRandCountRanCharsFromStr(caractere, CharCatLimit, rand))
                            .Concat(GetRandCountRanCharsFromStr(number, CharCatLimit, rand)).ToArray();
            if (password.Length < PwdLength)
            {
                password = password.Concat(GetRandCharsFromStr(all[rand.Next(0, all.Length)], PwdLength - password.Length, rand))
                            .ToArray();
            }

            Shuffle(password, rand);

            return new string(password);

        }
        private void Shuffle(char[] source, Random rand)
        {
            int n = source.Length;

            while (n > 1)
            {
                --n;
                int k = rand.Next(n + 1);
                var temp = source[k];
                source[k] = source[n];
                source[n] = temp;
            }
        }

        private IEnumerable<char> GetRandCountRanCharsFromStr(string source, int count, Random rand)
        {
            int randLimit = rand.Next(1, count);
            for (int i = 0; i < randLimit; ++i)
            {
                yield return source[rand.Next(0, source.Length)];
            }
        }

        private IEnumerable<char> GetRandCharsFromStr(string source, int count, Random rand)
        {
            for (int i = 0; i < count; ++i)
                yield return source[rand.Next(0, source.Length)];
        }
    }
}
