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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BMS_API.Models.Utility;

namespace BMS_API.Services
{
    public class SubAdminService : CommonService, ISubAdminService
    {
        public SubAdminService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region SubAdmin INSERT
        public async Task<Int64> SubAdmin_Insert(SubAdmin modelSubAdmin, string connection)
        {
            try
            {
                dynamic ivDtls = JsonConvert.DeserializeObject(modelSubAdmin.tblPermission);
                DataTable dtPermission = new DataTable();
                dtPermission.Columns.AddRange(new DataColumn[3]
                {
                    new DataColumn("ModuleIDP",typeof(Int32)),
                    new DataColumn("IsRead",typeof(Boolean)),
                    new DataColumn("IsWrite",typeof(Boolean)),
                });
                JToken m1 = ((Newtonsoft.Json.Linq.JContainer)ivDtls);

                foreach (dynamic ivDtl in m1)
                {
                    DataRow _row = dtPermission.NewRow();

                    _row["ModuleIDP"] = Convert.ToInt64(ivDtl["ModuleIDP"].ToString());
                    _row["IsRead"] = (ivDtl["IsRead"] == null) ? false : Convert.ToBoolean(ivDtl["IsRead"]);
                    _row["IsWrite"] = (ivDtl["IsWrite"] == null) ? false : Convert.ToBoolean(ivDtl["IsWrite"]);

                    dtPermission.Rows.Add(_row);
                }

                string commandText = "uspmstSubAdmin_Insert";

                using (SqlConnection conn = new SqlConnection(connection))
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter paramSubAdminIDP = new SqlParameter("@SubAdminIDP", SqlDbType.BigInt);
                    paramSubAdminIDP.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramSubAdminIDP);

                    AddInParameter(cmd, "FullName", SqlDbType.NVarChar, (object)modelSubAdmin.FullName ?? DBNull.Value);
                    AddInParameter(cmd, "EmailID", SqlDbType.NVarChar, (object)modelSubAdmin.EmailID ?? DBNull.Value);
                    AddInParameter(cmd, "MobileNo", SqlDbType.NVarChar, (object)modelSubAdmin.MobileNo ?? DBNull.Value);
                    AddInParameter(cmd, "Password", SqlDbType.NVarChar, (object)modelSubAdmin.Password ?? DBNull.Value);
                    AddInParameter(cmd, "RoleIDF", SqlDbType.BigInt, (object)modelSubAdmin.RoleIDF ?? DBNull.Value);
                    AddInParameter(cmd, "IsActive", SqlDbType.Bit, (object)modelSubAdmin.IsActive ?? DBNull.Value);
                    AddInParameter(cmd, "EntryBy", SqlDbType.Int, ObjUser.UserID);

                    SqlParameter paramIsDuplicate = new SqlParameter("@IsDuplicate", SqlDbType.Bit);
                    paramIsDuplicate.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramIsDuplicate);

                    SqlParameter sqlParamData = cmd.Parameters.AddWithValue("@Permission", dtPermission);
                    sqlParamData.SqlDbType = SqlDbType.Structured;

                    paramSubAdminIDP.Direction = ParameterDirection.Output;
                    paramSubAdminIDP.Value = false;

                    paramIsDuplicate.Direction = ParameterDirection.Output;
                    paramIsDuplicate.Value = false;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramSubAdminIDP.Value);
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        #endregion SubAdmin INSERT


        #region SubAdmin UPDATE
        public async Task<Int64> SubAdmin_Update(SubAdmin modelSubAdmin, string connection)
        {
            try
            {
                dynamic ivDtls = JsonConvert.DeserializeObject(modelSubAdmin.tblPermission);
                DataTable dtPermission = new DataTable();
                dtPermission.Columns.AddRange(new DataColumn[3]
                {
                    new DataColumn("ModuleIDP",typeof(Int32)),
                    new DataColumn("IsRead",typeof(Boolean)),
                    new DataColumn("IsWrite",typeof(Boolean)),
                });
                JToken m1 = ((Newtonsoft.Json.Linq.JContainer)ivDtls);

                foreach (dynamic ivDtl in m1)
                {
                    DataRow _row = dtPermission.NewRow();

                    _row["ModuleIDP"] = Convert.ToInt64(ivDtl["ModuleIDP"].ToString());
                    _row["IsRead"] = (ivDtl["IsRead"] == null) ? false : Convert.ToBoolean(ivDtl["IsRead"]);
                    _row["IsWrite"] = (ivDtl["IsWrite"] == null) ? false : Convert.ToBoolean(ivDtl["IsWrite"]);

                    dtPermission.Rows.Add(_row);
                }

                string commandText = "uspmstSubAdmin_Update";

                using (SqlConnection conn = new SqlConnection(connection))
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    AddInParameter(cmd, "SubAdminIDP", SqlDbType.BigInt, (object)modelSubAdmin.SubAdminIDP ?? DBNull.Value);
                    AddInParameter(cmd, "FullName", SqlDbType.NVarChar, (object)modelSubAdmin.FullName ?? DBNull.Value);
                    AddInParameter(cmd, "EmailID", SqlDbType.NVarChar, (object)modelSubAdmin.EmailID ?? DBNull.Value);
                    AddInParameter(cmd, "MobileNo", SqlDbType.NVarChar, (object)modelSubAdmin.MobileNo ?? DBNull.Value);
                    AddInParameter(cmd, "Password", SqlDbType.NVarChar, (object)modelSubAdmin.Password ?? DBNull.Value);
                    AddInParameter(cmd, "RoleIDF", SqlDbType.BigInt, (object)modelSubAdmin.RoleIDF ?? DBNull.Value);
                    AddInParameter(cmd, "IsActive", SqlDbType.Bit, (object)modelSubAdmin.IsActive ?? DBNull.Value);
                    AddInParameter(cmd, "EntryBy", SqlDbType.Int, ObjUser.UserID);

                    SqlParameter paramIsDuplicate = new SqlParameter("@IsDuplicate", SqlDbType.Bit);
                    paramIsDuplicate.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramIsDuplicate);

                    SqlParameter sqlParamData = cmd.Parameters.AddWithValue("@Permission", dtPermission);
                    sqlParamData.SqlDbType = SqlDbType.Structured;

                    paramIsDuplicate.Direction = ParameterDirection.Output;
                    paramIsDuplicate.Value = false;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(modelSubAdmin.SubAdminIDP);
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        #endregion SubAdmin UPDATE


        #region SubAdmin GET
        public async Task<string> SubAdmin_Get(Int64 subAdminIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstSubAdmin_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SubAdminIDP", SqlDbType = SqlDbType.BigInt, Value = subAdminIDP });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "SubAdminData";
                    ds.Tables[1].TableName = "PermissionModuleDetails";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstSubAdmin_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion SubAdmin GET


        #region SubAdmin GET_ALL
        public async Task<string> SubAdmin_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstSubAdmin_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstSubAdmin_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion SubAdmin GET_ALL


        #region SubAdmin ACTIVE INACTIVE
        public async Task<Int64> SubAdmin_ActiveInactive(Int64 subAdminIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramSubAdminIDP = new SqlParameter("@SubAdminIDP", subAdminIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspmstSubAdmin_Update_ActiveInActive @SubAdminIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramSubAdminIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstSubAdmin_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion SubAdmin ACTIVE INACTIVE


        #region SubAdmin DDL
        //public async Task<string> SubAdmin_DDL()
        //{
        //    try
        //    {
        //        string strResponse = "";
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "uspmstSubAdmin_DDL";
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
        //        await ErrorLog(1, e.Message, $"uspmstSubAdmin_DDL", 1);
        //        return "";
        //    }
        //}
        #endregion SubAdmin DDL


        #region SubAdmin DELETE
        public async Task<Int64> SubAdmin_Delete(Int64 subAdminIDP)
        {
            try
            {
                SqlParameter paramSubAdminIDP = new SqlParameter("@SubAdminIDP", subAdminIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstSubAdmin_Delete @SubAdminIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramSubAdminIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstSubAdmin_Delete", 1);
                return 0;
            }
        }
        #endregion SubAdmin DELETE


        #region SubAdmin GET Permission By ModuleIDF
        public async Task<string> SubAdmin_Get_Permission_By_ModuleIDF(Model_Get_Permission paramGetPermission)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspSubAdmin_Get_Permission_By_ModuleIDF";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SubAdminIDP", SqlDbType = SqlDbType.BigInt, Value = paramGetPermission.SubAdminIDP });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ModuleIDF", SqlDbType = SqlDbType.BigInt, Value = paramGetPermission.ModuleIDF });

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
                await ErrorLog(1, e.Message, $"uspSubAdmin_Get_Permission_By_ModuleIDF", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion SubAdmin GET Permission By ModuleIDF


        private SqlParameter AddInParameter(SqlCommand cmdSql, string strName, SqlDbType sqlType, object value)
        {
            SqlParameter prmReturn = null;
            //try
            //{
            prmReturn = new SqlParameter(strName, sqlType);
            prmReturn.Direction = ParameterDirection.Input;
            prmReturn.Value = value;
            cmdSql.Parameters.Add(prmReturn);
            return prmReturn;
        }

    }
}
