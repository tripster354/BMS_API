using BudgetManagement.Models;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using BMS_API.Models.DTOs;
using System.Collections.Generic;

namespace BMS_API.Services.Interface
{
    public interface IUserManagementService : ICommon
    {
        Task<Int64> UserManagement_Insert(ModelUserManagement modelUserManagement);
        Task<Int64> UserManagement_Update(ModelUserManagement modelUserManagement);
        Task<string> UserManagement_Get(Int64 userIDP);
        Task<string> UserManagement_Get_Invite(Int64 userIDP);
        Task<string> UserManagement_GetAll(ModelCommonGetAll param);
        Task<Int64> UserManagement_ActiveInactive(Int64 userIDP, Boolean isActive);
        Task<string> UserManagement_DDL();
        Task<Int64> UserManagement_Delete(Int64 userIDP);
        Task<Int64> User_Update_MyProfile(ModelUserManagement modelUserManagement);
        Task<Int64> User_Update_MyCredential(ModelUpdateCredential updateCredential);
        Task<Int64> User_Update_MyNotification(ModelNotification modelNotification);

        Task<VendorLoginResponse> LoginWithMobileOTP(string mobileNumber, int otp);
        Task<List<ActivityInterestDTO>> GetAllInterestActivityNames();
        Task<ModelUserManagementDTO> GetVendorById(long userId);
        Task<int> SendOtponMobile(string loginInput);
    }
}
