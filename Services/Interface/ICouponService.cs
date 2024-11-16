using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface ICouponService : ICommon
    {
        Task<Int64> Coupon_Insert(Coupon modelCoupon);
        Task<Int64> Coupon_Update(Coupon modelCoupon);
        Task<string> Coupon_Get(Int64 couponIDP);
        Task<string> Coupon_GetAll(ModelCommonGetAll param);
        Task<Int64> Coupon_ActiveInactive(Int64 couponIDP, Boolean isActive);
        Task<string> Coupon_DDL();
        Task<Int64> Coupon_Delete(Int64 couponIDP);
    }
}
