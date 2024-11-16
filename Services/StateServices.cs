using BMS_API.Services.Interface;
using BudgetManagement.Controllers;
using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BudgetManagement.Services
{
    public class StateServices : CommonService, IStateServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public StateServices(BMSContext context, IAuthService authService) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }

        #region INSERT
        
        public async Task<Int64> State_Insert(Tblstate tblstate)
        {
            try
            {
                SqlParameter paramStateIDP = new SqlParameter
                {
                    ParameterName = "StateIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                    Value = tblstate.StateIdp
                };
                SqlParameter paramStateName = new SqlParameter("@StateName", tblstate.StateName);
                SqlParameter paramCountryIDF = new SqlParameter("@CountryIDF", tblstate.CountryIdf);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", tblstate.IsActive);

                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };
                string paramSqlQuery = "EXECUTE dbo.uspmstState_Insert @StateIDP OUTPUT,@CountryIDF, @StateName, @IsActive, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramStateIDP, paramCountryIDF ,paramStateName, paramIsActive, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt32(paramStateIDP.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstState_Insert", 1);
                return 0;
            }
        }
        #endregion INSERT

        #region UPDATE
        
        public async Task<Int64> State_Update(Tblstate tblstate)
        {
            try
            {
                SqlParameter paramStateIdp = new SqlParameter("@StateIDP", tblstate.StateIdp);
                SqlParameter paramCountryIDF = new SqlParameter("@CountryIDF", tblstate.CountryIdf);
                SqlParameter paramStateName = new SqlParameter("@StateName", tblstate.StateName);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", tblstate.IsActive);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstState_Update @StateIDP, @CountryIDF, @StateName, @IsActive ,@IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramStateIdp, paramCountryIDF, paramStateName, paramIsActive ,paramIsDuplicate);

                //return (Convert.ToInt32(paramIsDuplicate.Value) == -1) ? -1 : tblstate.StateIdp;
                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : tblstate.StateIdp;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstState_Update", 1);
                return 0;
            }
        }
        #endregion UPDATE

        #region GET
        public async Task<string> State_Get(Int64 StateIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstState_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@StateIDP", SqlDbType = SqlDbType.BigInt, Value = StateIDP });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = await command.ExecuteReaderAsync();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                return strResponse;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstState_Get", 1);
                return null;
            }
        }
        #endregion GET

        #region GET-ALL
        
        public async Task<string> State_GetAll(ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstState_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = modelCommonGetAll.SearchKeyWord});
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageCrr", SqlDbType = SqlDbType.BigInt, Value = modelCommonGetAll.PageNo});
                    
                    _context.Database.OpenConnection();
                    DbDataReader ddr = await command.ExecuteReaderAsync();

                    //DataTable dt = new DataTable();
                    //dt.Load(ddr);
                    //strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

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
                await ErrorLog(1, e.Message, $"uspmstState_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        public async Task<Int64> State_ActiveInactive(Int64 stateIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramStateIDP = new SqlParameter("@StateIDP", stateIDP);
                //SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);

                var paramSqlQuery = "EXECUTE dbo.uspmstState_Update_ActiveInActive @StateIDP, @IsActive";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramStateIDP, paramIsActive);

                return 1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstState_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE
        public async Task<Int64> State_Delete(Int64 StateIDP)
        {
            try
            {
                SqlParameter paramStateIDP = new SqlParameter("@StateIDP", StateIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstState_Delete @StateIDP, @EntryBy, @IsDeleted OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramStateIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstState_delete", 1);
                return 0;
            }
        }
        #endregion DELETE

        #region DDL
        
        public async Task<string> State_DDL(Int64 CountryIDF)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstState_DDL";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@CountryIDF", SqlDbType = SqlDbType.BigInt, Value = CountryIDF });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = await command.ExecuteReaderAsync();
                    DataTable dt = new DataTable();
                    dt.Load(ddr);
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
                return strResponse;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstState_DDL", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion DDL
    }
}
