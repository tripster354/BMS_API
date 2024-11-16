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
    public class CountryServices : CommonService, lCountryServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public CountryServices(BMSContext context) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }

        #region INSERT
        
        public async Task<Int64> Country_Insert(TblCountry tblCountry)
        {
            try
            {
                SqlParameter paramCountryIDP = new SqlParameter
                {
                    ParameterName = "CountryIdp",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                    Value = tblCountry.CountryIdp
                };
                SqlParameter paramCountryName = new SqlParameter("@CountryName", tblCountry.CountryName);
                SqlParameter paramCountryCode = new SqlParameter("@CountryCode", tblCountry.CountryCode);
                SqlParameter paramCountryFlag = new SqlParameter("@CountryFlag", (object)tblCountry.CountryFlag ?? DBNull.Value);
                SqlParameter paramCountryCurrency = new SqlParameter("@CountryCurrency", (object)tblCountry.CountryCurrency ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)tblCountry.IsActive ?? DBNull.Value);
                SqlParameter paramISDCode = new SqlParameter("@ISDCode", (object)tblCountry.Isdcode ?? DBNull.Value);

                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };
                string paramSqlQuery = "EXECUTE dbo.uspmstCountry_Insert @CountryIdp OUTPUT, @CountryName, @CountryCode, @CountryFlag, @CountryCurrency, @IsActive, @ISDCode, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCountryIDP, paramCountryName, paramCountryCode, paramCountryFlag, paramCountryCurrency, paramIsActive, paramISDCode, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt32(paramCountryIDP.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCountry_Insert", 1);
                return 0;
            }
        }
        #endregion INSERT

        #region UPDATE
        public async Task<Int64> Country_Update(TblCountry tblCountry)
        {
            try
            {
                SqlParameter paramCountryIdp = new SqlParameter("@CountryIdp", tblCountry.CountryIdp);
                SqlParameter paramCountryName = new SqlParameter("@CountryName", tblCountry.CountryName);
                SqlParameter paramCountryCode = new SqlParameter("@CountryCode", tblCountry.CountryCode);
                SqlParameter paramCountryFlag = new SqlParameter("@CountryFlag", (object)tblCountry.CountryFlag ?? DBNull.Value );
                SqlParameter paramCountryCurrency = new SqlParameter("@CountryCurrency", (object)tblCountry.CountryCurrency ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)tblCountry.IsActive ?? DBNull.Value);
                SqlParameter paramISDCode = new SqlParameter("@ISDCode", (object)tblCountry.Isdcode ?? DBNull.Value);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };
                
                var paramSqlQuery = "EXECUTE dbo.uspmstCountry_Update @CountryIdp, @CountryName, @CountryCode, @CountryFlag, @CountryCurrency, @ISDCode, @IsActive, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCountryIdp, paramCountryName, paramCountryCode, paramCountryFlag, paramCountryCurrency, paramISDCode, paramIsActive, paramIsDuplicate);

                //return (Convert.ToInt32(paramIsDuplicate.Value) == -1) ? -1 : tblCountry.CountryIdp;
                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : tblCountry.CountryIdp;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCountry_Update", 1);
                return 0;
            }
        }
        #endregion UPDATE

        #region GET
        
        public async Task<string> Country_Get(Int64 countryIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCountry_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@CountryIDP", SqlDbType = SqlDbType.BigInt, Value = countryIDP });

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
                await ErrorLog(1, e.Message, $"uspmstCountry_Get", 1);
                return null;
            }
        }
        #endregion GET

        #region GET-ALL
        
        public async Task<string> Country_GetAll(ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCountry_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = modelCommonGetAll.SearchKeyWord });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageCrr", SqlDbType = SqlDbType.BigInt, Value = modelCommonGetAll.PageNo });

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
                await ErrorLog(1, e.Message, $"uspmstCountry_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        public async Task<Int64> Country_ActiveInactive(Int64 countryIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramCountryIDP = new SqlParameter("@CountryIDP", countryIDP);
                //SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);

                var paramSqlQuery = "EXECUTE dbo.uspmstCountry_Update_ActiveInActive @CountryIDP, @IsActive";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramCountryIDP, paramIsActive);

                return 1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCountry_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE        
        public async Task<Int64> Country_Delete(Int64 countryIPD)
        {
            try
            {
                SqlParameter paramCountryIDP = new SqlParameter("@CountryIdp", countryIPD);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstCountry_Delete @CountryIdp, @IsDeleted OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramCountryIDP, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstCountry_Delete", 1);
                return 0;
            }
        }
        #endregion DELETE

        #region DDL

        public async Task<string> Country_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCountry_DDL";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

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
                await ErrorLog(1, e.Message, $"uspmstCountry_DDL", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion DDL

        #region DDL
        public async Task<string> CountryCurrency_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstCountryCurrency_DDL";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

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
                await ErrorLog(1, e.Message, $"uspmstCountryCurrency_DDL", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion DDL
    }
}
