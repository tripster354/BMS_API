using BMS_API.Services.Interface.User;
using BudgetManagement.Models.Utility;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System;
using BMS_API.Models.User;
using BMS_API.Services.Interface.Partner;
using BMS_API.Models.Partner;
using BMS_API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BMS_API.Services.User
{
    public class tblUserService : CommonService, ItblUserService
    {
        public tblUserService(BMSContext context) : base(context)
        {
            _context = context;
        }

        public AuthorisedUser ObjUser { get; set; }


        #region tblUser GET_ALL
        public async Task<string> tblUser_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUser_GetAll";
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
                await ErrorLog(1, e.Message, $"uspUser_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion tblUser GET_ALL

        #region tblUser GET
        public async Task<string> tblUser_Get(Int64 userIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUser_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = userIDP });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "activityData";
                    ds.Tables[2].TableName = "favouriteData";
                    ds.Tables[3].TableName = "interestData";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspUser_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion tblUser GET

        #region UPDATE-MYPROFILE
        public async Task<Int64> User_Update_MyProfile(tblUser modelUser)
        {
            try
            {
                
                SqlParameter paramUserIdp = new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID };
                SqlParameter paramFullName = new SqlParameter() { ParameterName = "@FullName", SqlDbType = SqlDbType.NVarChar, Value = modelUser.FullName };
                SqlParameter paramEmail = new SqlParameter() { ParameterName = "@EmailID", SqlDbType = SqlDbType.NVarChar, Value = modelUser.EmailID== "null" ? "" : modelUser.EmailID };
                SqlParameter paramProfileImage = new SqlParameter() { ParameterName = "@ProfileImage", SqlDbType = SqlDbType.NVarChar, Value = (object)modelUser.ProfileImage ?? DBNull.Value };
                SqlParameter paramAddress = new SqlParameter() { ParameterName = "@Address", SqlDbType = SqlDbType.NVarChar, Value = modelUser.Address == "null" ? "" : modelUser.Address };
                SqlParameter paramSocialGoogle = new SqlParameter() { ParameterName = "@SocialGoogle", SqlDbType = SqlDbType.NVarChar, Value = modelUser.SocialGoogle == null ? "" : modelUser.SocialGoogle };
                SqlParameter paramSocialFacebook = new SqlParameter() { ParameterName = "@SocialFacebook", SqlDbType = SqlDbType.NVarChar, Value = (modelUser.SocialFacebook == null ? "" :  modelUser.SocialFacebook) };
                SqlParameter paramSocialLinkedIn = new SqlParameter() { ParameterName = "@SocialLinkedIn", SqlDbType = SqlDbType.NVarChar, Value = modelUser.SocialLinkedIn == null ? "" : modelUser.SocialLinkedIn };
                SqlParameter paramSocialInstagram = new SqlParameter() { ParameterName = "@SocialInstagram", SqlDbType = SqlDbType.NVarChar, Value = modelUser.SocialInstagram == null ? "" : modelUser.SocialInstagram };

                SqlParameter paramSocialTweeter = new SqlParameter() { ParameterName = "@SocialTweeter", SqlDbType = SqlDbType.NVarChar, Value = modelUser.SocialTweeter == null ? "" : modelUser.SocialTweeter };
                SqlParameter paramSocialTelegram = new SqlParameter() { ParameterName = "@SocialTelegram", SqlDbType = SqlDbType.NVarChar, Value = modelUser.SocialTelegram == "null" ? "" : modelUser.SocialTelegram };
                SqlParameter paramSocialOther = new SqlParameter() { ParameterName = "@SocialOther", SqlDbType = SqlDbType.NVarChar, Value = modelUser.SocialOther == "null" ? "" : modelUser.SocialOther };
                SqlParameter paramAboutMe = new SqlParameter() { ParameterName = "@AboutMe", SqlDbType = SqlDbType.NVarChar, Value = modelUser.AboutMe == "null" ? "" : modelUser.AboutMe };
                SqlParameter paramEntryBy = new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.Int, Value = ObjUser.UserID };
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };
                var paramSqlQuery = "EXECUTE dbo.uspUser_Update_MyProfile @UserIDP, @FullName, @EmailID, @ProfileImage, @Address, @SocialGoogle,@SocialFacebook, @SocialLinkedIn, @SocialInstagram,@SocialTweeter,@SocialTelegram,@SocialOther, @AboutMe,@EntryBy, @IsDuplicate OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIdp, paramFullName, paramEmail, paramProfileImage, paramAddress, paramSocialGoogle, paramSocialFacebook, paramSocialLinkedIn, paramSocialInstagram, paramSocialTweeter, paramSocialTelegram, paramSocialOther, paramAboutMe, paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : ObjUser.UserID;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspsysUser_Update_MyProfile", 1);
                return 0;
            }
        }
        #endregion UPDATE-MYPROFILE


        #region tblUser ACTIVE INACTIVE
        public async Task<Int64> tblUser_ActiveInactive(Int64 userIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramUserIDP = new SqlParameter("@UserIDP", userIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspUser_Update_ActiveInActive @UserIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspUser_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion tblUser ACTIVE INACTIVE

        #region GET all Booking 
        public async Task<string> User_Get_FullDetail(Int64 activityID)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_Get_Fulldetail_ByUser";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityID", SqlDbType = SqlDbType.BigInt, Value = activityID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "participateData";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);


                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_Get_Fulldetail_ByUser", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET all Booking 

        #region  Update Interest
        public async Task<Int64> User_Update_Interest(MyInterest myinterest)
        {
            try
            {
                SqlParameter paramUserIDF = new SqlParameter("@UserIDP", ObjUser.UserID);
                SqlParameter paramInterestIDP = new SqlParameter("@IntersetIDs", myinterest.InterestIDs);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspUser_Update_Interest @UserIDP, @IntersetIDs, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIDF, paramInterestIDP, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspUser_Update_Interest", 1);
                return 0;
            }
        }
        #endregion  Update Interest

        #region GET-ALL Interest   
        public async Task<string> User_GetAll_Interest()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstInterest_GetAll_With_UserSelected";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });

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
                await ErrorLog(1, e.Message, $"uspmstInterest_GetAll_With_UserSelected", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL Interest  

        #region GET-ALL Banner   
        public async Task<string> User_GetAll_Banner()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspmstBanner_GetAll_User";
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
                await ErrorLog(1, e.Message, $"uspmstBanner_GetAll_User", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL Banner  

        #region  Insert Contact List
        public async Task<Int64> User_Insert_Contact(ContactList contactList, string connection)
        {
            try
            {
                

                List<SyncUserContact> contactDetailList = new List<SyncUserContact>();
                JsonConvert.PopulateObject(contactList.contactList, contactDetailList);

                /*dynamic ivDtls = JsonConvert.DeserializeObject(Convert.ToString(contactList.contactList));
                JToken m1 = ((Newtonsoft.Json.Linq.JContainer)ivDtls);*/
                /* foreach (var contact in contactDetailList)
                 {
                     Int64 contactNumber = Convert.ToInt64(RemoveSpecialChars(contact.MobileNo));
                     SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                     SqlParameter paramContactName = new SqlParameter("@ContactName", contact.ContactName);
                     SqlParameter paramMobile = new SqlParameter("@Mobile", contactNumber);
                     SqlParameter paramProfileImage = new SqlParameter("@ProfileImage", contact.ProfileImage);
                     SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                     var paramSqlQuery = "EXECUTE dbo.usptblContactList_Insert @UserIDF, @ContactName, @Mobile,@ProfileImage, @EntryBy";
                     _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIDF, paramContactName, paramMobile, paramProfileImage, paramEntryBy);

                 }*/
                DataTable dtImport = new DataTable();
                dtImport.Columns.AddRange(new DataColumn[3]
                {
                    new DataColumn("ContactName",typeof(string)),
                    new DataColumn("Mobile",typeof(string)),
                    new DataColumn("ProfileImage",typeof(string))
                });
                dynamic ivDtls = JsonConvert.DeserializeObject(contactList.contactList.ToString());
                JToken m1 = ((Newtonsoft.Json.Linq.JContainer)ivDtls);
                Int32 index = 0;
                index++;
                foreach (dynamic ivDtl in m1)
                {
                    if (ivDtl != null)
                    {
                        Int64 contactNumber = Convert.ToInt64(RemoveSpecialChars(ivDtl["MobileNo"].ToString()));
                        DataRow _row = dtImport.NewRow();
                        _row["ContactName"] = (ivDtl["ContactName"].ToString() == null) ? "" : ivDtl["ContactName"].ToString();
                        _row["Mobile"] = (ivDtl["MobileNo"].ToString() == null) ? "" : contactNumber;
                        _row["ProfileImage"] = (ivDtl["ProfileImage"].ToString() == null) ? "" : ivDtl["ProfileImage"].ToString(); 
                        dtImport.Rows.Add(_row);
                    }
                }
                string commandText = "usptblContactList_Insert";
                using (SqlConnection conn = new SqlConnection(connection))
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    AddInParameter(cmd, "@UserIDF", SqlDbType.BigInt, ObjUser.UserID);
                    SqlParameter sqlParamData = cmd.Parameters.AddWithValue("@actData", dtImport);
                    AddInParameter(cmd, "EntryBy", SqlDbType.Int, (Int32)ObjUser.UserID);
                    sqlParamData.SqlDbType = SqlDbType.Structured;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"usptblContactList_Insert", 1);
                return 0;
            }
        }
        #endregion  Insert Contact List

        #region GET all User Activity  
        public async Task<string> User_Activity_GetAll(SearchWithDisplayType searchWithDisplayType)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUserActivity_GetAll_ByUserIDF";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@DisplayTypeID", SqlDbType = SqlDbType.Int, Value = searchWithDisplayType.DisplayTypeID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = searchWithDisplayType.SearchKeyWord });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "activityData";
                    ds.Tables[1].TableName = "trandingData";
                    ds.Tables[2].TableName = "interestData";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspUserActivity_GetAll_ByUserIDF", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET all User Activity  


        #region GET-ALL User_ContactList_GetAll   
        public async Task<string> User_ContactList_GetAll(Search param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspContactDetail_Get_All";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = param.SearchKeyWord });

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
                await ErrorLog(1, e.Message, $"uspContactDetail_Get_All", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL User_ContactList_GetAll

        #region User Feed INSERT
        public async Task<Int64> User_Feed_Insert(ModelFeed modelFeed, string connection)
        {
            Int64 feedIDP = 0;
            try
            {
                DataTable dtImport = new DataTable();
                dtImport.Columns.AddRange(new DataColumn[2]
                {
                    new DataColumn("UserID",typeof(Int64)),
                    new DataColumn("ContactID",typeof(Int64))
                });
                dynamic ivDtls = JsonConvert.DeserializeObject(modelFeed.participateList.ToString());
                JToken m1 = ((Newtonsoft.Json.Linq.JContainer)ivDtls);
                Int32 index = 0;
                index++;
                foreach (dynamic ivDtl in m1)
                {
                    if (ivDtl != null)
                    {
                        DataRow _row = dtImport.NewRow();
                        _row["UserID"] = (ivDtl["UserID"] == null ? 0 : Convert.ToInt64(ivDtl["UserID"]));
                        _row["ContactID"] = (ivDtl["ContactID"] == null ? 0 : Convert.ToInt64(ivDtl["ContactID"]));
                        dtImport.Rows.Add(_row);
                    }
                }
                string commandText = "uspFeed_Insert";
                using (SqlConnection conn = new SqlConnection(connection))
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter paramFeedIDP = new SqlParameter("@FeedIDP", SqlDbType.BigInt)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramFeedIDP);
                    AddInParameter(cmd, "@PartnerIDF", SqlDbType.BigInt, 0);
                    AddInParameter(cmd, "@UserIDF", SqlDbType.BigInt, ObjUser.UserID);
                    AddInParameter(cmd, "@Banner", SqlDbType.NVarChar, modelFeed.BannerAttachment);
                    AddInParameter(cmd, "@Title", SqlDbType.NVarChar, modelFeed.Title);
                    AddInParameter(cmd, "@ActivityIDF", SqlDbType.BigInt, modelFeed.ActivityIDF);

                    SqlParameter sqlParamData = cmd.Parameters.AddWithValue("@actData", dtImport);
                    AddInParameter(cmd, "EntryBy", SqlDbType.Int, (Int32)ObjUser.UserID);
                    sqlParamData.SqlDbType = SqlDbType.Structured;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    feedIDP = Convert.ToInt64(paramFeedIDP.Value);
                    conn.Close();
                    return feedIDP;
                }

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_Insert", 1);
                return 0;
            }
        }
        #endregion User Feed INSERT


        #region Feed Update
        public async Task<Int64> User_Feed_Update(ModelFeed modelFeed)
        {

            try
            {
                SqlParameter paramFeedIDP = new SqlParameter("@FeedIDP", (object)modelFeed.FeedIDP ?? DBNull.Value);
                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
                SqlParameter paramBanner = new SqlParameter("@Banner", (object)modelFeed.BannerAttachment ?? DBNull.Value);
                SqlParameter paramTitle = new SqlParameter("@Title", (object)modelFeed.Title ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspFeed_Update @FeedIDP, @PartnerIDF, @Banner, @Title,@EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramFeedIDP, paramPartnerIDF,paramBanner, paramTitle, paramEntryBy);

                return Convert.ToInt64(paramFeedIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_Update", 1);
                return 0;
            }
        }
        #endregion Feed Update


        #region User Activity DDL
        public async Task<string> User_Activity_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_DDL_ByUser";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });

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
                await ErrorLog(1, e.Message, $"uspActivity_DDL_ByUser", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion User Activity DDL


        #region GET all Feed 
        public async Task<string> User_Feed_Get_All(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspFeed_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = 0});
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageCrr", SqlDbType = SqlDbType.Int, Value = param.PageNo });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = param.SearchKeyWord });


                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "feedData";
                    ds.Tables[1].TableName = "feedParticipateData";
                    ds.Tables[2].TableName = "feedComentData";

                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET all Feed


        #region Update Feed comment
        public async Task<Int64> User_Update_Feed_Comment(ModelFeedComment modelafeedcomment)
        {
            try
            {
                SqlParameter paramFeedParticipantIDP = new SqlParameter
                {
                    ParameterName = "@FeedParticipantIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                SqlParameter paramFeedIDF = new SqlParameter("@FeedIDF", modelafeedcomment.FeedIDF);
                SqlParameter paramPartnerIDF = new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = 0 };
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramComment = new SqlParameter("@Comment", modelafeedcomment.Comment);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspFeed_Update_Comment @FeedParticipantIDP OUTPUT, @FeedIDF, @PartnerIDF, @UserIDF, @Comment, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramFeedParticipantIDP, paramFeedIDF, paramPartnerIDF, paramUserIDF, paramComment, paramEntryBy);

                return Convert.ToInt64(paramFeedParticipantIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_Update_Comment", 1);
                return 0;
            }
        }
        #endregion Update Feed comment


        #region GET all Feed Comment
        public async Task<string> User_Feed_Get_All_Comment(ModelFeedCommentAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspFeed_GetAll_Comment";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@FeedIDF", SqlDbType = SqlDbType.BigInt, Value = param.FeedIDF });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PageCrr", SqlDbType = SqlDbType.BigInt, Value = param.PageCrr });

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
                await ErrorLog(1, e.Message, $"uspFeed_GetAll_Comment", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET all Feed Comment


        #region Feed DELETE
        public async Task<Int64> User_Feed_Delete(Int64 feedIDP)
        {
            try
            {
                SqlParameter paramFeedIDP = new SqlParameter("@FeedIDP", feedIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspFeed_Delete @FeedIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramFeedIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_Delete", 1);
                return 0;
            }
        }
        #endregion Feed DELETE


        #region Update Feed reaction
        public async Task<Int64> User_Feed_Update_Reaction(ModelFeedUpdateReaction modelfeed)
        {
            try
            {

                SqlParameter paramFeedIDF = new SqlParameter("@FeedIDF", modelfeed.FeedIDF);
                SqlParameter paramPartnerIDF = new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = 0 };
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramFeedLike = new SqlParameter("@FeedLike", modelfeed.FeedLike);
                SqlParameter paramFeedSmile = new SqlParameter("@FeedSmile", modelfeed.FeedSmile);
                SqlParameter paramFeedBook = new SqlParameter("@FeedBook", modelfeed.FeedBook);
                SqlParameter paramFeedHeart = new SqlParameter("@FeedHeart", modelfeed.FeedHeart);
                SqlParameter paramFeedNetural = new SqlParameter("@FeedNetural", modelfeed.FeedNetural);
                SqlParameter paramFeedStar = new SqlParameter("@FeedStar", modelfeed.FeedStar);
                SqlParameter paramFeedFavourite = new SqlParameter("@FeedFavourite", modelfeed.FeedFavourite);

                var paramSqlQuery = "EXECUTE dbo.uspFeed_Update_Reaction @FeedIDF, @PartnerIDF, @UserIDF, @FeedLike, @FeedSmile,@FeedBook,@FeedHeart,@FeedNetural,@FeedStar,@FeedFavourite";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramFeedIDF, paramPartnerIDF, paramUserIDF, paramFeedLike, paramFeedSmile, paramFeedBook, paramFeedHeart, paramFeedNetural, paramFeedStar, paramFeedFavourite);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_Update_Comment", 1);
                return 0;
            }
        }
        #endregion Update Feed reaction

        #region GET Feed 
        public async Task<string> User_Feed_Get(Int64 FeedIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspFeed_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@FeedIDF", SqlDbType = SqlDbType.BigInt, Value = FeedIDP });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "feedData";
                    ds.Tables[1].TableName = "feedParticipateData";

                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET Feed

        #region Get All Trending Tutor 
        public async Task<string> User_Get_All_Trending_Tutor()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspPartner_GetAll_By_Tranding";
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
                await ErrorLog(1, e.Message, $"uspPartner_GetAll_By_Tranding", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Get All Trending Tutor


        #region Get All Sugested User
        public async Task<string> User_Get_All_Suggested_User()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUser_GetAll_By_Suggested";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });

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
                await ErrorLog(1, e.Message, $"uspUser_GetAll_By_Suggested", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Get All Sugested User

        #region  GET Club subscription
        public async Task<string> User_Get_Club_Subscription_Status(Int64 clubIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspClubSubscription_Get_SubscriptionStatus_By_ClubIDF";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ClubIDF", SqlDbType = SqlDbType.BigInt, Value = clubIDP });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = 0 });

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
                await ErrorLog(1, e.Message, $"uspClubSubscription_Get_SubscriptionStatus_By_ClubIDF", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET Club subscription

        [NonAction]
        public string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", "+", " ", "-", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }
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
