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
    public class CityServices : CommonService, ICityServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public CityServices(BMSContext context) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }

        #region INSERT
        public async Task<Int64> City_Insert(TblCity tblCity)
        {
            try
            {
                SqlParameter paramCityIDP = new SqlParameter
                {
                    ParameterName = "CityIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                    Value = tblCity.CityIdp
                };
                SqlParameter paramCityName = new SqlParameter("@CityName", tblCity.CityName);
                SqlParameter paramCountryIDF = new SqlParameter("@CountryIDF", (object)tblCity.CountryIdf ?? DBNull.Value);
                SqlParameter paramStateIDF = new SqlParameter("@StateIDF", (object)tblCity.StateIdf ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", tblCity.IsActive);

                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };
                string paramSqlQuery = "EXECUTE dbo.uspmstCity_Insert @CityIDP OUTPUT, @CountryIDF,@StateIDF, @CityName, @IsActive, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCityIDP, paramCountryIDF, paramStateIDF , paramCityName, paramIsActive ,paramIsDuplicate);

                //return (Convert.ToInt32(paramIsDuplicate.Value) == -1) ? -1 : Convert.ToInt32(paramCityIDP.Value);
                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt32(paramCityIDP.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCity_Insert", 1);
                return 0;
            }
        }
        #endregion INSERT

        #region UPDATE
        public async Task<Int64> City_Update(TblCity tblCity)
        {
            try
            {
                SqlParameter paramCityIdp = new SqlParameter("@CityIDP", tblCity.CityIdp);
                SqlParameter paramCountryIDF = new SqlParameter("@CountryIDF", (object)tblCity.CountryIdf ?? DBNull.Value);
                SqlParameter paramStateIDF = new SqlParameter("@StateIDF", (object)tblCity.StateIdf ?? DBNull.Value);
                SqlParameter paramCityName = new SqlParameter("@CityName", tblCity.CityName);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", tblCity.IsActive);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstCity_Update @CityIDP, @CountryIDF, @StateIDF, @CityName, @IsActive, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCityIdp, paramCountryIDF, paramStateIDF ,paramCityName, paramIsActive ,paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : tblCity.CityIdp;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCity_Update", 1);
                return 0;
            }
        }
        #endregion UPDATE

        #region GET
        public async Task<string> City_Get(Int64 cityIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCity_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@CityIdp", SqlDbType = SqlDbType.BigInt, Value = cityIDP });

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
                await ErrorLog(1, e.Message, $"uspmstCity_Get", 1);
                return null;
            }
        }
        #endregion GET

        #region GET-ALL
        public async Task<string> City_GetAll(ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCity_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = modelCommonGetAll.SearchKeyWord });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageCrr", SqlDbType = SqlDbType.Int, Value = modelCommonGetAll.PageNo });

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
                await ErrorLog(1, e.Message, $"uspmstCity_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        public async Task<Int64> City_ActiveInactive(Int64 cityIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramCityIDP = new SqlParameter("@CityIDP", cityIDP);
                //SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);

                var paramSqlQuery = "EXECUTE dbo.uspmstCity_Update_ActiveInActive @CityIDP, @IsActive";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramCityIDP, paramIsActive);

                return 1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCity_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE
        public async Task<Int64> City_Delete(Int64 CityIDP)
        {
            try
            {
                SqlParameter paramCityIDP = new SqlParameter("@CityIDP", CityIDP);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstCity_Delete @CityIDP, @IsDeleted OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCityIDP, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCity_Delete", 1);
                return 0;
            }
        }
        #endregion DELETE

        #region DDL
        public async Task<string> City_DDL(Int64 stateIDF)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCity_DDL";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@StateIDF", SqlDbType = SqlDbType.Int, Value = stateIDF });

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
                await ErrorLog(1, e.Message, $"uspmstCity_DDL", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion DDL

        #region DDL_FindAny
        public async Task<string> City_DDL_FindAny(string searchKeyword)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCity_DDL_FindAny";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = searchKeyword });

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
                await ErrorLog(1, e.Message, $"uspmstCity_DDL_findAny", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion DDL


    }
}
