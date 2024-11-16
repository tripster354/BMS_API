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
    public class BankServices : CommonService, lBankServices
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public BankServices(BMSContext context) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }

        #region INSERT
        
        public async Task<Int64> Bank_Insert(Bank bank)
        {
            try
            {
                SqlParameter paramBankIDP = new SqlParameter
                {
                    ParameterName = "BankIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                    Value = bank.BankIDP
                };
                SqlParameter paramBankName = new SqlParameter("@BankName", bank.BankName);
                SqlParameter paramBankImage = new SqlParameter("@BankImage", (object)bank.BankImage ?? DBNull.Value);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)bank.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };
                string paramSqlQuery = "EXECUTE dbo.uspmstBankDetail_Insert @BankIDP OUTPUT, @BankName, @BankImage, @IsActive,@EntryBy,  @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramBankIDP, paramBankName, paramBankImage, paramIsActive, paramEntryBy,  paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt32(paramBankIDP.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBankDetail_Insert", 1);
                return 0;
            }
        }
        #endregion INSERT

        #region UPDATE
        public async Task<Int64> Bank_Update(Bank bank)
        {
            try
            {
                SqlParameter paramBankIDP = new SqlParameter("@BankIDP", bank.BankIDP);
                SqlParameter paramBankName = new SqlParameter("@BankName", bank.BankName);
                SqlParameter paramBankImage = new SqlParameter("@BankImage", (object)bank.BankImage ?? DBNull.Value );
                SqlParameter paramIsActive = new SqlParameter("@IsActive", (object)bank.IsActive ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };
                
                var paramSqlQuery = "EXECUTE dbo.uspmstBankDetail_Update @BankIDP, @BankName, @BankImage, @IsActive,@EntryBy, @IsDuplicate OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramBankIDP, paramBankName, paramBankImage, paramIsActive, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : bank.BankIDP;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBankDetail_Update", 1);
                return 0;
            }
        }
        #endregion UPDATE

        #region GET
        
        public async Task<string> Bank_Get(Int64 BankIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstBankDetail_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@BankIDP", SqlDbType = SqlDbType.BigInt, Value = BankIDP });

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
                await ErrorLog(1, e.Message, $"uspmstBankDetail_Get", 1);
                return null;
            }
        }
        #endregion GET

        #region GET-ALL
        
        public async Task<string> Bank_GetAll(ModelCommonGetAll modelCommonGetAll)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstBankDetail_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = modelCommonGetAll.SearchKeyWord });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageNumber", SqlDbType = SqlDbType.BigInt, Value = modelCommonGetAll.PageNo });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = await command.ExecuteReaderAsync();

           
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
                await ErrorLog(1, e.Message, $"uspmstBankDetail_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL

        #region ACTIVE-INACTIVE
        public async Task<Int64> Bank_ActiveInactive(Int64 BankIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramBankIDP = new SqlParameter("@BankIDP", BankIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                var paramSqlQuery = "EXECUTE dbo.uspmstBankDetail_Update_ActiveInActive @CountryIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramBankIDP, paramIsActive, paramEntryBy);

                return 1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBankDetail_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DELETE        
        public async Task<Int64> Bank_Delete(Int64 BankIDP)
        {
            try
            {
                SqlParameter paramBankIDP = new SqlParameter("@BankIDP", BankIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspmstBankDetail_Delete @BankIDP, @EntryBy,@IsDeleted OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramBankIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspmstBankDetail_Delete", 1);
                return 0;
            }
        }
        #endregion DELETE

        #region DDL

        public async Task<string> Bank_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstBankDetail_DDL";
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
                await ErrorLog(1, e.Message, $"uspmstBankDetail_DDL", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion DDL

       
    }
}
