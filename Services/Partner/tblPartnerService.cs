using BMS_API.Services.Interface.Partner;
using BudgetManagement.Models.Utility;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System;
using BMS_API.Models.Partner;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BMS_API.Models.User;
using BMS_API.Models;
using System.Globalization;

namespace BMS_API.Services.Partner
{
    public class tblPartnerService : CommonService, ItblPartnerService
    {
        public tblPartnerService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }


        #region tblPartner Registration
        public async Task<Int64> tblPartner_Registration(tblPartner modeltblPartner)
        {
            try
            {
                SqlParameter paramPartnerIDP = new SqlParameter
                {
                    ParameterName = "@PartnerIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramFullName = new SqlParameter("@FullName", (object)modeltblPartner.FullName ?? DBNull.Value);
                SqlParameter paramMobileNo = new SqlParameter("@MobileNo", (object)modeltblPartner.MobileNo ?? DBNull.Value);
                SqlParameter paramEmailID = new SqlParameter("@EmailID", (object)modeltblPartner.EmailID ?? DBNull.Value);
                SqlParameter paramPassword = new SqlParameter("@Password", (object)modeltblPartner.Password ?? DBNull.Value);
                SqlParameter paramActivityTypeID = new SqlParameter("@ActivityTypeID", (object)modeltblPartner.ActivityTypeID ?? DBNull.Value);
                SqlParameter paramKYCAttachment1 = new SqlParameter("@KYCAttachment1", (object)modeltblPartner.KYCAttachment1 ?? DBNull.Value);
                SqlParameter paramKYCAttachment2 = new SqlParameter("@KYCAttachment2", (object)modeltblPartner.KYCAttachment2 ?? DBNull.Value);
                SqlParameter paramKYCAttachment3 = new SqlParameter("@KYCAttachment3", (object)modeltblPartner.KYCAttachment3 ?? DBNull.Value);
                SqlParameter paramKYCAttachment4 = new SqlParameter("@KYCAttachment4", (object)modeltblPartner.KYCAttachment4 ?? DBNull.Value);
                SqlParameter paramVideoAttachment = new SqlParameter("@VideoAttachment", (object)modeltblPartner.VideoAttachment ?? DBNull.Value);
                SqlParameter paramSocialFacebook = new SqlParameter("@SocialFacebook", (object)modeltblPartner.SocialFacebook ?? DBNull.Value);
                SqlParameter paramSocialLinkedIn = new SqlParameter("@SocialLinkedIn", (object)modeltblPartner.SocialLinkedIn ?? DBNull.Value);
                SqlParameter paramSocialInstagram = new SqlParameter("@SocialInstagram", (object)modeltblPartner.SocialInstagram ?? DBNull.Value);
                //SqlParameter paramApplicationStatus = new SqlParameter("@ApplicationStatus", modeltblPartner.ApplicationStatus);
                SqlParameter paramApplicationStatus = new SqlParameter
                {
                    ParameterName = "@ApplicationStatus",
                    SqlDbType = System.Data.SqlDbType.TinyInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspPartner_Registration @PartnerIDP OUTPUT, @FullName, @MobileNo, @EmailID, @Password, @ActivityTypeID, @KYCAttachment1, @KYCAttachment2, @KYCAttachment3, @KYCAttachment4, @VideoAttachment, @SocialFacebook, @SocialLinkedIn, @SocialInstagram, @ApplicationStatus OUTPUT";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramPartnerIDP, paramFullName, paramMobileNo, paramEmailID, paramPassword, paramActivityTypeID, paramKYCAttachment1, paramKYCAttachment2, paramKYCAttachment3, paramKYCAttachment4, paramVideoAttachment, paramSocialFacebook, paramSocialLinkedIn, paramSocialInstagram, paramApplicationStatus);

                if (Convert.ToInt32(paramApplicationStatus.Value) == 0 || Convert.ToInt32(paramApplicationStatus.Value) == 1 || Convert.ToInt32(paramApplicationStatus.Value) == 2 || Convert.ToInt32(paramApplicationStatus.Value) == 3 || Convert.ToInt32(paramApplicationStatus.Value) == 4)
                {
                    return Convert.ToInt64(paramApplicationStatus.Value);
                }
                else
                {
                    return Convert.ToInt64(paramPartnerIDP.Value);
                }
                //return (Convert.ToInt32(paramApplicationStatus.Value) == 0) ? -1 : Convert.ToInt32(paramCountryIDP.Value);
                //return Convert.ToInt64(paramPartnerIDP.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPartner_Registration", 1);
                return 0;
            }
        }
        #endregion tblPartner Registration


        #region tblPartner UPDATE
        //public async Task<Int64> tblPartner_Update(tblPartner modeltblPartner)
        //{
        //    try
        //    {
        //        SqlParameter paramPartnerIDP = new SqlParameter("@PartnerIDP", modeltblPartner.PartnerIDP);
        //        SqlParameter paramFirstName = new SqlParameter("@FirstName", modeltblPartner.FirstName);
        //        SqlParameter paramLastName = new SqlParameter("@LastName", modeltblPartner.LastName);
        //        SqlParameter paramFullName = new SqlParameter("@FullName", modeltblPartner.FullName);
        //        SqlParameter paramCompanyName = new SqlParameter("@CompanyName", modeltblPartner.CompanyName);
        //        SqlParameter paramMobileNo = new SqlParameter("@MobileNo", modeltblPartner.MobileNo);
        //        SqlParameter paramEmailID = new SqlParameter("@EmailID", modeltblPartner.EmailID);
        //        SqlParameter paramPassword = new SqlParameter("@Password", modeltblPartner.Password);
        //        SqlParameter paramActivityTypeID = new SqlParameter("@ActivityTypeID", modeltblPartner.ActivityTypeID);
        //        SqlParameter paramKYCTypeID = new SqlParameter("@KYCTypeID", modeltblPartner.KYCTypeID);
        //        SqlParameter paramKYCAttachment1 = new SqlParameter("@KYCAttachment1", modeltblPartner.KYCAttachment1);
        //        SqlParameter paramKYCAttachment2 = new SqlParameter("@KYCAttachment2", modeltblPartner.KYCAttachment2);
        //        SqlParameter paramKYCAttachment3 = new SqlParameter("@KYCAttachment3", modeltblPartner.KYCAttachment3);
        //        SqlParameter paramKYCAttachment4 = new SqlParameter("@KYCAttachment4", modeltblPartner.KYCAttachment4);
        //        SqlParameter paramVideoAttachment = new SqlParameter("@VideoAttachment", modeltblPartner.VideoAttachment);
        //        SqlParameter paramIsApprove = new SqlParameter("@IsApprove", modeltblPartner.IsApprove);
        //        SqlParameter paramIsApproveActionDate = new SqlParameter("@IsApproveActionDate", modeltblPartner.IsApproveActionDate);
        //        SqlParameter paramExperience = new SqlParameter("@Experience", modeltblPartner.Experience);
        //        SqlParameter paramCityIDF = new SqlParameter("@CityIDF", modeltblPartner.CityIDF);
        //        SqlParameter paramRefferedBy = new SqlParameter("@RefferedBy", modeltblPartner.RefferedBy);
        //        SqlParameter paramRegistrationDate = new SqlParameter("@RegistrationDate", modeltblPartner.RegistrationDate);
        //        SqlParameter paramSubscriptionTypeIDF = new SqlParameter("@SubscriptionTypeIDF", modeltblPartner.SubscriptionTypeIDF);
        //        SqlParameter paramSubscriptionStartDate = new SqlParameter("@SubscriptionStartDate", modeltblPartner.SubscriptionStartDate);
        //        SqlParameter paramSubscriptionEndDate = new SqlParameter("@SubscriptionEndDate", modeltblPartner.SubscriptionEndDate);
        //        SqlParameter paramProfileImage = new SqlParameter("@ProfileImage", modeltblPartner.ProfileImage);
        //        SqlParameter paramSocialFacebook = new SqlParameter("@SocialFacebook", modeltblPartner.SocialFacebook);
        //        SqlParameter paramSocialLinkedIn = new SqlParameter("@SocialLinkedIn", modeltblPartner.SocialLinkedIn);
        //        SqlParameter paramSocialInstagram = new SqlParameter("@SocialInstagram", modeltblPartner.SocialInstagram);
        //        SqlParameter paramIsActive = new SqlParameter("@IsActive", modeltblPartner.IsActive);
        //        SqlParameter paramOTP = new SqlParameter("@OTP", modeltblPartner.OTP);
        //        SqlParameter paramLoginToken = new SqlParameter("@LoginToken", modeltblPartner.LoginToken);
        //        SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
        //        SqlParameter paramIsDuplicate = new SqlParameter
        //        {
        //            ParameterName = "@IsDuplicate",
        //            SqlDbType = System.Data.SqlDbType.Bit,
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        var paramSqlQuery = "EXECUTE dbo.uspPartner_Update @PartnerIDP, @FirstName, @LastName, @FullName, @CompanyName, @MobileNo, @EmailID, @Password, @ActivityTypeID, @KYCTypeID, @KYCAttachment1, @KYCAttachment2, @KYCAttachment3, @KYCAttachment4, @VideoAttachment, @IsApprove, @IsApproveActionDate, @Experience, @CityIDF, @RefferedBy, @RegistrationDate, @SubscriptionTypeIDF, @SubscriptionStartDate, @SubscriptionEndDate, @ProfileImage, @SocialFacebook, @SocialLinkedIn, @SocialInstagram, @IsActive, @OTP, @LoginToken, @EntryBy, @IsDuplicate OUTPUT";
        //        await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramPartnerIDP, paramFirstName, paramLastName, paramFullName, paramCompanyName, paramMobileNo, paramEmailID, paramPassword, paramActivityTypeID, paramKYCTypeID, paramKYCAttachment1, paramKYCAttachment2, paramKYCAttachment3, paramKYCAttachment4, paramVideoAttachment, paramIsApprove, paramIsApproveActionDate, paramExperience, paramCityIDF, paramRefferedBy, paramRegistrationDate, paramSubscriptionTypeIDF, paramSubscriptionStartDate, paramSubscriptionEndDate, paramProfileImage, paramSocialFacebook, paramSocialLinkedIn, paramSocialInstagram, paramIsActive, paramOTP, paramLoginToken, paramEntryBy, paramIsDuplicate);

        //        return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramPartnerIDP.Value);

        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"uspPartner_Update", 1);
        //        return 0;
        //    }
        //}
        #endregion tblPartner UPDATE


        #region tblPartner GET
        public async Task<string> tblPartner_Get(Int64 partnerIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspPartner_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDP", SqlDbType = SqlDbType.BigInt, Value = partnerIDP });

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
                await ErrorLog(1, e.Message, $"uspPartner_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion tblPartner GET


        #region tblPartner GET_ALL
        public async Task<string> tblPartner_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspPartner_GetAll";
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
                await ErrorLog(1, e.Message, $"uspPartner_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion tblPartner GET_ALL


        #region tblPartner APPROVE REJECT
        public async Task<Int64> tblPartner_ApproveReject(Int64 partnerIDP, Int32 isApprove)
        {
            try
            {
                SqlParameter paramPartnerIDP = new SqlParameter("@PartnerIDP", partnerIDP);
                SqlParameter paramIsApprove = new SqlParameter("@IsApprove", isApprove);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspPartner_Update_ApproveReject @PartnerIDP, @IsApprove, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPartnerIDP, paramIsApprove, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPartner_Update_ApproveReject", 1);
                return 0;
            }
        }
        #endregion tblPartner APPROVE REJECT


        #region tblPartner ACTIVE INACTIVE
        public async Task<Int64> tblPartner_ActiveInactive(Int64 partnerIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramPartnerIDP = new SqlParameter("@PartnerIDP", partnerIDP);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspPartner_Update_ActiveInActive @PartnerIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPartnerIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPartner_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion tblPartner ACTIVE INACTIVE


        #region tblPartner DDL
        //public async Task<string> tblPartner_DDL()
        //{
        //    try
        //    {
        //        string strResponse = "";
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "uspPartner_DDL";
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
        //        await ErrorLog(1, e.Message, $"uspPartner_DDL", 1);
        //        return "";
        //    }
        //}
        #endregion tblPartner DDL


        #region tblPartner DELETE
        public async Task<Int64> tblPartner_Delete(Int64 partnerIDP)
        {
            try
            {
                SqlParameter paramPartnerIDP = new SqlParameter("@PartnerIDP", partnerIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspPartner_Delete @PartnerIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPartnerIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPartner_Delete", 1);
                return 0;
            }
        }
        #endregion tblPartner DELETE

        #region UPDATE-MYPROFILE
        public async Task<Int64> Partner_Update_MyProfile(tblPartner modeltblPartner)
        {
            try
            {

                SqlParameter paramPartnerIDP = new SqlParameter() { ParameterName = "@PartnerIDP", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID };
                SqlParameter paramFullName = new SqlParameter() { ParameterName = "@FullName", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.FullName };
                SqlParameter paramEmail = new SqlParameter() { ParameterName = "@EmailID", SqlDbType = SqlDbType.NVarChar, Value = modeltblPartner.EmailID == null ? "" : modeltblPartner.EmailID };
                SqlParameter paramProfileImage = new SqlParameter() { ParameterName = "@ProfileImage", SqlDbType = SqlDbType.NVarChar, Value = (object)modeltblPartner.ProfileImage ?? DBNull.Value };
                SqlParameter paramSocialFacebook = new SqlParameter() { ParameterName = "@SocialFacebook", SqlDbType = SqlDbType.NVarChar, Value = ((modeltblPartner.SocialFacebook == null || modeltblPartner.SocialFacebook == "null") ? "" : modeltblPartner.SocialFacebook) };
                SqlParameter paramSocialTweeter = new SqlParameter() { ParameterName = "@SocialTweeter", SqlDbType = SqlDbType.NVarChar, Value = (modeltblPartner.SocialTweeter == null || modeltblPartner.SocialTweeter == "null") ? "" : modeltblPartner.SocialTweeter };
                SqlParameter paramSocialTelegram = new SqlParameter() { ParameterName = "@SocialTelegram", SqlDbType = SqlDbType.NVarChar, Value = (modeltblPartner.SocialTelegram == null || modeltblPartner.SocialTelegram == "null") ? "" : modeltblPartner.SocialTelegram };
                SqlParameter paramSocialOther = new SqlParameter() { ParameterName = "@SocialOther", SqlDbType = SqlDbType.NVarChar, Value = (modeltblPartner.SocialOther == null || modeltblPartner.SocialFacebook == "null") ? "" : modeltblPartner.SocialOther };
                SqlParameter paramKYCAttachment1 = new SqlParameter("@KYCAttachment1", (object)modeltblPartner.KYCAttachment1 ?? DBNull.Value);
                SqlParameter paramKYCAttachment2 = new SqlParameter("@KYCAttachment2", (object)modeltblPartner.KYCAttachment2 ?? DBNull.Value);
                SqlParameter paramKYCAttachment3 = new SqlParameter("@KYCAttachment3", (object)modeltblPartner.KYCAttachment3 ?? DBNull.Value);
                SqlParameter paramKYCAttachment4 = new SqlParameter("@KYCAttachment4", (object)modeltblPartner.KYCAttachment4 ?? DBNull.Value);
                SqlParameter paramExperience = new SqlParameter("@Experience", (object)modeltblPartner.YearofExperience ?? DBNull.Value);
                SqlParameter paramBankDetail = new SqlParameter("@BankDetail", (object)modeltblPartner.BankDetail ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.Int, Value = ObjUser.UserID };
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };
                var paramSqlQuery = "EXECUTE dbo.uspPartner_Update_MyProfile @PartnerIDP, @FullName, @EmailID, @ProfileImage, @SocialFacebook, @SocialTweeter,@SocialTelegram,@SocialOther, @KYCAttachment1, @KYCAttachment2, @KYCAttachment3, @KYCAttachment4, @Experience,@BankDetail, @EntryBy, @IsDuplicate OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramPartnerIDP, paramFullName, paramEmail, paramProfileImage, paramSocialFacebook, paramSocialTweeter, paramSocialTelegram, paramSocialOther, paramKYCAttachment1, paramKYCAttachment2, paramKYCAttachment3, paramKYCAttachment4, paramExperience, paramBankDetail,  paramEntryBy, paramIsDuplicate);

                return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : ObjUser.UserID;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPartner_Update_MyProfile", 1);
                return 0;
            }
        }
        #endregion UPDATE-MYPROFILE

        #region GET Mobile Dashboard summary
        public async Task<string> Partner_Get_Dashboard(Int64 partnerIDP, PartnerDashboard dashboard)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspDashbord_Partner_Summary";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDP", SqlDbType = SqlDbType.BigInt, Value = partnerIDP });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@DateFrom", SqlDbType = SqlDbType.DateTime, Value = dashboard.DateFrom });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@DateTo", SqlDbType = SqlDbType.DateTime, Value = dashboard.DateTo });


                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "revenue";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);


                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspDashbord_Partner_Summary", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET Mobile Dashboard summary


        #region GET all Booking 
        public async Task<string> Partner_Get_All_Booking(Int64 partnerIDP,int displayTypeID)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_GetAll_ByPartner";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = partnerIDP });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@DisplayTypeID", SqlDbType = SqlDbType.Int, Value = displayTypeID });


                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "participatedata";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);


                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_GetAll_ByPartner", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET all Booking 


        #region GET all Booking 
        public async Task<string> Partner_Get_FullDetail_Booking(Int64 activityID)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_Get_Fulldetail_ByPartner";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityID", SqlDbType = SqlDbType.BigInt, Value = activityID });


                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "participatedata";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_Get_Fulldetail_ByPartner", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET all Booking

        #region update Activity actual time
        public async Task<Int64> Activity_UpdateActualTime(ActivityActualTime modelactivity)
        {
            try
            {
         
                SqlParameter paramActivityID = new SqlParameter("@ActivityID", modelactivity.ActivityIDP);
                SqlParameter paramType = new SqlParameter("@Type",modelactivity.Type);
                

                var paramSqlQuery = "EXECUTE dbo.uspActivity_Update_ActualTime @ActivityID, @Type";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramActivityID, paramType);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_Update_ActualTime", 1);
                return 0;
            }
        }
        #endregion update Activity actual time

        #region update Activity attendance
        public async Task<Int64> Activity_UpdateAttendance(ActivityAttendace modelactivityattendace)
        {
            try
            {
                SqlParameter paramAttendanceIDP = new SqlParameter
                {
                    ParameterName = "@AttendanceIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
                SqlParameter paramActivityIDF = new SqlParameter("@ActivityIDF", modelactivityattendace.ActivityIDF);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", modelactivityattendace.UserIDF);
                SqlParameter paramContactIDF = new SqlParameter("@ContactIDF", modelactivityattendace.ContactIDF);
                SqlParameter paramPresentStatus = new SqlParameter("@PresentStatus", modelactivityattendace.PresentStatus);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy",  ObjUser.UserID );

                var paramSqlQuery = "EXECUTE dbo.uspAttendance_Insert @AttendanceIDP OUTPUT, @PartnerIDF, @ActivityIDF, @UserIDF, @ContactIDF, @PresentStatus,@EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramAttendanceIDP, paramPartnerIDF, paramActivityIDF, paramUserIDF, paramContactIDF, paramPresentStatus, paramEntryBy);

                return Convert.ToInt64(paramAttendanceIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspAttendance_Insert", 1);
                return 0;
            }
        }
        #endregion update  Activity attendance


        #region Feed INSERT
        public async Task<Int64> Feed_Insert(ModelFeed modelFeed, string connection)
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
                    AddInParameter(cmd, "@PartnerIDF", SqlDbType.BigInt, ObjUser.UserID);
                    AddInParameter(cmd, "@UserIDF", SqlDbType.BigInt, 0);
                    AddInParameter(cmd, "@Banner", SqlDbType.NVarChar, modelFeed.BannerAttachment);
                    AddInParameter(cmd, "@Title", SqlDbType.NVarChar, modelFeed.Title);
                    AddInParameter(cmd, "@ActivityIDF", SqlDbType.BigInt, modelFeed.ActivityIDF);

                    SqlParameter sqlParamData = cmd.Parameters.AddWithValue("@actData", dtImport);
                    AddInParameter(cmd, "EntryBy", SqlDbType.Int, (Int32)ObjUser.UserID);
                    sqlParamData.SqlDbType = SqlDbType.Structured;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    feedIDP= Convert.ToInt64(paramFeedIDP.Value);
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
        #endregion Feed INSERT


        #region Feed Update
        public async Task<Int64> Feed_Update(ModelFeed modelFeed)
        {
            
            try
            {
                SqlParameter paramFeedIDP = new SqlParameter("@FeedIDP", (object)modelFeed.FeedIDP ?? DBNull.Value);
                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
                SqlParameter paramBanner = new SqlParameter("@Banner", (object) modelFeed.BannerAttachment ?? DBNull.Value);
                SqlParameter paramTitle = new SqlParameter("@Title", (object)modelFeed.Title ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspFeed_Update @FeedIDP, @PartnerIDF, @Banner, @Title,@EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramFeedIDP, paramPartnerIDF,  paramBanner, paramTitle, paramEntryBy);

                return Convert.ToInt64(paramFeedIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFeed_Update", 1);
                return 0;
            }
        }
        #endregion Feed Update

        #region Get Activity Participate List
        public async Task<string> Partner_Activity_Participate_List(SearchParticipateFeed param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_Get_ParticipateList";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityID", SqlDbType = SqlDbType.BigInt, Value = param.ActivityID });
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
                await ErrorLog(1, e.Message, $"uspActivity_Get_ParticipateList", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Get Activity Participate List

        #region Activity DDL
        public async Task<string> Partner_Activity_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_DDL_ByPartner";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });

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
                await ErrorLog(1, e.Message, $"uspActivity_DDL_ByPartner", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Activity DDL


        #region GET all Feed 
        public async Task<string> Partner_Feed_Get_All(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspFeed_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = 0 });
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
        public async Task<Int64> Partner_Update_Feed_Comment(ModelFeedComment modelafeedcomment)
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
                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", modelafeedcomment.UserIDF);
                SqlParameter paramComment = new SqlParameter("@Comment", modelafeedcomment.Comment);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspFeed_Update_Comment @FeedParticipantIDP OUTPUT, @FeedIDF, @PartnerIDF, @UserIDF, @Comment, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramFeedParticipantIDP, paramFeedIDF, paramPartnerIDF,  paramUserIDF, paramComment, paramEntryBy);

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
        public async Task<string> Partner_Feed_Get_All_Comment(ModelFeedCommentAll param)
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
        public async Task<Int64> Feed_Delete(Int64 feedIDP)
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
        public async Task<Int64> Feed_Update_Reaction(ModelFeedUpdateReaction modelfeed)
        {
            try
            {
               
                SqlParameter paramFeedIDF = new SqlParameter("@FeedIDF", modelfeed.FeedIDF);
                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", modelfeed.UserIDF);
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
        public async Task<string> Partner_Feed_Get(Int64 FeedIDP)
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

        #region  GET Club subscription
        public async Task<string> Partner_Get_Club_Subscription_Status(Int64 clubIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspClubSubscription_Get_SubscriptionStatus_By_ClubIDF";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ClubIDF", SqlDbType = SqlDbType.BigInt, Value = clubIDP });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = 0 });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });

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
