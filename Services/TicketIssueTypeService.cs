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
    public class TicketIssueTypeService : CommonService, ITicketIssueTypeService
    {
        public TicketIssueTypeService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }


        #region TicketIssueType INSERT
        public async Task<Int64> TicketIssueType_Insert(TicketIssueType modelTicketIssueType)
        {
            try
            {
                SqlParameter paramTicketIssueTypeIDP = new SqlParameter
                {
                    ParameterName = "@TicketIssueTypeIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramTicketIssueTypeName = new SqlParameter("@TicketIssueTypeName", (object)modelTicketIssueType.TicketIssueTypeName ?? DBNull.Value);
                SqlParameter paramDescription = new SqlParameter("@Description", (object)modelTicketIssueType.Description ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelTicketIssueType.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstTicketIssueType_Insert @TicketIssueTypeIDP OUTPUT, @TicketIssueTypeName, @Description, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramTicketIssueTypeIDP, paramTicketIssueTypeName, paramDescription, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramTicketIssueTypeIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstTicketIssueType_Insert", 1);
                return 0;
            }
        }
        #endregion TicketIssueType INSERT


        #region TicketIssueType UPDATE
        public async Task<Int64> TicketIssueType_Update(TicketIssueType modelTicketIssueType)
        {
            try
            {
                SqlParameter paramTicketIssueTypeIDP = new SqlParameter("@TicketIssueTypeIDP", modelTicketIssueType.TicketIssueTypeIDP);
                SqlParameter paramTicketIssueTypeName = new SqlParameter("@TicketIssueTypeName", (object)modelTicketIssueType.TicketIssueTypeName ?? DBNull.Value);
                SqlParameter paramDescription = new SqlParameter("@Description", (object)modelTicketIssueType.Description ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelTicketIssueType.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstTicketIssueType_Update @TicketIssueTypeIDP, @TicketIssueTypeName, @Description, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramTicketIssueTypeIDP, paramTicketIssueTypeName, paramDescription, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramTicketIssueTypeIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstTicketIssueType_Update", 1);
                return 0;
            }
        }
        #endregion TicketIssueType UPDATE


        #region TicketIssueType GET
        public async Task<string> TicketIssueType_Get(Int64 ticketIssueTypeIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstTicketIssueType_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@TicketIssueTypeIDP", SqlDbType = SqlDbType.BigInt, Value = ticketIssueTypeIDP });

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
                await ErrorLog(1, e.Message, $"uspmstTicketIssueType_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion TicketIssueType GET


        #region TicketIssueType GET_ALL
        public async Task<string> TicketIssueType_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstTicketIssueType_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstTicketIssueType_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion TicketIssueType GET_ALL


        #region TicketIssueType ACTIVE INACTIVE
        public async Task<Int64> TicketIssueType_ActiveInactive(Int64 ticketIssueTypeIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramTicketIssueTypeIDP = new SqlParameter("@TicketIssueTypeIDP", ticketIssueTypeIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstTicketIssueType_Update_ActiveInActive @TicketIssueTypeIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramTicketIssueTypeIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstTicketIssueType_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion TicketIssueType ACTIVE INACTIVE


        #region TicketIssueType DDL
        public async Task<string> TicketIssueType_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstTicketIssueType_DDL";
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
                await ErrorLog(1, e.Message, $"uspmstTicketIssueType_DDL", 1);
                return "";
            }
        }
        #endregion TicketIssueType DDL


        #region TicketIssueType DELETE
        public async Task<Int64> TicketIssueType_Delete(Int64 ticketIssueTypeIDP)
        {
            try
            {
                SqlParameter paramTicketIssueTypeIDP = new SqlParameter("@TicketIssueTypeIDP", ticketIssueTypeIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstTicketIssueType_Delete @TicketIssueTypeIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramTicketIssueTypeIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstTicketIssueType_Delete", 1);
                return 0;
            }
        }
        #endregion TicketIssueType DELETE

    }
}
