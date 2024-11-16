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
    public class BannerService : CommonService, IBannerService
    {
        public BannerService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region Banner INSERT
        public async Task<Int64> Banner_Insert(Banner modelBanner)
        {
            try
            {
                SqlParameter paramBannerIDP = new SqlParameter
                {
                    ParameterName = "@BannerIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramBannerName = new SqlParameter("@BannerName", (object)modelBanner.BannerName ?? DBNull.Value);
                SqlParameter paramBannerTypeID = new SqlParameter("@BannerTypeID", (object)modelBanner.BannerTypeID ?? DBNull.Value);
                SqlParameter paramUserTypeID = new SqlParameter("@UserTypeID", (object)modelBanner.UserTypeID ?? DBNull.Value);
                SqlParameter paramAttachment = new SqlParameter("@Attachment", (object)modelBanner.Attachment ?? DBNull.Value);
                SqlParameter paramBannerURL = new SqlParameter("@BannerURL", (object)modelBanner.BannerURL ?? DBNull.Value);
                SqlParameter paramPositionID = new SqlParameter("@PositionID", (object)modelBanner.PositionID ?? DBNull.Value);
                SqlParameter paramCityIDFs = new SqlParameter("@CityIDFs", (object)modelBanner.CityIDFs ?? DBNull.Value);
                SqlParameter paramExpiryDate = new SqlParameter("@ExpiryDate", (object)modelBanner.ExpiryDate ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelBanner.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstBanner_Insert @BannerIDP OUTPUT, @BannerName, @BannerTypeID, @UserTypeID, @Attachment, @BannerURL, @PositionID, @CityIDFs, @ExpiryDate, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramBannerIDP, paramBannerName, paramBannerTypeID, paramUserTypeID, paramAttachment, paramBannerURL, paramPositionID, paramCityIDFs, paramExpiryDate, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramBannerIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBanner_Insert", 1);
                return 0;
            }
        }
        #endregion Banner INSERT


        #region Banner UPDATE
        public async Task<Int64> Banner_Update(Banner modelBanner)
        {
            try
            {
                SqlParameter paramBannerIDP = new SqlParameter("@BannerIDP", modelBanner.BannerIDP);
                SqlParameter paramBannerName = new SqlParameter("@BannerName", (object)modelBanner.BannerName ?? DBNull.Value);
                SqlParameter paramBannerTypeID = new SqlParameter("@BannerTypeID", (object)modelBanner.BannerTypeID ?? DBNull.Value);
                SqlParameter paramUserTypeID = new SqlParameter("@UserTypeID", (object)modelBanner.UserTypeID ?? DBNull.Value);
                SqlParameter paramAttachment = new SqlParameter("@Attachment", (object)modelBanner.Attachment ?? DBNull.Value);
                SqlParameter paramBannerURL = new SqlParameter("@BannerURL", (modelBanner.BannerURL == null) ? "" : modelBanner.BannerURL);
                SqlParameter paramPositionID = new SqlParameter("@PositionID", (object)modelBanner.PositionID ?? DBNull.Value);
                SqlParameter paramCityIDFs = new SqlParameter("@CityIDFs", (object)modelBanner.CityIDFs ?? DBNull.Value);
                SqlParameter paramExpiryDate = new SqlParameter("@ExpiryDate", (object)modelBanner.ExpiryDate ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelBanner.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstBanner_Update @BannerIDP, @BannerName, @BannerTypeID, @UserTypeID, @Attachment, @BannerURL, @PositionID, @CityIDFs, @ExpiryDate, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramBannerIDP, paramBannerName, paramBannerTypeID, paramUserTypeID, paramAttachment, paramBannerURL, paramPositionID, paramCityIDFs, paramExpiryDate, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramBannerIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBanner_Update", 1);
                return 0;
            }
        }
        #endregion Banner UPDATE


        #region Banner GET
        public async Task<string> Banner_Get(Int64 bannerIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstBanner_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@BannerIDP", SqlDbType = SqlDbType.BigInt, Value = bannerIDP });

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
                await ErrorLog(1, e.Message, $"uspmstBanner_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Banner GET


        #region Banner GET_ALL
        public async Task<string> Banner_GetAll(ModelCommonGetAll param)
        {
            try
            {
                if (ObjUser == null || ObjUser.UserType == null)
                {
                    throw new Exception("ObjUser or ObjUser.UserType is null.");
                }

                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstBanner_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = param.SearchKeyWord });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageNumber", SqlDbType = SqlDbType.Int, Value = param.PageNo });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@EntryTypeID", SqlDbType = SqlDbType.TinyInt, Value = ObjUser.UserType });

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
                await ErrorLog(1, e.Message, $"uspmstBanner_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Banner GET_ALL


        #region Banner ACTIVE INACTIVE
        public async Task<Int64> Banner_ActiveInactive(Int64 bannerIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramBannerIDP = new SqlParameter("@BannerIDP", bannerIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstBanner_Update_ActiveInActive @BannerIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramBannerIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBanner_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion Banner ACTIVE INACTIVE


        #region Banner APPROVE REJECT
        public async Task<Int64> Banner_ApproveReject(Int64 bannerIDP, Int32 isApprove)
        {
            try
            {
                SqlParameter paramBannerIDP = new SqlParameter("@BannerIDP", bannerIDP);
                SqlParameter paramIsApprove = new SqlParameter("@IsApprove", isApprove);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstBanner_Update_ApproveReject @BannerIDP, @IsApprove, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramBannerIDP, paramIsApprove, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBanner_Update_ApproveReject", 1);
                return 0;
            }
        }
        #endregion Banner APPROVE REJECT


        #region Banner DDL
        //public async Task<string> Banner_DDL()
        //{
        //    try
        //    {
        //        string strResponse = "";
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "uspmstBanner_DDL";
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            _context.Database.OpenConnection();

        //            DbDataReader ddr = command.ExecuteReader();
        //            DataTable dt = new DataTable();
        //            dt.Load(ddr);
        //            strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        //        }
        //        return strResponse;

        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"uspmstBanner_DDL", 1);
        //        return "";
        //    }
        //}
        #endregion Banner DDL


        #region Banner DELETE
        public async Task<Int64> Banner_Delete(Int64 bannerIDP)
        {
            try
            {
                SqlParameter paramBannerIDP = new SqlParameter("@BannerIDP", bannerIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstBanner_Delete @BannerIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramBannerIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBanner_Delete", 1);
                return 0;
            }
        }
        #endregion Banner DELETE

    }
}
