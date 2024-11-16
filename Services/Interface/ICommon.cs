using BudgetManagement.Models.Utility;
using BudgetManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace BMS_API.Services.Interface
{
    public interface ICommon
    {
        enum UserType
        {
            Admin = 1,
            Franchise = 2,
            User = 3,
            Vendor = 4,
        }
        enum EmailTemplateType
        {
            Registration = 1,
            LoginOTP = 2,
            ForgotPassword = 3,
        }
        AuthorisedUser ObjUser { get; set; }
        //AuthorisedUser ObjVendor { get; set; }
        Task<bool> ErrorLog(long userId, string errorMessage, string errorAction, int errorCode);
    }
}
