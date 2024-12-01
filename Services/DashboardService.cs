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
using Newtonsoft.Json;
using System.Collections.Generic;
using BMS_API.Models.DTOs;
using System.Linq.Expressions;
using System.Linq;
using System.Diagnostics;

namespace BMS_API.Services
{
    public class DashboardService : CommonService, IDashboardService
    {
        public DashboardService(BMSContext context) : base(context)
        {
            _context= context;
        }

        public AuthorisedUser ObjUser { get; set; }


        #region Dashboard stats
        public async Task<string> Dashboard_Detail()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspDashbord_Summary";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    _context.Database.OpenConnection();


                    DbDataReader ddr = command.ExecuteReader();
                    DataSet ds = new DataSet();
                    while (!ddr.IsClosed)
                        ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "data";
                    ds.Tables[1].TableName = "partner";

                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                return strResponse;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspDashbord_Summary", 1);
                return null;
            }
        }
        #endregion Dashboard stats

        #region Upcoming-Activities
        public async Task<string> GetUpcomingActivities()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetUpcoming_Activities";
                    command.CommandType = CommandType.StoredProcedure;

                    _context.Database.OpenConnection();

                    using (DbDataReader ddr = command.ExecuteReader())
                    {
                        DataSet ds = new DataSet();

                        while (!ddr.IsClosed)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(ddr);
                            ds.Tables.Add(dt);
                        }

                        //Assign names to DataTables as needed
                        if(ds.Tables.Count > 0)
                            ds.Tables[0].TableName = "UpComingActivties";

                        //Convert DataSet to JSON stgring

                        strResponse = JsonConvert.SerializeObject(ds);
                    }
                    _context.Database.CloseConnection();
                }
                return strResponse;
            }
            catch(Exception e)
            {
                await ErrorLog(1, e.Message, $"uspGetUpcoming_Activities", 1);
                return null;
            }
        }
        #endregion Upcoming-Activities

        #region Skills-Details
        public async Task<object> SkillsDetailsAsync(Int64 UserId)
        {
            try
            {
                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";

                List<SkillDetails> trendingSkills = new List<SkillDetails>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "[SkillDetail]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@UserId",
                        SqlDbType = SqlDbType.BigInt,
                        Value = UserId
                    });

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var trendingSkill = new SkillDetails
                            {
                                LikeStatus = (int)(reader["LikeStatus"] != DBNull.Value ? Convert.ToInt32(reader["LikeStatus"]) : (int?)0),
                                ProfileImage = reader["ProfileImage"] != DBNull.Value ? baseBannerUrl + reader["ProfileImage"].ToString() : string.Empty,
                                SkillName = reader["SkillName"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Description = reader["Description"].ToString(),
                                Location = reader["Location"].ToString(),
                                Duration = reader["Duration"].ToString(),
                                RemainingSeats = reader["RemainingSeats"] != DBNull.Value ? Convert.ToInt32(reader["RemainingSeats"]) : (int?)null,
                                //GalleryImage = ColumnExists(reader, "GalleryImage") ? reader["GalleryImage"]?.ToString() : null,
  
                                Rating = (int?)(ColumnExists(reader, "Rating") ? (reader["Rating"] != DBNull.Value ? Convert.ToDouble(reader["Rating"]) : (double?)null) : null),
                                Price = ColumnExists(reader, "Price") ? (reader["Price"] != DBNull.Value ? Convert.ToInt32(reader["Price"]) : (int?)null) : null,
                            };

                            trendingSkills.Add(trendingSkill);
                        }
                    }

                    var response = new
                    {
                        status = 200,
                        data = trendingSkills
                    };

                    return response;
                }
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"SkillDetail", 1);
                return "Error, something went wrong!";
            }
        }

        //private bool ColumnExists(DbDataReader reader, string columnName)
        //{
        //    try
        //    {
        //        return reader.GetOrdinal(columnName) >= 0;
        //    }
        //    catch (IndexOutOfRangeException)
        //    {
        //        return false;
        //    }
        //}

        #endregion Trending-Skills

        #region Trending-Skills
        public async Task<object> GetAllTrendingSkillsAsync(Int64 activityStatus)
        {
            try
            {
                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";

                List<TrendingSkillsDTO> trendingSkills = new List<TrendingSkillsDTO>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "[uspTrendingSkillsNew]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Status",
                        SqlDbType = SqlDbType.Int,
                        Value = activityStatus
                    });

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var trendingSkill = new TrendingSkillsDTO
                            {
                                SkillId = (int)(reader["ActivityIDP"] != DBNull.Value ? Convert.ToInt32(reader["ActivityIDP"]) : (int?)null),
                                SkillImage = reader["SkillImage"] != DBNull.Value ? baseBannerUrl + reader["SkillImage"].ToString() : string.Empty,
                                SkillName = reader["SkillName"].ToString(),
                                StartDateTime = reader["StartDateTime"].ToString(),
                                Description = reader["Description"].ToString(),
                                AllocatedSeats = reader["AllocatedSeats"] != DBNull.Value ? Convert.ToInt32(reader["AllocatedSeats"]) : (int?)null,
                                TotalSeats = reader["TotalSeats"] != DBNull.Value ? Convert.ToInt32(reader["TotalSeats"]) : (int?)null,
                                PricePerSession = reader["PricePerSession"] != DBNull.Value ? Convert.ToDouble(reader["PricePerSession"]) : (double?)null,
                                RemainingSeats = reader["RemainingSeats"] != DBNull.Value ? Convert.ToInt32(reader["RemainingSeats"]) : (int?)null,
                                //GalleryImage = ColumnExists(reader, "GalleryImage") ? reader["GalleryImage"]?.ToString() : null,
                                ReviewText = ColumnExists(reader, "ReviewText") ? reader["ReviewText"]?.ToString() : null,
                                Rating = (int?)(ColumnExists(reader, "Rating") ? (reader["Rating"] != DBNull.Value ? Convert.ToDouble(reader["Rating"]) : (double?)null) : null),
                                PostDescription = ColumnExists(reader, "PostDescription") ? reader["PostDescription"]?.ToString() : null,
                                Likes = ColumnExists(reader, "Likes") ? (reader["Likes"] != DBNull.Value ? Convert.ToInt32(reader["Likes"]) : (int?)null) : null,
                                Dislikes = ColumnExists(reader, "Dislikes") ? (reader["Dislikes"] != DBNull.Value ? Convert.ToInt32(reader["Dislikes"]) : (int?)null) : null,
                                StudentName = ColumnExists(reader, "StudentName") ? reader["StudentName"]?.ToString() : null,
                                StudentImage = ColumnExists(reader, "StudentImage") ? reader["StudentImage"]?.ToString() : null,
                                TotalConnection = ColumnExists(reader, "TotalConnection") ? (reader["TotalConnection"] != DBNull.Value ? Convert.ToInt32(reader["TotalConnection"]) : (int?)null) : null,

                                // New fields based on the updated procedure
                                TotalBookings = ColumnExists(reader, "TotalBookings") ? (reader["TotalBookings"] != DBNull.Value ? Convert.ToInt32(reader["TotalBookings"]) : (int?)null) : null,
                                TotalClicks = ColumnExists(reader, "TotalClicks") ? (reader["TotalClicks"] != DBNull.Value ? Convert.ToInt32(reader["TotalClicks"]) : (int?)null) : null,
                                TotalFavourites = ColumnExists(reader, "TotalFavourites") ? (reader["TotalFavourites"] != DBNull.Value ? Convert.ToInt32(reader["TotalFavourites"]) : (int?)null) : null,
                                TrendRank = (int?)(ColumnExists(reader, "TrendRank") ? (reader["TrendRank"] != DBNull.Value ? Convert.ToDouble(reader["TrendRank"]) : (double?)null) : null),
                                LikeStatus = (int?)(ColumnExists(reader, "LikeStatus") ? (reader["LikeStatus"] != DBNull.Value ? Convert.ToInt32(reader["LikeStatus"]) : (int?)null) : null)
                                
                            };

                            trendingSkills.Add(trendingSkill);
                        }
                    }

                    var response = new
                    {
                        status = 200,
                        data = trendingSkills
                    };

                    return response;
                }
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspTrendingSkillsNew", 1);
                return "Error, something went wrong!";
            }
        }

        private bool ColumnExists(DbDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        #endregion Trending-Skills

        #region Skills-Like-DisLikes
        public async Task<string> TrendingSkillLikeDislikes(long skillId, long userId, bool likeStatus)
        {
            try
            {
                string strResponse = "";
                //to check skillId exists or not
                using(var checkCommand = _context.Database.GetDbConnection().CreateCommand())
                {
                    checkCommand.CommandText = "SELECT COUNT(1) FROM tblActivity WHERE ActivityIDP = @SkillId";
                    checkCommand.CommandType = System.Data.CommandType.Text;
                    checkCommand.Parameters.Add(new SqlParameter() { ParameterName = "@SkillID", SqlDbType = SqlDbType.BigInt, Value = skillId });

                    _context.Database.OpenConnection();
                    var result = await checkCommand.ExecuteScalarAsync();
                    _context.Database.CloseConnection();
                    if(Convert.ToInt32(result) == 0)
                    {
                        return "Error : SkillID doesnot exist";
                    }
                }
                //if skillID exists proceed to like/dislike
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspSkillLikeDislike";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SkillID", SqlDbType = SqlDbType.BigInt, Value = skillId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserID", SqlDbType = SqlDbType.BigInt, Value = userId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@LikeStatus", SqlDbType = SqlDbType.Bit, Value = likeStatus });

                    _context.Database.OpenConnection();
                    await command.ExecuteNonQueryAsync();

                    strResponse = "Success, Like/Dislike status updated!";
                }
                return strResponse;
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspSkilllikeDislike", 1);
                return "Error, Somethin went wrong!";
            }
        }
        #endregion

        #region Dashboard-BookActivity
        public async Task<string> BookActivity(long activityId, long userId, int seatCount, string contactIdFs, int bookingStatus = 1)
        {
            try
            {
                string strResponse = "";
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspBookActivity";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ActivityIDP", SqlDbType = SqlDbType.BigInt, Value = activityId });
                    command.Parameters.Add(new SqlParameter() { ParameterName="@UserID",SqlDbType=SqlDbType.BigInt, Value = userId });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@SeatCount", SqlDbType = SqlDbType.TinyInt, Value = seatCount });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@contactIDFs", SqlDbType = SqlDbType.NVarChar, Size=300, Value = contactIdFs });
                    command.Parameters.Add(new SqlParameter() { ParameterName ="@BookingStatus", SqlDbType = SqlDbType.TinyInt, Value= bookingStatus });

                    _context.Database.OpenConnection();

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while(await reader.ReadAsync())
                            {
                                strResponse = reader.GetString(0);
                            }
                        }
                    }
                    _context.Database.CloseConnection();
                }
                return strResponse;
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspBookActivity", 1);
                return "Error, Something went wrong ";
            }
        }
        #endregion

        #region get-User-info
        public async Task<string> User_Get()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetUser";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });

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
                await ErrorLog(1, e.Message, $"uspUser_GetAll_ByUserIDF", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion


        #region Dashboard-Get-Banners
        public async Task<List<BannerDTO>> GetBannersAsync()
        {
            var banners = new List<BannerDTO>();

            try
            {
                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetBanners";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    _context.Database.OpenConnection();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while(await reader.ReadAsync())
                            {
                                var banner = new BannerDTO
                                {
                                    BannerId = reader.GetInt64(0),
                                    //Attachment = reader.GetString(1)
                                    Attachment = baseBannerUrl + reader.GetString(1)
                                };
                                banners.Add(banner);
                            }
                        }
                    }
                    _context.Database.CloseConnection();
                }
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspGetBanners", 1);
                return new List<BannerDTO>();
            }
            return banners;
        }
        #endregion

        #region Skill-Interest-API- Service
        public async Task<List<SkillDTO>> GetInterestedSkillsAsync()
        {
            var skills = new List<SkillDTO>();

            try
            {
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetSkillInterest";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    _context.Database.OpenConnection();

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while(await reader.ReadAsync())
                            {
                                var skill = new SkillDTO
                                {
                                    ActivityIDP = reader.GetInt64(0),
                                    Banner = reader.GetString(1),
                                    Title = reader.GetString(2),
                                };
                                skills.Add(skill);
                            }
                        }
                    }
                    _context.Database.CloseConnection();
                }
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspGetSkillInterest", 1);
                return null;
            }
            return skills;
        }
        #endregion

        #region Dashboard - suggested offers
        public async Task<List<SuggestedOffersDTO>> GetSuggestedOffersAsync(int status, float? discountedPrice = null)
        {
            try
            {
                List<SuggestedOffersDTO> suggestedOffers = new List<SuggestedOffersDTO>();

                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspSuggestedOffers";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@Status",
                        SqlDbType = SqlDbType.Int,
                        Value = status
                    });

                    command.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@DiscountedPrice",
                        SqlDbType = SqlDbType.Float,
                        Value = discountedPrice ??(object)DBNull.Value
                    });

                    _context.Database.OpenConnection();
                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var offer = new SuggestedOffersDTO
                            {
                                OfferCoverImage = reader["OfferCoverImage"] != DBNull.Value ? reader["OfferCoverImage"].ToString() : string.Empty,
                                OfferPercentage = reader["OfferPercentage"] != DBNull.Value ? reader["OfferPercentage"].ToString() : string.Empty,
                                Title = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : string.Empty,
                                DateTime = reader["DateTime"] != DBNull.Value ? reader["DateTime"].ToString() : string.Empty,
                                ProfileImage = reader["ProfileImage"] != DBNull.Value ? reader["ProfileImage"].ToString() : string.Empty,
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty
                            };

                            suggestedOffers.Add(offer);
                        }
                    }
                }
                return suggestedOffers;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, "uspSuggestedOffers", 1);
                return null;
            }
        }
        #endregion

        #region Dashboard - trending-teachers
        public async Task<List<TrendingTeachersDTO>> GetTrendingTeachersAsync()
        {
            try
            {
                List<TrendingTeachersDTO> trendingTeachers = new List<TrendingTeachersDTO> ();

                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetTrendingTeachers";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    _context.Database.OpenConnection();
                    
                    
                    using (var reader =await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var teacher = new TrendingTeachersDTO
                            {
                                TeacherId = reader["TeacherID"] != DBNull.Value ? Convert.ToInt64(reader["TeacherID"]) : 0,
                                ProfileImage = reader["ProfileImage"] != DBNull.Value ? baseBannerUrl + reader["ProfileImage"].ToString() : string.Empty,
                                FirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : string.Empty,
                                LastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : string.Empty,
                                CompanyName = reader["CompanyName"] != DBNull.Value ? reader["CompanyName"].ToString() : string.Empty,
                                TotalSkillsPublished = reader["TotalSkillsPublished"] != DBNull.Value ? Convert.ToInt32(reader["TotalSkillsPublished"]) : 0,
                                TotalBookings = reader["TotalBookings"] != DBNull.Value ? Convert.ToInt64(reader["TotalBookings"]) : 0,
                                TotalPosts = reader["TotalPosts"] != DBNull.Value ? Convert.ToInt32(reader["TotalPosts"]) : 0,
                                TrendingScore = reader["TrendingScore"] != DBNull.Value ? Convert.ToInt32(reader["TrendingScore"]) : 0
                            };

                            trendingTeachers.Add(teacher);
                        }
                    }
                }
                return trendingTeachers;
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, "uspGetTrendingTeachers", 1);
                return null;
            }
        }
        #endregion

        #region Dashboard-Trending-clubs
        public async Task<List<ClubDTO>> GetTrendingClubsAsync()
        {
            try
            {
                List<ClubDTO> trendingClubs = new List<ClubDTO>();
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetTrendingClubs";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    _context.Database.OpenConnection();

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            var club = new ClubDTO
                            {
                                ClubID = reader["ClubIDP"] != DBNull.Value ? Convert.ToInt64(reader["ClubIDP"]) : 0,
                                ClubName = reader["ClubName"] != DBNull.Value ? reader["ClubName"].ToString() : string.Empty,
                                TotalSubscriptions = reader["TotalSubscriptions"] != DBNull.Value ? Convert.ToInt32(reader["TotalSubscriptions"]) : 0,
                                TotalSkills = reader["TotalSkills"] != DBNull.Value ? Convert.ToInt32(reader["TotalSkills"]) : 0,
                                TrendingScore = reader["TrendingScore"] != DBNull.Value ? Convert.ToInt32(reader["TrendingScore"]) : 0,
                                ClubImage = reader["ClubBanner"] != DBNull.Value ? reader["ClubBanner"].ToString() : string.Empty
                            };
                            trendingClubs.Add(club);
                        }
                    }
                    return trendingClubs;
                }
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, "uspGetTrendingClubs", 1);
                return null;
            }
        }
        #endregion

        #region Dashboard - GetStudentsList
        public async Task<List<StudentDTO>> GetStudentlISTAsync()
        {
            try
            {
                List<StudentDTO> studentsList = new List<StudentDTO>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetStudentList";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    _context.Database.OpenConnection();
                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            var student = new StudentDTO
                            {
                                StudentID = reader["StudentID"] != DBNull.Value ? Convert.ToInt64(reader["StudentID"]) : 0,
                                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString().Trim() : string.Empty,
                                EmailID = reader["EmailID"] != DBNull.Value ? reader["EmailID"].ToString() : string.Empty,
                                MobileNo = reader["MobileNo"] != DBNull.Value ? reader["MobileNo"].ToString() : string.Empty,
                                ProfileImage = reader["ProfileImage"] != DBNull.Value ? reader["ProfileImage"].ToString() : string.Empty
                            };
                            studentsList.Add(student);
                        }
                    }
                }
                return studentsList;
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, "uspGetStudentList", 1);
                return null;
            }
        }
        #endregion

        #region Dashboard - student - followers List
        public async Task<List<FollowerDTO>> GetFollowerListAsync(long userId)
        {
            try
            {
                List<FollowerDTO> followersList = new List<FollowerDTO>();
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetFollowersList";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var param = command.CreateParameter();
                    param.ParameterName = "UserIDP";
                    param.Value = userId;
                    command.Parameters.Add(param);
                    
                    _context.Database.OpenConnection();

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            var followers = new FollowerDTO
                            {
                                FollowerID = reader["FollowerID"] != DBNull.Value ? Convert.ToInt64(reader["FollowerID"]) : 0,
                                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString().Trim() : string.Empty,
                                ProfileImage = reader["ProfileImage"] != DBNull.Value ? reader["ProfileImage"].ToString() : string.Empty,
                                TotalFollowers = reader["TotalFollowers"] != DBNull.Value ? Convert.ToInt32(reader["TotalFollowers"]) : 0
                            };
                            followersList.Add(followers);
                        }
                    }
                    return followersList;
                }
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, "uspGetFollowersList", 1);
                return null;
            }
        }
        #endregion

        #region Dashboard-suggested-connections
        public async Task<List<ConnectionUserDTO>> GetSuggestedConnectionsAsync(long userId)
        {
            try
            {
                List<ConnectionUserDTO> suggestedUsers = new List<ConnectionUserDTO>();

                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetSuggestedConnections";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var param = command.CreateParameter();
                    param.ParameterName = "@UserIDP";
                    param.Value = userId;   
                    command.Parameters.Add(param);

                    _context.Database.OpenConnection();

                    using(var reader =await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new ConnectionUserDTO
                            {
                                UserIDP = reader["UserIDP"] != DBNull.Value ? Convert.ToInt64(reader["UserIDP"]) : 0,
                                FullName = reader["FullName"].ToString(),
                                //ProfileImage = reader["ProfileImage"].ToString()
                                ProfileImage = reader["ProfileImage"] != DBNull.Value ? baseBannerUrl + reader["ProfileImage"].ToString() : string.Empty,
                                IsFollowing = (int)(reader["IsFollowing"] != DBNull.Value ? Convert.ToInt32(reader["IsFollowing"]) : (int?)0),

                            };
                            suggestedUsers.Add(user);
                        }
                    }
                }
                return suggestedUsers;
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, "uspGetSuggestedConnections",1);
                return null;
            }
        }
        #endregion

        #region Dashboard - follow-unfollow-user
        public async Task<string> FollowUnfollowUserAsync(long userId, long followedUserId, int actionType)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC uspFollowUnfollowUser @UserIDP = {0}, @FollowedUserIDP = {1}, @ActionType = {2}",
                    userId, followedUserId, actionType);

                return "Success"; // Successfully followed or unfollowed
            }
            catch (SqlException ex)
            {
                // Check if the error message matches the "not following" scenario
                if (ex.Message.Contains("You are not following this user."))
                {
                    return "You are not following this user."; 
                }
                else if (ex.Message.Contains("You cannot follow/unfollow yourself."))
                {
                    return "You cannot follow/unfollow yourself."; 
                }
                else if (ex.Message.Contains("Invalid ActionType"))
                {
                    return "Invalid ActionType. Use 1 for follow, 0 for unfollow."; 
                }

                return "An error occurred while processing your request."; 
            }
            catch (Exception ex)
            {
                
                return "An unexpected error occurred.";
            }
        }
        #endregion

        #region Participant - info
        public async Task<(IEnumerable<ParticipentInfoDTO> Participants, int TotalBookings, int TotalActivities, int TotalPartners)> GetParticipantInfoAsync()
        {
            try
            {
                var ParticipantInfoList = new List<ParticipentInfoDTO>();
                int totalBookings = 0;
                int totalActivities = 0;
                int totalPartners = 0;


                using (var commeand = _context.Database.GetDbConnection().CreateCommand())
                {
                    commeand.CommandText = "dbo.uspGetParticipentsInfo";
                    commeand.CommandType = System.Data.CommandType.StoredProcedure;

                    await _context.Database.OpenConnectionAsync();

                    using(var reader = await commeand.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            //ProfileImage = reader["ProfirealeImage"] != DBNull.Value ? reader["ProfileImage"].ToString() : string.Empty,
                            var participantInfor = new ParticipentInfoDTO
                            {
                                ParticiantImage = reader["ParticipantImage"] != DBNull.Value ? reader["ParticipantImage"].ToString() : null,
                                ParticipantName = reader["ParticipantName"] != DBNull.Value ? reader["ParticipantName"].ToString() : null,
                                SkillName = reader["SkillName"] != DBNull.Value ? reader["SkillName"].ToString() : null,
                                ParticipantJoinDate = (DateTime)(reader["ParticipantJoinTime"] != DBNull.Value ? Convert.ToDateTime(reader["ParticipantJoinTime"]) : (DateTime?)null)
                            };
                            ParticipantInfoList.Add(participantInfor);
                        }   

                        //total activity bookings
                        if(await reader.NextResultAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                totalBookings = reader["TotalActivityBookings"] != DBNull.Value ? Convert.ToInt32(reader["TotalActivityBookings"]) : 0;
                            }
                        }

                        //total activities
                        if(await reader.NextResultAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                totalActivities = reader["TotalActivities"] != DBNull.Value ? Convert.ToInt32(reader["TotalActivities"]) : 0;
                            }
                        }
                        //total partners
                        if(await reader.NextResultAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                totalPartners = reader["TotalPartners"] != DBNull.Value ? Convert.ToInt32(reader["TotalPartners"]) : 0;
                            }
                        }
                    }
                }
                return (ParticipantInfoList, totalBookings, totalActivities, totalPartners) ;
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, "uspGetParticipentsInfo", 1);
                return (null,0,0,0);
            }
        }
        #endregion
                
    }
}
