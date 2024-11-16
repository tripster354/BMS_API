using BudgetManagement.Controllers;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Data.Common;
using BMS_API.Services.Interface;

namespace BudgetManagement.Services
{
    public class EmailTemplateService : CommonController, IEmailTemplateService
    {
        public EmailTemplateService(BMSContext context, IAuthService authService) : base(context, authService)
        {
            _context = context;
            _authService = authService;
        }
        public AuthorisedUser ObjUser { get; set; }

        #region email-template update
        [NonAction]
        public async Task<Int64> EmailTemplate_Update(MstEmailTemplate mstEmail)
        {
            try
            {
                SqlParameter paramEmailTemplateIdp = new SqlParameter() { ParameterName = "@EmailTemplateIDP", SqlDbType = SqlDbType.BigInt, Value = mstEmail.EmailTemplateIdp};
                //SqlParameter paramEmailTemplateIdp = new SqlParameter
                //{
                //    ParameterName = "@EmailTemplateIDP",
                //    SqlDbType = System.Data.SqlDbType.BigInt,
                //    Direction = System.Data.ParameterDirection.Output,
                //    Value = mstEmail.EmailTemplateIdp
                //};
                SqlParameter paramTemplateName = new SqlParameter() { ParameterName = "@TemplateName", SqlDbType = SqlDbType.NVarChar, Value = mstEmail.TemplateName };
                SqlParameter paramIsEmail = new SqlParameter() { ParameterName = "@IsEmail", SqlDbType = SqlDbType.Bit, Value = (mstEmail.IsEmail == null) ? true : mstEmail.IsEmail };
                SqlParameter paramIsPush = new SqlParameter() { ParameterName = "@IsPush", SqlDbType = SqlDbType.Bit, Value = (mstEmail.IsPush == null) ? true : mstEmail.IsPush };
                SqlParameter paramIsSMS = new SqlParameter() { ParameterName = "@IsSMS", SqlDbType = SqlDbType.Bit, Value = (mstEmail.IsSms == null) ? true : mstEmail.IsSms };
                SqlParameter paramEmailSubject = new SqlParameter() { ParameterName = "@EmailSubject", SqlDbType = SqlDbType.NVarChar, Value = (mstEmail.EmailSubject == null) ? "" : mstEmail.EmailSubject };
                SqlParameter paramEmailContent = new SqlParameter() { ParameterName = "@EmailContent", SqlDbType = SqlDbType.NVarChar, Value = (mstEmail.EmailContent == null) ? "" : mstEmail.EmailContent };
                SqlParameter paramPushContent = new SqlParameter() { ParameterName = "@PushContent", SqlDbType = SqlDbType.NVarChar, Value = (mstEmail.PushContent == null) ? "" : mstEmail.PushContent };
                SqlParameter paramSMSContent = new SqlParameter() { ParameterName = "@SMSContent", SqlDbType = SqlDbType.NVarChar, Value = (mstEmail.Smscontent == null) ? "" : mstEmail.Smscontent };
                SqlParameter paramTemplateVarible = new SqlParameter() { ParameterName = "@TemplateVarible", SqlDbType = SqlDbType.NVarChar, Value = (mstEmail.TemplateVarible == null) ? "" : mstEmail.TemplateVarible };
                SqlParameter paramReceiverID = new SqlParameter() { ParameterName = "@ReceiverID", SqlDbType = SqlDbType.NVarChar, Value = (mstEmail.ReceiverID == null) ? "" : mstEmail.ReceiverID };
                SqlParameter paramIsActive = new SqlParameter() { ParameterName = "@IsActive", SqlDbType = SqlDbType.Bit, Value = (mstEmail.IsActive == null) ? true : mstEmail.IsActive };                
                SqlParameter paramEntryBy = new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.Int, Value = ObjUser.UserID };
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };
                var paramSqlQuery = "EXECUTE dbo.uspmstEmailTemplate_Update @EmailTemplateIDP, @TemplateName, @IsEmail, @IsPush, @IsSMS, @EmailSubject, @EmailContent, @PushContent, @SMSContent, @TemplateVarible, @ReceiverID, @IsActive, @EntryBy, @IsDuplicate OUTPUT";

                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramEmailTemplateIdp, paramTemplateName, paramIsEmail, paramIsPush, paramIsSMS, paramEmailSubject, paramEmailContent, paramPushContent, paramSMSContent, paramTemplateVarible, paramReceiverID, paramIsActive , paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt32(paramEmailTemplateIdp.Value);
                //return Convert.ToInt64(paramEmailTemplateIdp.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"EmailTemplate_Update EmailTemplateIDP = {mstEmail.EmailTemplateIdp}", 1);
                return 0;
            }
        }
        #endregion email-template update

        #region email-template get
        [NonAction]
        public async Task<string> EmailTemplate_Get(Int64 emailTemplateIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstEmailTemplate_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@EmailTemplateIDP", SqlDbType = SqlDbType.BigInt, Value = emailTemplateIDP });

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
                await ErrorLog(1, e.Message, $"EmailTemplate_Get", 1);
                return "Error, Something wrong!";
            }

        }
        #endregion GET

        #region GET-BY-EmailTemplateType
        [NonAction]
        public async Task<string> EmailTemplate_GetByTemplateType(Byte EmailTemplateType)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstEmailTemplate_Getby_EmailTemplateType";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@EmailTemplateType", SqlDbType = SqlDbType.BigInt, Value = EmailTemplateType });

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
                await ErrorLog(1, e.Message, $"uspmstEmailTemplate_Getby_EmailTemplateType", 1);
                return "Error, Something wrong!";
            }

        }
        #endregion GET-BY-EmailTemplateType

        #region GET-ALL
        [NonAction]
        public async Task<string> EmailTemplate_GetAll()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstEmailTemplate_GetAll";
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
                await ErrorLog(1, e.Message, $"uspmstEmailTemplate_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion email-template get all
    }
}
