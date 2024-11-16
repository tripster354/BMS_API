using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services
{
    public class CouponService : CommonService, ICouponService
    {
        public CouponService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region Coupon INSERT
        public async Task<Int64> Coupon_Insert(Coupon modelCoupon)
        {
            try
            {
                SqlParameter paramCouponIDP = new SqlParameter
                {
                    ParameterName = "@CouponIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramPromoCode = new SqlParameter("@PromoCode", (object)modelCoupon.PromoCode ?? DBNull.Value);
                SqlParameter paramAttachment = new SqlParameter("@Attachment", (object)modelCoupon.Attachment ?? DBNull.Value);
                SqlParameter paramDiscountTypeID = new SqlParameter("@DiscountTypeID", (object)modelCoupon.DiscountTypeID ?? DBNull.Value);
                SqlParameter paramDiscountValue = new SqlParameter("@DiscountValue", (object)modelCoupon.DiscountValue ?? DBNull.Value);
                SqlParameter paramStartDate = new SqlParameter("@StartDate", (object)modelCoupon.StartDate ?? DBNull.Value);
                SqlParameter paramEndDate = new SqlParameter("@EndDate", (object)modelCoupon.EndDate ?? DBNull.Value);
                SqlParameter paramCouponCode = new SqlParameter("@CouponCode", (object)modelCoupon.CouponCode ?? DBNull.Value);
                SqlParameter paramCouponCodeGenerateNo = new SqlParameter("@CouponCodeGenerateNo", (object)modelCoupon.CouponCodeGenerateNo ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelCoupon.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstCoupon_Insert @CouponIDP OUTPUT, @PromoCode, @Attachment, @DiscountTypeID, @DiscountValue, @StartDate, @EndDate, @CouponCode, @CouponCodeGenerateNo, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCouponIDP, paramPromoCode, paramAttachment, paramDiscountTypeID, paramDiscountValue, paramStartDate, paramEndDate, paramCouponCode, paramCouponCodeGenerateNo, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramCouponIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCoupon_Insert", 1);
                return 0;
            }
        }
        #endregion Coupon INSERT


        #region Coupon UPDATE
        public async Task<Int64> Coupon_Update(Coupon modelCoupon)
        {
            try
            {
                SqlParameter paramCouponIDP = new SqlParameter("@CouponIDP", modelCoupon.CouponIDP);
                SqlParameter paramPromoCode = new SqlParameter("@PromoCode", (object)modelCoupon.PromoCode ?? DBNull.Value);
                SqlParameter paramAttachment = new SqlParameter("@Attachment", (object)modelCoupon.Attachment ?? DBNull.Value);
                SqlParameter paramDiscountTypeID = new SqlParameter("@DiscountTypeID", (object)modelCoupon.DiscountTypeID ?? DBNull.Value);
                SqlParameter paramDiscountValue = new SqlParameter("@DiscountValue", (object)modelCoupon.DiscountValue ?? DBNull.Value);
                SqlParameter paramStartDate = new SqlParameter("@StartDate", (object)modelCoupon.StartDate ?? DBNull.Value);
                SqlParameter paramEndDate = new SqlParameter("@EndDate", (object)modelCoupon.EndDate ?? DBNull.Value);
                SqlParameter paramCouponCode = new SqlParameter("@CouponCode", (object)modelCoupon.CouponCode ?? DBNull.Value);
                SqlParameter paramCouponCodeGenerateNo = new SqlParameter("@CouponCodeGenerateNo", (object)modelCoupon.CouponCodeGenerateNo ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelCoupon.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstCoupon_Update @CouponIDP, @PromoCode, @Attachment, @DiscountTypeID, @DiscountValue, @StartDate, @EndDate, @CouponCode, @CouponCodeGenerateNo, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCouponIDP, paramPromoCode, paramAttachment, paramDiscountTypeID, paramDiscountValue, paramStartDate, paramEndDate, paramCouponCode, paramCouponCodeGenerateNo, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramCouponIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCoupon_Update", 1);
                return 0;
            }
        }
        #endregion Coupon UPDATE


        #region Coupon GET
        public async Task<string> Coupon_Get(Int64 couponIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCoupon_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@CouponIDP", SqlDbType = SqlDbType.BigInt, Value = couponIDP });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);

                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCoupon_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Coupon GET


        #region Coupon GET_ALL
        public async Task<string> Coupon_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCoupon_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = param.SearchKeyWord });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageNumber", SqlDbType = SqlDbType.Int, Value = param.PageNo });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "pagination";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCoupon_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Coupon GET_ALL


        #region Coupon ACTIVE INACTIVE
        public async Task<Int64> Coupon_ActiveInactive(Int64 couponIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramCouponIDP = new SqlParameter("@CouponIDP", couponIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstCoupon_Update_ActiveInActive @CouponIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramCouponIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCoupon_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion Coupon ACTIVE INACTIVE


        #region Coupon DDL
        public async Task<string> Coupon_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCoupon_DDL";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    _context.Database.OpenConnection();

                    DbDataReader ddr = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCoupon_DDL", 1);
                return "";
            }
        }
        #endregion Coupon DDL


        #region Coupon DELETE
        public async Task<Int64> Coupon_Delete(Int64 couponIDP)
        {
            try
            {
                SqlParameter paramCouponIDP = new SqlParameter("@CouponIDP", couponIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstCoupon_Delete @CouponIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramCouponIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCoupon_Delete", 1);
                return 0;
            }
        }
        #endregion Coupon DELETE

    }
}
