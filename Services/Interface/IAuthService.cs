using System.Threading.Tasks;
using System;
using BudgetManagement.Models.Utility;
using BudgetManagement.Models;
using BMS_API.Models.Utility;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface IAuthService: ICommon
    {

        Task<LoginResponse> Login(AuthLogin authLogin, string ipAddress);
        Task<AuthorisedUser> Login_By_Token(long token, UserType userType);
        Task<string> Auth_PasswordForgot(string userName, Int32 userType);
        Task<bool> Auth_PasswordChange(AuthReset newPass);
        Task<bool> Auth_OTPVarification(AuthOTPVarification oTPVarification);
        Task<string> Auth_OTPRequest();
        Task<long> Auth_Admin_Change_Password(string userNmae, string password, string firstName, string lastName, string oldPassword);
        Task<bool> Auth_Admin_Change_Settings(SysAdmin adminInfo);
        Task<bool> Auth_Logout();
        Task<AuthorisedUser> VendorLogin_By_Token(long token, UserType xUserType);

    }
}
