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
    public class InterestService : CommonService, IInterestService
    {
        public InterestService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }


        #region Interest INSERT
        public async Task<Int64> Interest_Insert(Interest modelInterest)
        {
            try
            {
                SqlParameter paramInterestIDP = new SqlParameter
                {
                    ParameterName = "@InterestIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramInterestName = new SqlParameter("@InterestName", (object)modelInterest.InterestName ?? DBNull.Value);
                SqlParameter paramInterestAttachment = new SqlParameter("@InterestAttachment", (object)modelInterest.InterestAttachment ?? DBNull.Value);
                SqlParameter paramDescription = new SqlParameter("@Description", (object)modelInterest.Description ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelInterest.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstInterest_Insert @InterestIDP OUTPUT, @InterestName, @InterestAttachment, @Description, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramInterestIDP, paramInterestName, paramInterestAttachment, paramDescription, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramInterestIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstInterest_Insert", 1);
                return 0;
            }
        }
        #endregion Interest INSERT


        #region Interest UPDATE
        public async Task<Int64> Interest_Update(Interest modelInterest)
        {
            try
            {
                SqlParameter paramInterestIDP = new SqlParameter("@InterestIDP", (object)modelInterest.InterestIDP ?? DBNull.Value);
                SqlParameter paramInterestName = new SqlParameter("@InterestName", (object)modelInterest.InterestName ?? DBNull.Value);
                SqlParameter paramInterestAttachment = new SqlParameter("@InterestAttachment", (object)modelInterest.InterestAttachment ?? DBNull.Value);
                SqlParameter paramDescription = new SqlParameter("@Description", (object)modelInterest.Description ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelInterest.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstInterest_Update @InterestIDP, @InterestName, @InterestAttachment, @Description, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramInterestIDP, paramInterestName, paramInterestAttachment, paramDescription, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramInterestIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstInterest_Update", 1);
                return 0;
            }
        }
        #endregion Interest UPDATE


        #region Interest GET
        public async Task<string> Interest_Get(Int64 interestIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstInterest_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@InterestIDP", SqlDbType = SqlDbType.BigInt, Value = interestIDP });

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
                await ErrorLog(1, e.Message, $"uspmstInterest_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Interest GET


        #region Interest GET_ALL
        public async Task<string> Interest_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstInterest_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstInterest_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Interest GET_ALL


        #region Interest ACTIVE INACTIVE
        public async Task<Int64> Interest_ActiveInactive(Int64 interestIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramInterestIDP = new SqlParameter("@InterestIDP", interestIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstInterest_Update_ActiveInActive @InterestIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramInterestIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstInterest_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion Interest ACTIVE INACTIVE


        #region Interest DDL
        public async Task<string> Interest_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstInterest_DDL";
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
                await ErrorLog(1, e.Message, $"uspmstInterest_DDL", 1);
                return "";
            }
        }
        #endregion Interest DDL


        #region Interest DELETE
        public async Task<Int64> Interest_Delete(Int64 interestIDP)
        {
            try
            {
                SqlParameter paramInterestIDP = new SqlParameter("@InterestIDP", interestIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstInterest_Delete @InterestIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramInterestIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstInterest_Delete", 1);
                return 0;
            }
        }
        #endregion Interest DELETE

    }
}
