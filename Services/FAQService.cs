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
    public class FAQService : CommonService, IFAQService
    {
        public FAQService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region FAQ INSERT
        public async Task<Int64> FAQ_Insert(FAQ modelFAQ)
        {
            try
            {
                SqlParameter paramFAQIDP = new SqlParameter
                {
                    ParameterName = "@FAQIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramFAQQuestion = new SqlParameter("@FAQQuestion", (object)modelFAQ.FAQQuestion ?? DBNull.Value);
                SqlParameter paramFAQAnswer = new SqlParameter("@FAQAnswer", (object)modelFAQ.FAQAnswer ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelFAQ.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstFAQ_Insert @FAQIDP OUTPUT, @FAQQuestion, @FAQAnswer, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramFAQIDP, paramFAQQuestion, paramFAQAnswer, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramFAQIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstFAQ_Insert", 1);
                return 0;
            }
        }
        #endregion FAQ INSERT


        #region FAQ UPDATE
        public async Task<Int64> FAQ_Update(FAQ modelFAQ)
        {
            try
            {
                SqlParameter paramFAQIDP = new SqlParameter("@FAQIDP", modelFAQ.FAQIDP);
                SqlParameter paramFAQQuestion = new SqlParameter("@FAQQuestion", (object)modelFAQ.FAQQuestion ?? DBNull.Value);
                SqlParameter paramFAQAnswer = new SqlParameter("@FAQAnswer", (object)modelFAQ.FAQAnswer ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelFAQ.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstFAQ_Update @FAQIDP, @FAQQuestion, @FAQAnswer, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramFAQIDP, paramFAQQuestion, paramFAQAnswer, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramFAQIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstFAQ_Update", 1);
                return 0;
            }
        }
        #endregion FAQ UPDATE


        #region FAQ GET
        public async Task<string> FAQ_Get(Int64 fAQIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstFAQ_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@FAQIDP", SqlDbType = SqlDbType.BigInt, Value = fAQIDP });

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
                await ErrorLog(1, e.Message, $"uspmstFAQ_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion FAQ GET


        #region FAQ GET_ALL
        public async Task<string> FAQ_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstFAQ_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstFAQ_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion FAQ GET_ALL


        #region FAQ ACTIVE INACTIVE
        public async Task<Int64> FAQ_ActiveInactive(Int64 fAQIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramFAQIDP = new SqlParameter("@FAQIDP", fAQIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstFAQ_Update_ActiveInActive @FAQIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramFAQIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstFAQ_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion FAQ ACTIVE INACTIVE


        #region FAQ DDL
        public async Task<string> FAQ_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstFAQ_DDL";
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
                await ErrorLog(1, e.Message, $"uspmstFAQ_DDL", 1);
                return "";
            }
        }
        #endregion FAQ DDL


        #region FAQ DELETE
        public async Task<Int64> FAQ_Delete(Int64 fAQIDP)
        {
            try
            {
                SqlParameter paramFAQIDP = new SqlParameter("@FAQIDP", fAQIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstFAQ_Delete @FAQIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramFAQIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstFAQ_Delete", 1);
                return 0;
            }
        }
        #endregion FAQ DELETE

    }
}
