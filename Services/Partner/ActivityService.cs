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
using System.Globalization;
using BMS_API.Models.User;
using BMS_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using BMS_API.Models.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace BMS_API.Services.Partner
{
    public class ActivityService : CommonService, IActivityService
    {
        public ActivityService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region Activity INSERT
        //public async Task<Int64> Activity_Insert(Activity modelActivity)
        //{
        //    try
        //    {
        //        // Null checks
        //        if (modelActivity == null)
        //            throw new ArgumentNullException(nameof(modelActivity), "modelActivity is null.");

        //        if (ObjUser == null || ObjUser.UserID == null)
        //            throw new Exception("ObjUser or ObjUser.UserID is null.");

        //        SqlParameter paramActivityIDP = new SqlParameter
        //        {
        //            ParameterName = "@ActivityIDP",
        //            SqlDbType = System.Data.SqlDbType.BigInt,
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
        //        SqlParameter paramInterestIDF = new SqlParameter("@InterestIDF", (object)modelActivity.InterestIDF ?? DBNull.Value);
        //        SqlParameter paramBanner = new SqlParameter("@Banner", (object)modelActivity.BannerAttachment ?? DBNull.Value);
        //        SqlParameter paramActivityTitle = new SqlParameter("@ActivityTitle", (object)modelActivity.ActivityTitle ?? DBNull.Value);
        //        SqlParameter paramActivityAbout = new SqlParameter("@ActivityAbout", (object)modelActivity.ActivityAbout ?? DBNull.Value);
        //        SqlParameter paramVenue = new SqlParameter("@Venue", (object)modelActivity.Venue ?? DBNull.Value);
        //        SqlParameter paramLongitude = new SqlParameter("@Longitude", (object)modelActivity.Longitude ?? DBNull.Value);
        //        SqlParameter paramLatitude = new SqlParameter("@Latitude", (object)modelActivity.Latitude ?? DBNull.Value);
        //        SqlParameter paramGeoLocation = new SqlParameter("@GeoLocation", (object)modelActivity.GeoLocation ?? DBNull.Value);
        //        SqlParameter paramStartDateTime = new SqlParameter("@StartDateTime", DateTime.ParseExact(modelActivity.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
        //        SqlParameter paramEndDateTime = new SqlParameter("@EndDateTime", DateTime.ParseExact(modelActivity.EndDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));

        //        SqlParameter paramStartTimeActual = new SqlParameter("@StartDateTimeActual", (object)(modelActivity.StartTimeActual.HasValue ? modelActivity.StartTimeActual.Value : DBNull.Value))
        //        {
        //            SqlDbType = System.Data.SqlDbType.Time
        //        };

        //        SqlParameter paramEndTimeActual = new SqlParameter("@EndTimeActual", (object)(modelActivity.EndTimeActual.HasValue ? modelActivity.EndTimeActual.Value : DBNull.Value))
        //        {
        //            SqlDbType = System.Data.SqlDbType.Time
        //        };

        //        SqlParameter paramTotalSeats = new SqlParameter("@TotalSeats", (object)modelActivity.TotalSeats ?? DBNull.Value);
        //        SqlParameter paramAllocatedSeats = new SqlParameter("@AllocatedSeats", (object)modelActivity.AllocatedSeats ?? DBNull.Value);
        //        SqlParameter paramPrice = new SqlParameter("@Price", (object)modelActivity.Price ?? DBNull.Value);
        //        SqlParameter paramWebinarLink = new SqlParameter("@WebinarLink", (object)modelActivity.WebinarLink ?? DBNull.Value);
        //        SqlParameter paramCouponIDF = new SqlParameter("@CouponIDF", (object)modelActivity.CouponIDF ?? DBNull.Value);
        //        SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

        //        var paramSqlQuery = "EXECUTE dbo.uspActivity_InsertNew @ActivityIDP OUTPUT, @PartnerIDF, @InterestIDF, @Banner, @ActivityTitle, @ActivityAbout, @Venue, @Longitude, @Latitude, @GeoLocation, @StartDateTime, @EndDateTime, @StartDateTimeActual, @EndTimeActual, @TotalSeats, @AllocatedSeats, @Price, @WebinarLink, @CouponIDF, @EntryBy";

        //        await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramActivityIDP, paramPartnerIDF, paramInterestIDF, paramBanner, paramActivityTitle, paramActivityAbout, paramVenue, paramLongitude, paramLatitude, paramGeoLocation, paramStartDateTime, paramEndDateTime, paramStartTimeActual, paramEndTimeActual, paramTotalSeats, paramAllocatedSeats, paramPrice, paramWebinarLink, paramCouponIDF, paramEntryBy);

        //        // Check if duplicate was found
        //        if (Convert.ToInt64(paramActivityIDP.Value) == -1)
        //        {
        //            throw new Exception("Duplicate activity detected.");
        //        }

        //        return Convert.ToInt64(paramActivityIDP.Value);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"uspActivity_InsertNew", 1);
        //        return 0;
        //    }
        //}
        public async Task<Int64> Activity_Insert(Activity modelActivity)
        {
            try
            {
                // Null checks
                if (modelActivity == null)
                    throw new ArgumentNullException(nameof(modelActivity), "modelActivity is null.");

                if (ObjUser == null || ObjUser.UserID == null)
                    throw new Exception("ObjUser or ObjUser.UserID is null.");

                // Validation for missing fields
                var missingFields = new List<string>();
                //if (string.IsNullOrWhiteSpace(modelActivity.BannerAttachment)) missingFields.Add("BannerAttachment");
                if (string.IsNullOrWhiteSpace(modelActivity.ActivityTitle)) missingFields.Add("ActivityTitle");
                if (string.IsNullOrWhiteSpace(modelActivity.ActivityAbout)) missingFields.Add("ActivityAbout");
                if (string.IsNullOrWhiteSpace(modelActivity.Venue)) missingFields.Add("Venue");
                if (string.IsNullOrWhiteSpace(modelActivity.StartDateTime)) missingFields.Add("StartDateTime");
                if (string.IsNullOrWhiteSpace(modelActivity.EndDateTime)) missingFields.Add("EndDateTime");
                if (modelActivity.TotalSeats == null) missingFields.Add("TotalSeats");
                if (modelActivity.Price == null) missingFields.Add("Price");
                if (string.IsNullOrWhiteSpace(modelActivity.WebinarLink)) missingFields.Add("WebinarLink");
                if (!modelActivity.StartTimeActual.HasValue) missingFields.Add("StartTimeActual");
                if (!modelActivity.EndTimeActual.HasValue) missingFields.Add("EndTimeActual");
                if (string.IsNullOrWhiteSpace(modelActivity.ActivityInterestName)) missingFields.Add("ActivityInterestName");

                if (missingFields.Any())
                {
                    throw new Exception($"{string.Join(", ", missingFields)} field(s) missing, please input these fields.");
                }

                SqlParameter paramActivityIDP = new SqlParameter
                {
                    ParameterName = "@ActivityIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
                SqlParameter paramInterestIDF = new SqlParameter("@InterestIDF", (object)modelActivity.InterestIDF ?? DBNull.Value);
                SqlParameter paramBanner = new SqlParameter("@Banner", (object)modelActivity.BannerAttachment ?? DBNull.Value);
                SqlParameter paramActivityTitle = new SqlParameter("@ActivityTitle", (object)modelActivity.ActivityTitle ?? DBNull.Value);
                SqlParameter paramActivityAbout = new SqlParameter("@ActivityAbout", (object)modelActivity.ActivityAbout ?? DBNull.Value);
                SqlParameter paramVenue = new SqlParameter("@Venue", (object)modelActivity.Venue ?? DBNull.Value);
                SqlParameter paramLongitude = new SqlParameter("@Longitude", (object)modelActivity.Longitude ?? DBNull.Value);
                SqlParameter paramLatitude = new SqlParameter("@Latitude", (object)modelActivity.Latitude ?? DBNull.Value);
                SqlParameter paramGeoLocation = new SqlParameter("@GeoLocation", (object)modelActivity.GeoLocation ?? DBNull.Value);

                // Parse DateTime from string
                SqlParameter paramStartDateTime = new SqlParameter("@StartDateTime", DateTime.ParseExact(modelActivity.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));
                SqlParameter paramEndDateTime = new SqlParameter("@EndDateTime", DateTime.ParseExact(modelActivity.EndDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture));

                SqlParameter paramStartTimeActual = new SqlParameter("@StartDateTimeActual", (object)(modelActivity.StartTimeActual.HasValue ? modelActivity.StartTimeActual.Value : DBNull.Value))
                {
                    SqlDbType = System.Data.SqlDbType.Time
                };

                SqlParameter paramEndTimeActual = new SqlParameter("@EndTimeActual", (object)(modelActivity.EndTimeActual.HasValue ? modelActivity.EndTimeActual.Value : DBNull.Value))
                {
                    SqlDbType = System.Data.SqlDbType.Time
                };

                SqlParameter paramTotalSeats = new SqlParameter("@TotalSeats", (object)modelActivity.TotalSeats ?? DBNull.Value);
                SqlParameter paramAllocatedSeats = new SqlParameter("@AllocatedSeats", (object)modelActivity.AllocatedSeats ?? DBNull.Value);
                SqlParameter paramPrice = new SqlParameter("@Price", (object)modelActivity.Price ?? DBNull.Value);
                SqlParameter paramWebinarLink = new SqlParameter("@WebinarLink", (object)modelActivity.WebinarLink ?? DBNull.Value);
                SqlParameter paramCouponIDF = new SqlParameter("@CouponIDF", (object)modelActivity.CouponIDF ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspActivity_InsertNew @ActivityIDP OUTPUT, @PartnerIDF, @InterestIDF, @Banner, @ActivityTitle, @ActivityAbout, @Venue, @Longitude, @Latitude, @GeoLocation, @StartDateTime, @EndDateTime, @StartDateTimeActual, @EndTimeActual, @TotalSeats, @AllocatedSeats, @Price, @WebinarLink, @CouponIDF, @EntryBy";

                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramActivityIDP, paramPartnerIDF, paramInterestIDF, paramBanner, paramActivityTitle, paramActivityAbout, paramVenue, paramLongitude, paramLatitude, paramGeoLocation, paramStartDateTime, paramEndDateTime, paramStartTimeActual, paramEndTimeActual, paramTotalSeats, paramAllocatedSeats, paramPrice, paramWebinarLink, paramCouponIDF, paramEntryBy);

                // Check if duplicate was found
                if (Convert.ToInt64(paramActivityIDP.Value) == -1)
                {
                    throw new Exception("Duplicate activity detected.");
                }

                return Convert.ToInt64(paramActivityIDP.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_InsertNew", 1);
                return 0;
            }
        }



        #endregion Activity INSERT


        #region Activity UPDATE
        public async Task<Int64> Activity_Update(Activity modelActivity)
        {
            try
            {

                SqlParameter paramActivityIDP = new SqlParameter("@ActivityIDP", (object)modelActivity.ActivityIDP ?? DBNull.Value);
                    SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", ObjUser.UserID);
                SqlParameter paramInterestIDF = new SqlParameter("@InterestIDF", (object)modelActivity.InterestIDF ?? DBNull.Value);
                SqlParameter paramBanner = new SqlParameter("@Banner", (object)modelActivity.BannerAttachment ?? DBNull.Value);
                SqlParameter paramActivityTitle = new SqlParameter("@ActivityTitle", (object)modelActivity.ActivityTitle ?? DBNull.Value);
                SqlParameter paramActivityAbout = new SqlParameter("@ActivityAbout", (object)modelActivity.ActivityAbout ?? DBNull.Value);
                SqlParameter paramVenue = new SqlParameter("@Venue", (object)modelActivity.Venue ?? DBNull.Value);
                SqlParameter paramLongitude = new SqlParameter("@Longitude", (object)modelActivity.Longitude ?? DBNull.Value);
                SqlParameter paramLatitude = new SqlParameter("@Latitude", (object)modelActivity.Latitude ?? DBNull.Value);
                SqlParameter paramGeoLocation = new SqlParameter("@GeoLocation", (object)modelActivity.GeoLocation ?? DBNull.Value);
                SqlParameter paramStartDateTime = new SqlParameter("@StartDateTime", Convert.ToDateTime(DateTime.ParseExact(modelActivity.StartDateTime, "dd/MM/yyyy H/m", CultureInfo.InvariantCulture)));
                SqlParameter paramEndDateTime = new SqlParameter("@EndDateTime", Convert.ToDateTime(DateTime.ParseExact(modelActivity.EndDateTime, "dd/MM/yyyy H/m", CultureInfo.InvariantCulture)));
                SqlParameter paramTotalSeats = new SqlParameter("@TotalSeats", (object)modelActivity.TotalSeats ?? DBNull.Value);
                SqlParameter paramAllocatedSeats = new SqlParameter("@AllocatedSeats", (object)modelActivity.AllocatedSeats ?? DBNull.Value);
                SqlParameter paramPrice = new SqlParameter("@Price", (object)modelActivity.Price ?? DBNull.Value);
                //SqlParameter paramWebinarLink = new SqlParameter("@WebinarLink", modelActivity.WebinarLink != null ? modelActivity.WebinarLink: DBNull.Value);

                SqlParameter paramWebinarLink = new SqlParameter() { ParameterName = "@WebinarLink", SqlDbType = SqlDbType.NVarChar, Value = modelActivity.WebinarLink != null ? modelActivity.WebinarLink : "" };

                SqlParameter paramCouponIDF = new SqlParameter("@CouponIDF", (object)modelActivity.CouponIDF ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspActivity_Update @ActivityIDP, @PartnerIDF, @InterestIDF, @Banner, @ActivityTitle, @ActivityAbout, @Venue, @Longitude, @Latitude, @GeoLocation, @StartDateTime, @EndDateTime, @TotalSeats, @AllocatedSeats, @Price,@WebinarLink, @CouponIDF, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramActivityIDP, paramPartnerIDF,paramInterestIDF, paramBanner, paramActivityTitle, paramActivityAbout, paramVenue, paramLongitude, paramLatitude, paramGeoLocation, paramStartDateTime, paramEndDateTime, paramTotalSeats, paramAllocatedSeats, paramPrice, paramWebinarLink, paramCouponIDF, paramEntryBy);

                return Convert.ToInt64(paramActivityIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_Update", 1);
                return 0;
            }
        }
        #endregion Activity UPDATE


        #region Activity GET
        public async Task<string> Activity_Get(Int64 activityIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityIDP", SqlDbType = SqlDbType.BigInt, Value = activityIDP });

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
                await ErrorLog(1, e.Message, $"uspActivity_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Activity GET

        #region Activity Request  GET Full Detail
        public async Task<string> Activity_Request_Get_FullDetail(Int64 activityIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivityRequest_Get_FullDetail";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityIDP", SqlDbType = SqlDbType.BigInt, Value = activityIDP });

                   
                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "detail";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivityRequest_Get_FullDetail", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Activity GET

        #region Activity GET_ALL
        //public async Task<string> Activity_GetAll(Int64 actvityStatus)
        //{
        //    try
        //    {
        //        string strResponse = "";
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "uspActivity_GetAll";
        //            command.CommandType = System.Data.CommandType.StoredProcedure;
        //            command.Parameters.Add(new SqlParameter() { ParameterName = "@Status", SqlDbType = SqlDbType.Int, Value = actvityStatus });

        //            _context.Database.OpenConnection();
        //            DbDataReader ddr = command.ExecuteReader();
        //            DataSet ds = new DataSet();
        //            while (!ddr.IsClosed)
        //                ds.Tables.Add().Load(ddr);
        //            ds.Tables[0].TableName = "data";
        //            strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
        //        }
        //        return strResponse;

        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"uspActivity_GetAll", 1);
        //        return "Error, Something wrong!";
        //    }
        //}
        #endregion Activity GET_ALL

        #region Activity GET_ALL By user
        public async Task<string> Activity_GetAll_ByUser(SearchWithDisplayTypeWithInterest param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_GetAll_ByUser";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PartnerIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@DisplayTypeID", SqlDbType = SqlDbType.Int, Value = param.DisplayTypeID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@InterestID", SqlDbType = SqlDbType.Int, Value = param.InterestID });
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
                await ErrorLog(1, e.Message, $"uspActivity_GetAll_ByUser", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Activity GET_ALL By user


        #region Activity Approve DisApproved
        public async Task<Int64> Activity_ApproveReject(Int64 activityIDP, Int32 isApprove)
        {
            try
            {
                SqlParameter paramactivityidp = new SqlParameter("@ActivityIDP", activityIDP);
                SqlParameter paramisactive = new SqlParameter("@Status", isApprove);
                SqlParameter paramentryby = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramsqlquery = "execute dbo.uspActivity_Update_ApproveDisapprove @ActivityIDP, @Status, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramsqlquery, paramactivityidp, paramisactive, paramentryby);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_Update_ApproveDisapprove", 1);
                return 0;
            }
        }
        #endregion Activity Approve DisApproved

        #region Activity Booked
        public async Task<Int64> Activity_Booked(ActivityBooked modelactivitybook)
        {
            try
            {
                SqlParameter paramAvtivityBookedIDP = new SqlParameter
                {
                    ParameterName = "@AvtivityBookedIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", modelactivitybook.PartnerIDF);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramActivityIDF = new SqlParameter("@ActivityIDF", modelactivitybook.ActivityIDF);
                SqlParameter paramBookingStatus = new SqlParameter("@BookingStatus", modelactivitybook.BookingStatus);
                SqlParameter paramContactIDPs = new SqlParameter("@ContactIDPs", (object)modelactivitybook.ContactIDPs ?? DBNull.Value );
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspActivityBooked_Insert @AvtivityBookedIDP OUTPUT, @PartnerIDF, @UserIDF, @ActivityIDF, @BookingStatus, @ContactIDPs,@EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramAvtivityBookedIDP, paramPartnerIDF, paramUserIDF, paramActivityIDF, paramBookingStatus, paramContactIDPs, paramEntryBy);

                return Convert.ToInt64(paramAvtivityBookedIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivityBooked_Insert", 1);
                return 0;
            }
        }
        #endregion Activity Booked

        #region Activity booked cancelled
        public async Task<Int64> Activity_Booked_Cancelled(ActivityBookedCancelled modelactivitybookcancelled)
        {
            try
            {
                SqlParameter paramAvtivityBookedIDP = new SqlParameter("@AvtivityBookedIDP", modelactivitybookcancelled.AvtivityBookedIDP);
                SqlParameter paramActivityIDF = new SqlParameter("@ActivityIDF", modelactivitybookcancelled.ActivityIDF);
                SqlParameter paramBookingStatus = new SqlParameter("@BookingStatus", modelactivitybookcancelled.BookingStatus);
                SqlParameter paramentryby = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramsqlquery = "execute dbo.uspActivityBooked_Cancelled @AvtivityBookedIDP, @ActivityIDF,@BookingStatus, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramsqlquery, paramAvtivityBookedIDP, paramActivityIDF, paramBookingStatus, paramentryby);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivityBooked_Cancelled", 1);
                return 0;
            }
        }
        #endregion Activity booked cancelled


        #region Activity GET_ALL By user id
        public async Task<string> Activity_GetAll_ByUserIDF(Search modelsearch)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_GetAll_ByUserIDF";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = modelsearch.SearchKeyWord });

                    _context.Database.OpenConnection();
                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "interestData";
                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_GetAll_ByUser", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Activity GET_ALL By user id
        #region GET-ALL By User IDF of Suggested Offer
        public async Task<string> Suggested_Offer_Activity_GetAll_ByUserIDF(Search modelsearch)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_SuggestedOffer_GetAll_ByUserIDF";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = modelsearch.SearchKeyWord });

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
                await ErrorLog(1, e.Message, $"uspActivity_SuggestedOffer_GetAll_ByUserIDF", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL By User IDF of Suggested Offer

        #region Activity DDL
        //public async Task<string> Activity_DDL()
        //{
        //    try
        //    {
        //        string strResponse = "";
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "uspActivity_DDL";
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
        //        await ErrorLog(1, e.Message, $"uspActivity_DDL", 1);
        //        return "";
        //    }
        //}
        #endregion Activity DDL


        #region Activity GET_ALL By Search
        public async Task<string> Activity_GetAll_BySearch(ActivitySearch activitySearch)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_GetAll_Search_ByUserIDF";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@InterestIDPs", SqlDbType = SqlDbType.NVarChar, Value = activitySearch.InterestIDPs!=null? activitySearch.InterestIDPs : "" });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@LocationMeter", SqlDbType = SqlDbType.Int, Value = activitySearch.LocationMeter != null ? activitySearch.LocationMeter : DBNull.Value});
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@StartPrice", SqlDbType = SqlDbType.Int, Value = activitySearch.StartPrice !=null ? activitySearch.StartPrice : DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@EndPrice", SqlDbType = SqlDbType.Int, Value = activitySearch.EndPrice!=null?activitySearch.EndPrice: DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PeopleCount", SqlDbType = SqlDbType.Int, Value = activitySearch.PeopleCount!=null?activitySearch.PeopleCount: DBNull.Value });
                  //  command.Parameters.Add(new SqlParameter() { ParameterName = "@StartTime", SqlDbType = SqlDbType.DateTime, Value = activitySearch.StartTime });
                   // command.Parameters.Add(new SqlParameter() { ParameterName = "@EndTime", SqlDbType = SqlDbType.DateTime, Value = activitySearch.EndTime });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Rating", SqlDbType = SqlDbType.Float, Value = activitySearch.Rating!=null? activitySearch.Rating: DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Latitude", SqlDbType = SqlDbType.Decimal, Value = activitySearch.Latitude != null ? activitySearch.Latitude : DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Longitude", SqlDbType = SqlDbType.Decimal, Value = activitySearch.Longitude != null ? activitySearch.Longitude : DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@TimeSlot", SqlDbType = SqlDbType.Int, Value = activitySearch.TimeSlot!=null?activitySearch.TimeSlot: DBNull.Value });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = activitySearch.SearchKeyWord!=null? activitySearch.SearchKeyWord:"" });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@IsMyActivity", SqlDbType = SqlDbType.Int, Value = activitySearch.IsMyActivity });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@DisplayTypeID", SqlDbType = SqlDbType.Int, Value = activitySearch.DisplayTypeID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@InterestID", SqlDbType = SqlDbType.Int, Value = activitySearch.InterestID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityID", SqlDbType = SqlDbType.BigInt, Value = activitySearch.ActivityID });


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
                await ErrorLog(1, e.Message, $"uspActivity_GetAll_Search_ByUserIDF", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Activity GET_ALL By Search

        #region GET-ALL_ By For Vendor Coupon
        public async Task<string> Activity_Coupon_GetAll_Vendor(ActivityCoupon param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    
                    command.CommandText = "uspmstCoupon_GetAll_For_Vendor";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityEndDate", SqlDbType = SqlDbType.DateTime, Value = param.ActivityEndDate });


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
                await ErrorLog(1, e.Message, $"uspmstCoupon_GetAll_For_Vendor", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL_ By For Vendor Coupon

        #region Activity DELETE
        public async Task<Int64> Activity_Delete(Int64 activityIDP)
        {
            try
            {
                SqlParameter paramActivityIDP = new SqlParameter("@ActivityIDP", activityIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspActivity_Delete @ActivityIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramActivityIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_Delete", 1);
                return 0;
            }
        }
        #endregion Activity DELETE

        #region Update Favourite Activity
        public async Task<Int64> Activity_Update_Favourite_ByUser(Int64 activityIDP)
        {
            try
            {
                SqlParameter paramActivityIDP = new SqlParameter("@ActivityIDP", activityIDP);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspActivity_Update_Favourite_ByUser @ActivityIDP, @UserIDF, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramActivityIDP, paramUserIDF, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspActivity_Update_Favourite_ByUser", 1);
                return 0;
            }
        }
        #endregion Update Favourite Activity


        #region GET-ALL Favourite Activity 
        public async Task<string> Activity_GetAll_Favourite(Search modelsearch)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_Favourite_GetAll_ByUser";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDF", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SearchKeyWord", SqlDbType = SqlDbType.NVarChar, Value = modelsearch.SearchKeyWord });

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
                await ErrorLog(1, e.Message, $"uspActivity_Favourite_GetAll_ByUser", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL Favourite Activity 

        #region Update Follower
        public async Task<Int64> Activity_Update_Follower(ModelFollowerUpdate modelFollowerUpdate)
        {
            try
            {
                SqlParameter paramFollowIDP = new SqlParameter("@FollowIDP", (object)modelFollowerUpdate.FollowIDP ?? DBNull.Value);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramFollowUserIDF = new SqlParameter("@FollowUserIDF", (object)modelFollowerUpdate.FollowUserIDF ?? DBNull.Value);
                SqlParameter paramFollowContactIDF = new SqlParameter("@FollowContactIDF", (object)modelFollowerUpdate.FollowContactIDF ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspFollower_Update_Follower @FollowIDP, @UserIDF, @FollowUserIDF, @FollowContactIDF, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramFollowIDP, paramUserIDF, paramFollowUserIDF, paramFollowContactIDF, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspFollower_Update_Follower", 1);
                return 0;
            }
        }
        #endregion Update Follower

        #region GET-ALL Follower
        public async Task<string> Activity_GetAll_Follower()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspFollower_GetAll";
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
                await ErrorLog(1, e.Message, $"uspFollower_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET-ALL Follower

        #region getActivityById
        public async Task<ActivityListDTO> GetActivityById(long activityId)
        {
            ActivityListDTO activityList = null;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT * FROM tblActivity WHERE ActivityIDP = @ActivityId";
                command.CommandType = System.Data.CommandType.Text;

                var activityIdParam = new SqlParameter("@ActivityId", System.Data.SqlDbType.BigInt)
                {
                    Value = activityId
                };
                command.Parameters.Add(activityIdParam);

                _context.Database.OpenConnection();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows && await reader.ReadAsync())
                    {
                        string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";
                        string bannerFilename = reader["Banner"]?.ToString();

                        activityList = new ActivityListDTO
                        {
                            
                            BannerAttachmentPath = !string.IsNullOrEmpty(bannerFilename) ? baseBannerUrl + bannerFilename : null,
                            ActivityName = reader["ActivityTitle"]?.ToString(),
                            ActivityAbout = reader["ActivityAbout"]?.ToString(),
                            Venue = reader["Venue"]?.ToString(),
                            //Longitude = reader["Longitude"] as decimal?,
                            //Latitude = reader["Latitude"] as decimal?,
                            //GeoLocation = reader["GeoLocation"]?.ToString(),
                            StartDate = reader["StartDateTime"] != DBNull.Value ? ((DateTime)reader["StartDateTime"]).ToString("MM/dd/yy") : null,
                            EndDate = reader["EndDateTime"] != DBNull.Value ? ((DateTime)reader["EndDateTime"]).ToString("MM/dd/yy") : null,
                            TotalSeats = reader["TotalSeats"] as int?,
                            //AllocatedSeats = reader["AllocatedSeats"] as int?,
                            Price = (decimal?)(reader["Price"] != DBNull.Value ? Convert.ToSingle(reader["Price"]) : (float?)null), // Updated line
                            WebinarLink = reader["WebinarLink"]?.ToString(),
                            StartTime = reader["StartDateTimeActual"] != DBNull.Value ? (TimeSpan?)reader["StartDateTimeActual"] : null,
                            EndTime = reader["EndDateTimeActual"] != DBNull.Value ? (TimeSpan?)reader["EndDateTimeActual"] : null,
                            SkillId =  reader["ActivityIDP"] != DBNull.Value ? (long)reader["ActivityIDP"] : 0, // Set the ActivityId
                        };
                    }
                }
            }

            return activityList;
        }

        public async Task<object> Activity_GetAll(int activityStatus, int per_page, int page)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspActivity_GetAll";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Adding parameters for the stored procedure
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Status", SqlDbType = SqlDbType.Int, Value = activityStatus });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Page", SqlDbType = SqlDbType.Int, Value = page });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PerPage", SqlDbType = SqlDbType.Int, Value = per_page });


                    //adding output parameter to get the total records
                    var totalRecordsParam = new SqlParameter() { ParameterName = "@TotalRecords", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
                    command.Parameters.Add(totalRecordsParam);

                    // Open the database connection
                    await _context.Database.OpenConnectionAsync();

                    // Execute the command and load the data into a DataTable
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    // Load data from the reader into a DataTable
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    // Convert the DataTable into a list of dictionaries
                    var dataList = new List<Dictionary<string, object>>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var dataRow = new Dictionary<string, object>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            dataRow[column.ColumnName] = row[column];
                        }
                        dataList.Add(dataRow);
                    }

                    //get total records from output parameter
                    int totalRecords = (int)totalRecordsParam.Value;
                    bool isMore = (page * per_page) < totalRecords;

                    // Return a structured response
                    return new
                        {
                            status = 200,
                            message = "Data fetched successfully",
                            data = dataList,
                            totalRecords,
                            isMore,
                            currentPage = page
                        };
                    
                }
            }
            catch (Exception e)
            {
                // Log the error
                await ErrorLog(1, e.Message, "uspActivity_GetAll", 1);
                return new
                {
                    status = 500,
                    message = "Error, something went wrong!",
                    error = e.Message
                };
            }
        }


        #endregion
    }
}