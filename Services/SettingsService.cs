using BudgetManagement.Controllers;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models.Utility;
using System.Collections.Generic;
using System.Data.Common;
using static BMS_API.Services.Interface.ICommon;
using BMS_API.Services.Interface;

namespace BudgetManagement.Services
{
    public class SettingsService : CommonService, ISettingsService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public SettingsService(BMSContext context) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }

        
        public async Task<string> Admin_GetSettings()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspsysAdmin_Get";
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
                //await ErrorLog(1, e.Message, $"uspsysAdmin_Get", 1);
                return "Error, Something wrong!";
            }
        }

        
        public async Task<Int64> Admin_SMTP(SettingsSMTP settingsSMTP)
        {
            try
            {
                SqlParameter paramSMTPEmailAddress = new SqlParameter() { ParameterName = "@SMTPEmailAddress", SqlDbType = SqlDbType.NVarChar, Value = settingsSMTP.SMTPEmailAddress };
                SqlParameter paramSMTPUserName = new SqlParameter() { ParameterName = "@SMTPUserName", SqlDbType = SqlDbType.NVarChar, Value = settingsSMTP.SMTPUserName };
                SqlParameter paramSMTPPassword = new SqlParameter() { ParameterName = "@SMTPPassword", SqlDbType = SqlDbType.NVarChar, Value = settingsSMTP.SMTPPassword };
                SqlParameter paramSMTPHost = new SqlParameter() { ParameterName = "@SMTPHost", SqlDbType = SqlDbType.NVarChar, Value = settingsSMTP.SMTPHost };
                SqlParameter paramSMTPPort = new SqlParameter() { ParameterName = "@SMTPPort", SqlDbType = SqlDbType.Int, Value = settingsSMTP.SMTPPort };
                SqlParameter paramSMTPSSL = new SqlParameter() { ParameterName = "@SMTPSSL", SqlDbType = SqlDbType.Bit, Value = settingsSMTP.SMTPSSL };
                SqlParameter paramEntryBy = new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.Int, Value = ObjUser.UserID };
                
                var paramSqlQuery = "EXECUTE dbo.uspsysAdmin_Update_SMTP @SMTPEmailAddress, @SMTPUserName, @SMTPPassword, @SMTPHost, @SMTPPort, @SMTPSSL, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramSMTPEmailAddress, paramSMTPUserName, paramSMTPPassword, paramSMTPHost, paramSMTPPort, paramSMTPSSL, paramEntryBy);

                return 1;
            }
            catch (Exception e)
            {
                //await ErrorLog(1, e.Message, $"uspsysAdmin_Update_SMTP", 1);
                return 0;
            }
        }
        
        //public async Task<Int64> Admin_ScoreRance(SettingsScoreRange scoreRange)
        //{
        //    try
        //    {
        //        SqlParameter paramScoreLevel0_From = new SqlParameter() { ParameterName = "@ScoreLevel0_From", SqlDbType = SqlDbType.Int, Value = scoreRange.ScoreLevel0_From };
        //        SqlParameter paramScoreLevel1_From = new SqlParameter() { ParameterName = "@ScoreLevel1_From", SqlDbType = SqlDbType.Int, Value = scoreRange.ScoreLevel1_From };
        //        SqlParameter paramScoreLevel1_To = new SqlParameter() { ParameterName = "@ScoreLevel1_To", SqlDbType = SqlDbType.Int, Value = scoreRange.ScoreLevel1_To };
        //        SqlParameter paramScoreLevel2_From = new SqlParameter() { ParameterName = "@ScoreLevel2_From", SqlDbType = SqlDbType.Int, Value = scoreRange.ScoreLevel2_From };
        //        SqlParameter paramScoreLevel2_To = new SqlParameter() { ParameterName = "@ScoreLevel2_To", SqlDbType = SqlDbType.Int, Value = scoreRange.ScoreLevel2_To };
        //        SqlParameter paramScoreLevel3_From = new SqlParameter() { ParameterName = "@ScoreLevel3_From", SqlDbType = SqlDbType.Int, Value = scoreRange.ScoreLevel3_From };

        //        SqlParameter paramScoreLevel0_Name = new SqlParameter() { ParameterName = "@ScoreLevel0_Name", SqlDbType = SqlDbType.NVarChar, Value = scoreRange.ScoreLevel0_Name };
        //        SqlParameter paramScoreLevel1_Name = new SqlParameter() { ParameterName = "@ScoreLevel1_Name", SqlDbType = SqlDbType.NVarChar, Value = scoreRange.ScoreLevel1_Name };
        //        SqlParameter paramScoreLevel2_Name = new SqlParameter() { ParameterName = "@ScoreLevel2_Name", SqlDbType = SqlDbType.NVarChar, Value = scoreRange.ScoreLevel2_Name };
        //        SqlParameter paramScoreLevel3_Name = new SqlParameter() { ParameterName = "@ScoreLevel3_Name", SqlDbType = SqlDbType.NVarChar, Value = scoreRange.ScoreLevel3_Name };

        //        SqlParameter paramEntryBy = new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.Int, Value = ObjUser.UserID };
                
        //        var paramSqlQuery = "EXECUTE dbo.uspsysAdmin_Update_ScoreLevel @ScoreLevel0_From, @ScoreLevel1_From, @ScoreLevel1_To, @ScoreLevel2_From, @ScoreLevel2_To, @ScoreLevel3_From, @ScoreLevel0_Name, @ScoreLevel1_Name, @ScoreLevel2_Name, @ScoreLevel3_Name";
        //        await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramScoreLevel0_From, paramScoreLevel1_From, paramScoreLevel1_To, paramScoreLevel2_From, paramScoreLevel2_To, paramScoreLevel3_From, paramScoreLevel0_Name, paramScoreLevel1_Name, paramScoreLevel2_Name, paramScoreLevel3_Name);

        //        return 1;
        //    }
        //    catch (Exception e)
        //    {
        //        //await ErrorLog(1, e.Message, $"uspsysAdmin_Update_ScoreLevel", 1);
        //        return 0;
        //    }
        //}
    }
}
