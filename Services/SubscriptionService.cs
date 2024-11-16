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
    public class SubscriptionService : CommonService, ISubscriptionService
    {
        public SubscriptionService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region Subscription INSERT
        public async Task<Int64> Subscription_Insert(Subscription modelSubscription)
        {
            try
            {
                SqlParameter paramSubscriptionIDP = new SqlParameter
                {
                    ParameterName = "@SubscriptionIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramSubscriptionPackage = new SqlParameter("@SubscriptionPackage", modelSubscription.SubscriptionPackage);
                SqlParameter paramSubscriptionType = new SqlParameter("@SubscriptionType", modelSubscription.SubscriptionType);
                SqlParameter paramSubscriptionFeesMonthly = new SqlParameter("@SubscriptionFeesMonthly", modelSubscription.SubscriptionFeesMonthly);
                SqlParameter paramSubscriptionFeesYearly = new SqlParameter("@SubscriptionFeesYearly", modelSubscription.SubscriptionFeesYearly);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", modelSubscription.IsActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstSubscription_Insert @SubscriptionIDP OUTPUT, @SubscriptionPackage, @SubscriptionType, @SubscriptionFeesMonthly, @SubscriptionFeesYearly, @IsActive, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramSubscriptionIDP, paramSubscriptionPackage, paramSubscriptionType, paramSubscriptionFeesMonthly, paramSubscriptionFeesYearly, paramIsActive, paramEntryBy);

                return Convert.ToInt64(paramSubscriptionIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstSubscription_Insert", 1);
                return 0;
            }
        }
        #endregion Subscription INSERT

        #region Subscription UPDATE
        public async Task<Int64> Subscription_Update(Subscription modelSubscription)
        {
            try
            {
                SqlParameter paramSubscriptionIDP = new SqlParameter("@SubscriptionIDP", modelSubscription.SubscriptionIDP);
                SqlParameter paramSubscriptionPackage = new SqlParameter("@SubscriptionPackage", modelSubscription.SubscriptionPackage);
                SqlParameter paramSubscriptionType = new SqlParameter("@SubscriptionType", modelSubscription.SubscriptionType);
                SqlParameter paramSubscriptionFeesMonthly = new SqlParameter("@SubscriptionFeesMonthly", modelSubscription.SubscriptionFeesMonthly);
                SqlParameter paramSubscriptionFeesYearly = new SqlParameter("@SubscriptionFeesYearly", modelSubscription.SubscriptionFeesYearly);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", modelSubscription.IsActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstSubscription_Update @SubscriptionIDP, @SubscriptionPackage, @SubscriptionType, @SubscriptionFeesMonthly, @SubscriptionFeesYearly, @IsActive, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramSubscriptionIDP, paramSubscriptionPackage, paramSubscriptionType, paramSubscriptionFeesMonthly, paramSubscriptionFeesYearly, paramIsActive, paramEntryBy);

                return Convert.ToInt64(paramSubscriptionIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstSubscription_Update", 1);
                return 0;
            }
        }
        #endregion Subscription UPDATE

        #region Subscription GET
        public async Task<string> Subscription_Get(Int64 subscriptionIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstSubscription_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SubscriptionIDP", SqlDbType = SqlDbType.BigInt, Value = subscriptionIDP });

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
                await ErrorLog(1, e.Message, $"uspmstSubscription_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Subscription GET

        #region Subscription GET_ALL
        public async Task<string> Subscription_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstSubscription_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstSubscription_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Subscription GET_ALL


        #region Subscription ACTIVE INACTIVE
        public async Task<Int64> Subscription_ActiveInactive(Int64 subscriptionIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramSubscriptionIDP = new SqlParameter("@SubscriptionIDP", subscriptionIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstSubscription_Update_ActiveInActive @SubscriptionIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramSubscriptionIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstSubscription_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion Subscription ACTIVE INACTIVE

        #region Subscription DELETE
        public async Task<Int64> Subscription_Delete(Int64 subscriptionIDP)
        {
            try
            {
                SqlParameter paramSubscriptionIDP = new SqlParameter("@SubscriptionIDP", subscriptionIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstSubscription_Delete @SubscriptionIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramSubscriptionIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstSubscription_Delete", 1);
                return 0;
            }
        }
        #endregion Subscription DELETE

    }
}
