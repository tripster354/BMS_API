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
using Microsoft.AspNetCore.Mvc;

namespace BMS_API.Services
{
    public class PageMenuService : CommonService, IPageMenuService
    {
        public PageMenuService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region pagemenu insert
        [NonAction]
        public async Task<Int32> PageMenu_Insert(TblPageMenu tblPageMenu)
        {
            try
            {
                SqlParameter paramPageMenuIDP = new SqlParameter
                {
                    ParameterName = "PageMenuIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                    Value = tblPageMenu.PageMenuIDP
                };
                SqlParameter paramTitleMenu = new SqlParameter("@TitleMenu", tblPageMenu.TitleMenu);
                SqlParameter paramTitlePage = new SqlParameter("@TitlePage", tblPageMenu.TitlePage);
                SqlParameter paramPageContent = new SqlParameter("@PageContent", tblPageMenu.PageContent);
                SqlParameter paramSequence = new SqlParameter("@Sequence", tblPageMenu.Sequence);
                SqlParameter paramActiveType = new SqlParameter("@ActiveType", tblPageMenu.ActiveType);
                SqlParameter paramPageMenuIDF = new SqlParameter("@PageMenuIDF", tblPageMenu.PageMenuIDF);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };
                var paramSqlQuery = "EXECUTE dbo.uspPageMenu_Insert @PageMenuIDP OUTPUT, @TitleMenu, @TitlePage, @PageContent, @Sequence, @ActiveType, @PageMenuIDF, @EntryBy, @IsDuplicate OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPageMenuIDP, paramTitleMenu, paramTitlePage, paramPageContent, paramSequence, paramActiveType, paramPageMenuIDF, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt32(paramPageMenuIDP.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPageMenu_Insert PageMenuIDP = {1}", 1);
                return 0;
            }
        }
        #endregion pagemenu insert

        #region pagemenu update
        [NonAction]
        public async Task<Int32> PageMenu_Update(TblPageMenu tblPageMenu)
        {
            try
            {
                SqlParameter paramPageMenuIDP = new SqlParameter("@PageMenuIDP", tblPageMenu.PageMenuIDP);
                SqlParameter paramTitleMenu = new SqlParameter("@TitleMenu", tblPageMenu.TitleMenu);
                SqlParameter paramTitlePage = new SqlParameter("@TitlePage", tblPageMenu.TitlePage);
                SqlParameter paramPageContent = new SqlParameter("@PageContent", tblPageMenu.PageContent);
                SqlParameter paramSequence = new SqlParameter("@Sequence", tblPageMenu.Sequence);
                SqlParameter paramActiveType = new SqlParameter("@ActiveType", tblPageMenu.ActiveType);
                SqlParameter paramPageMenuIDF = new SqlParameter("@PageMenuIDF", tblPageMenu.PageMenuIDF);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };
                var paramSqlQuery = "EXECUTE dbo.uspPageMenu_Update @PageMenuIDP, @TitleMenu, @TitlePage, @PageContent, @Sequence, @ActiveType, @PageMenuIDF, @EntryBy, @IsDuplicate OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPageMenuIDP, paramTitleMenu, paramTitlePage, paramPageContent, paramSequence, paramActiveType, paramPageMenuIDF, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : tblPageMenu.PageMenuIDP;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPageMenu_Insert PageMenuIDP = {tblPageMenu.PageMenuIDP}", 1);
                return 0;
            }
        }
        #endregion pagemenu update

        #region pagemenu delete
        [NonAction]
        public async Task<Int32> PageMenu_Delete(Int64 pageMenuIDP)
        {
            try
            {
                SqlParameter paramPageMenuIDP = new SqlParameter("@PageMenuIDP", pageMenuIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                // OutParamater isDeleted
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspPageMenu_Delete @PageMenuIDP, @EntryBy, @IsDeleted OUTPUT";

                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPageMenuIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"PageMenu_Delete PageMenuIDP = {pageMenuIDP}", 1);
                return 0;
            }
        }
        #endregion pagemenu delete

        #region pagemenu get
        [NonAction]
        public async Task<string> PageMenu_Get(Int64 pageMenuIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspPageMenu_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageMenuIDP", SqlDbType = SqlDbType.BigInt, Value = pageMenuIDP });

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
                await ErrorLog(1, e.Message, $"uspPageMenu_Get", 1);
                return null;
            }
        }
        #endregion pagemenu get

        #region pagemenu get all
        [NonAction]
        public async Task<string> PageMenu_GetAll()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspPageMenu_GetAll";
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
                await ErrorLog(1, e.Message, $"uspPageMenu_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion pagemenu get all

        #region pagemenu active inactive
        [NonAction]
        public async Task<Int32> PageMenu_Active_InActive(Int64 pageMenuIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramPageMenuIDP = new SqlParameter("@PageMenuIDP", pageMenuIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspPageMenu_Active_InActive @PageMenuIDP, @IsActive, @EntryBy";

                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPageMenuIDP, paramIsActive, paramEntryBy);

                return 1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPageMenu_Active_InActive PageMenuIDP = {pageMenuIDP}", 1);
                return 0;
            }
        }
        #endregion pagemenu active inactive

        #region pagemenu get detail
        [NonAction]
        public async Task<string> PageMenu_GetDetail(string pageMenuName)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspPageMenu_GetDetail";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageMenuName", SqlDbType = SqlDbType.NVarChar, Value = pageMenuName });

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
                await ErrorLog(1, e.Message, $"uspPageMenu_GetDetail", 1);
                return null;
            }
        }
        #endregion pagemenu get

    }
}
