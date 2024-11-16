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
    public class RoleService : CommonService, IRoleService
    {
        public RoleService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region Role INSERT
        public async Task<Int64> Role_Insert(Role modelRole)
        {
            try
            {
                SqlParameter paramRoleIDP = new SqlParameter
                {
                    ParameterName = "@RoleIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramRoleName = new SqlParameter("@RoleName", (object)modelRole.RoleName ?? DBNull.Value);
                SqlParameter paramDescription = new SqlParameter("@Description", (object)modelRole.Description ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelRole.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstRole_Insert @RoleIDP OUTPUT, @RoleName, @Description, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramRoleIDP, paramRoleName, paramDescription, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramRoleIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstRole_Insert", 1);
                return 0;
            }
        }
        #endregion Role INSERT


        #region Role UPDATE
        public async Task<Int64> Role_Update(Role modelRole)
        {
            try
            {
                SqlParameter paramRoleIDP = new SqlParameter("@RoleIDP", modelRole.RoleIDP);
                SqlParameter paramRoleName = new SqlParameter("@RoleName", (object)modelRole.RoleName ?? DBNull.Value);
                SqlParameter paramDescription = new SqlParameter("@Description", (object)modelRole.Description ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)modelRole.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstRole_Update @RoleIDP, @RoleName, @Description, @IsActive, @EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramRoleIDP, paramRoleName, paramDescription, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramRoleIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstRole_Update", 1);
                return 0;
            }
        }
        #endregion Role UPDATE


        #region Role GET
        public async Task<string> Role_Get(Int64 roleIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstRole_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@RoleIDP", SqlDbType = SqlDbType.BigInt, Value = roleIDP });

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
                await ErrorLog(1, e.Message, $"uspmstRole_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Role GET


        #region Role GET_ALL
        public async Task<string> Role_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstRole_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstRole_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Role GET_ALL


        #region Role ACTIVE INACTIVE
        public async Task<Int64> Role_ActiveInactive(Int64 roleIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramRoleIDP = new SqlParameter("@RoleIDP", roleIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstRole_Update_ActiveInActive @RoleIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramRoleIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstRole_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion Role ACTIVE INACTIVE


        #region Role DDL
        public async Task<string> Role_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstRole_DDL";
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
                await ErrorLog(1, e.Message, $"uspmstRole_DDL", 1);
                return "";
            }
        }
        #endregion Role DDL


        #region Role DELETE
        public async Task<Int64> Role_Delete(Int64 roleIDP)
        {
            try
            {
                SqlParameter paramRoleIDP = new SqlParameter("@RoleIDP", roleIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstRole_Delete @RoleIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramRoleIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstRole_Delete", 1);
                return 0;
            }
        }
        #endregion Role DELETE

    }
}
