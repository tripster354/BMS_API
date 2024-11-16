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
    public class PollService : CommonService, IPollService
    {
        public PollService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region Poll INSERT
        public async Task<Int64> Poll_Insert(Poll modelPoll)
        {
            try
            {
                SqlParameter paramPollIDP = new SqlParameter("@PollIDP", (object)modelPoll.PollIDP ?? DBNull.Value);
                SqlParameter paramQuestion = new SqlParameter("@Question", (object)modelPoll.Question ?? DBNull.Value);
                SqlParameter paramOpt1 = new SqlParameter("@Opt1", (object)modelPoll.Opt1 ?? DBNull.Value);
                SqlParameter paramOpt2 = new SqlParameter("@Opt2", (object)modelPoll.Opt2 ?? DBNull.Value);
                SqlParameter paramOpt3 = new SqlParameter("@Opt3", (object)modelPoll.Opt3 ?? DBNull.Value);
                SqlParameter paramOpt4 = new SqlParameter("@Opt4", (object)modelPoll.Opt4 ?? DBNull.Value);
                SqlParameter paramOpt1Count = new SqlParameter("@Opt1Count", (object)modelPoll.Opt1Count ?? DBNull.Value);
                SqlParameter paramOpt2Count = new SqlParameter("@Opt2Count", (object)modelPoll.Opt2Count ?? DBNull.Value);
                SqlParameter paramOpt3Count = new SqlParameter("@Opt3Count", (object)modelPoll.Opt3Count ?? DBNull.Value);
                SqlParameter paramOpt4Count = new SqlParameter("@Opt4Count", (object)modelPoll.Opt4Count ?? DBNull.Value);
                SqlParameter paramExpiryDate = new SqlParameter("@ExpiryDate", (object)modelPoll.ExpiryDate ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelPoll.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.TinyInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstPoll_Insert @PollIDP, @Question, @Opt1, @Opt2, @Opt3, @Opt4, @Opt1Count, @Opt2Count, @Opt3Count, @Opt4Count, @ExpiryDate, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramPollIDP, paramQuestion, paramOpt1, paramOpt2, paramOpt3, paramOpt4, paramOpt1Count, paramOpt2Count, paramOpt3Count, paramOpt4Count, paramExpiryDate, paramIsActive, paramEntryBy, paramIsDuplicate);

                return Convert.ToInt64(paramIsDuplicate.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstPoll_Insert", 1);
                return 0;
            }
        }
        #endregion Poll INSERT


        #region Poll UPDATE
        public async Task<Int64> Poll_Update(Poll modelPoll)
        {
            try
            {
                SqlParameter paramPollIDP = new SqlParameter("@PollIDP", modelPoll.PollIDP);
                SqlParameter paramQuestion = new SqlParameter("@Question", (object)modelPoll.Question ?? DBNull.Value);
                SqlParameter paramOpt1 = new SqlParameter("@Opt1", (object)modelPoll.Opt1 ?? DBNull.Value);
                SqlParameter paramOpt2 = new SqlParameter("@Opt2", (object)modelPoll.Opt2 ?? DBNull.Value);
                SqlParameter paramOpt3 = new SqlParameter("@Opt3", (object)modelPoll.Opt3 ?? DBNull.Value);
                SqlParameter paramOpt4 = new SqlParameter("@Opt4", (object)modelPoll.Opt4 ?? DBNull.Value);
                SqlParameter paramOpt1Count = new SqlParameter("@Opt1Count", (object)modelPoll.Opt1Count ?? DBNull.Value);
                SqlParameter paramOpt2Count = new SqlParameter("@Opt2Count", (object)modelPoll.Opt2Count ?? DBNull.Value);
                SqlParameter paramOpt3Count = new SqlParameter("@Opt3Count", (object)modelPoll.Opt3Count ?? DBNull.Value);
                SqlParameter paramOpt4Count = new SqlParameter("@Opt4Count", (object)modelPoll.Opt4Count ?? DBNull.Value);
                SqlParameter paramExpiryDate = new SqlParameter("@ExpiryDate", (object)modelPoll.ExpiryDate ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelPoll.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.TinyInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstPoll_Update @PollIDP, @Question, @Opt1, @Opt2, @Opt3, @Opt4, @Opt1Count, @Opt2Count, @Opt3Count, @Opt4Count, @ExpiryDate, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramPollIDP, paramQuestion, paramOpt1, paramOpt2, paramOpt3, paramOpt4, paramOpt1Count, paramOpt2Count, paramOpt3Count, paramOpt4Count, paramExpiryDate, paramIsActive, paramEntryBy, paramIsDuplicate);

                return Convert.ToInt64(paramIsDuplicate.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstPoll_Update", 1);
                return 0;
            }
        }
        #endregion Poll UPDATE


        #region Poll GET
        public async Task<string> Poll_Get(Int64 pollIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstPoll_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PollIDP", SqlDbType = SqlDbType.BigInt, Value = pollIDP });

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
                await ErrorLog(1, e.Message, $"uspmstPoll_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Poll GET


        #region Poll GET_ALL
        public async Task<string> Poll_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstPoll_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstPoll_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Poll GET_ALL


        #region Poll ACTIVE INACTIVE
        public async Task<Int64> Poll_ActiveInactive(Int64 pollIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramPollIDP = new SqlParameter("@PollIDP", pollIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstPoll_Update_ActiveInActive @PollIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPollIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstPoll_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion Poll ACTIVE INACTIVE


        #region Poll DDL
        //public async Task<string> Poll_DDL()
        //{
        //    try
        //    {
        //        string strResponse = "";
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "uspmstPoll_DDL";
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
        //        await ErrorLog(1, e.Message, $"uspmstPoll_DDL", 1);
        //        return "";
        //    }
        //}
        #endregion Poll DDL


        #region Poll DELETE
        public async Task<Int64> Poll_Delete(Int64 pollIDP)
        {
            try
            {
                SqlParameter paramPollIDP = new SqlParameter("@PollIDP", pollIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstPoll_Delete @PollIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPollIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstPoll_Delete", 1);
                return 0;
            }
        }
        #endregion Poll DELETE

    }
}
