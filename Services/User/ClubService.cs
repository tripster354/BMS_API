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
using System.Globalization;
using BMS_API.Models.DTOs;

namespace BMS_API.Services.User
{
    public class ClubService : CommonService, IClubService
    {
        public ClubService(BMSContext context) : base(context)
        {
            _context = context;
        }

        public AuthorisedUser ObjUser { get; set; }

        #region Club INSERT
        public async Task<Int64> Club_Insert(Club modelClub)
        {
            try
            {
                SqlParameter paramClubIDP = new SqlParameter
                {
                    ParameterName = "@ClubIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramInterestIDF = new SqlParameter("@InterestIDF", (object)modelClub.InterestIDF ?? DBNull.Value);
                SqlParameter paramClubName = new SqlParameter("@ClubName", (object)modelClub.ClubName ?? DBNull.Value);
                SqlParameter paramClubBanner = new SqlParameter("@ClubBanner", (object)modelClub.ClubBanner ?? DBNull.Value);
                SqlParameter paramClubVenue = new SqlParameter("@ClubVenue", (object)modelClub.ClubVenue ?? DBNull.Value);
                SqlParameter paramClubDescription = new SqlParameter("@ClubDescription", (object)modelClub.ClubDescription ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", Convert.ToInt32(ObjUser.UserID));

                var paramSqlQuery = "EXECUTE dbo.uspClub_Insert @ClubIDP OUTPUT, @UserIDF, @InterestIDF, @ClubName, @ClubBanner,  @ClubVenue, @ClubDescription, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramClubIDP, paramUserIDF, paramInterestIDF, paramClubName, paramClubBanner, paramClubVenue, paramClubDescription, paramEntryBy);

                return Convert.ToInt64(paramClubIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspClub_Insert", 1);
                return 0;
            }
        }
        #endregion Subscribe_Club 


        #region Club INSERT
        public async Task<Int64> Subscribe_Club(SubscribeClub Entity)
        {
            try
            {
                SqlParameter paramClubSubscriptionIDP = new SqlParameter
                {
                    ParameterName = "@ClubSubscriptionIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramPartnerIDF = new SqlParameter("@PartnerIDF", (object)Entity.PartnerIDF ?? DBNull.Value);
                SqlParameter paramClubIDF = new SqlParameter("@ClubIDF", (object)Entity.ClubIDF ?? DBNull.Value);
                SqlParameter paramIsPaid = new SqlParameter("@IsPaid", (object)Entity.IsPaid ?? DBNull.Value);
                SqlParameter paramSubscriptionDate = new SqlParameter("@SubscriptionDate", (object)Entity.SubscriptionDate ?? DBNull.Value);
                SqlParameter paramIsDeleted = new SqlParameter("@IsDeleted", (object)Entity.IsDeleted ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", Convert.ToInt32(ObjUser.UserID));
                SqlParameter paramEntryDate = new SqlParameter("@EntryDate", DateTime.Now);

                var paramSqlQuery = "EXECUTE dbo.uspClub_Subscribe @ClubSubscriptionIDP OUTPUT, @UserIDF, @PartnerIDF, @ClubIDF, @IsPaid,  @SubscriptionDate, @IsDeleted, @EntryBy, @EntryDate";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramClubSubscriptionIDP, paramUserIDF, paramPartnerIDF, paramClubIDF, paramIsPaid, paramSubscriptionDate, paramIsDeleted, paramEntryBy, paramEntryDate);

                return Convert.ToInt64(paramClubSubscriptionIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspClub_Insert", 1);
                return 0;
            }
        }
        #endregion Subscribe_Club



        #region Club UPDATE
        public async Task<Int64> Club_Update(Club modelClub)
        {
            try
            {

                SqlParameter paramClubIDP = new SqlParameter("@ClubIDP", (object)modelClub.ClubIDP ?? DBNull.Value);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramInterestIDF = new SqlParameter("@InterestIDF", (object)modelClub.InterestIDF ?? DBNull.Value);
                SqlParameter paramClubName = new SqlParameter("@ClubName", (object)modelClub.ClubName ?? DBNull.Value);
                SqlParameter paramClubBanner = new SqlParameter("@ClubBanner", (object)modelClub.ClubBanner ?? DBNull.Value);
                SqlParameter paramClubVenue = new SqlParameter("@ClubVenue", (object)modelClub.ClubVenue ?? DBNull.Value);
                SqlParameter paramClubDescription = new SqlParameter("@ClubDescription", (object)modelClub.ClubDescription ?? DBNull.Value);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);

                var paramSqlQuery = "EXECUTE dbo.uspClub_Update @ClubIDP, @UserIDF, @InterestIDF, @ClubName, @ClubBanner, @ClubVenue, @ClubDescription, @Latitude, @GeoLocation, @EntryBy";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramClubIDP, paramUserIDF, paramInterestIDF, paramClubName, paramClubBanner, paramClubVenue, paramClubDescription, paramEntryBy);

                return Convert.ToInt64(paramClubIDP.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspClub_Update", 1);
                return 0;
            }
        }
        #endregion Club UPDATE


        //#region  Club GET_ALL
        //public async Task<ClubResponse> GetClubList()
        //{
        //    try
        //    {
        //        var clubs = new List<ClubResponse>();
     
        //        using (var command = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            command.CommandText = "GetClubList";
        //            command.CommandType = System.Data.CommandType.StoredProcedure;

        //            await _context.Database.OpenConnectionAsync();

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    var clubInfo = new ClubResponse
        //                    {
        //                        ClubIDP = reader.GetInt64(reader.GetOrdinal("ClubIDP")),
        //                        ClubName = reader.IsDBNull(reader.GetOrdinal("ClubName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubName")),
        //                        ClubBanner = reader.IsDBNull(reader.GetOrdinal("ClubBanner")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubBanner")),
        //                        ClubVenue = reader.IsDBNull(reader.GetOrdinal("ClubVenue")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubVenue")),
        //                        ClubDescription = reader.IsDBNull(reader.GetOrdinal("ClubDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubDescription")),
        //                        UserInfoList = new List<tblUser>()
        //                    };



        //                }
        //            }
        //            await _context.Database.CloseConnectionAsync();
        //        }
        //        return clubs;

        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"uspClub_GetAll_ByUser", 1);
        //        return "Error, Something wrong!";
        //    }
        //}
        //#endregion Club GET_ALL

        #region Trending Club GET_ALL
        public async Task<string> TrendingClub_GetAll()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspClub_GetAll_ByUser";
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
                await ErrorLog(1, e.Message, $"uspClub_GetAll_ByUser", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Trending Club GET_ALL

        #region Club GET
        public async Task<string> Club_Get(Int64 clubIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspClub_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@ClubIDP", SqlDbType = SqlDbType.BigInt, Value = clubIDP });

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
                await ErrorLog(1, e.Message, $"uspClub_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion Club GET
        
        #region Club- get -skill block
        public async Task<List<ClubSkillDTO>> GetClubSkillsAsync()
        {
            try
            {
                var clubSkills = new List<ClubSkillDTO>();

                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetClubSkills";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    _context.Database.OpenConnection();

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var skill = new ClubSkillDTO
                            {
                                SkillID = reader["SkillID"] != DBNull.Value ? Convert.ToInt64(reader["SkillID"]) : 0,
                                SkillCoverImage = reader["SkillCoverImage"] != DBNull.Value ? reader["SkillCoverImage"].ToString() : string.Empty,
                                SkillName = reader["SkillName"] != DBNull.Value ? reader["SkillName"].ToString() : string.Empty,
                                StartDateTime = reader["StartDateTime"] != DBNull.Value ? Convert.ToDateTime(reader["StartDateTime"]) : (DateTime?)null,
                                EndDateTime = reader["EndDateTime"] != DBNull.Value ? Convert.ToDateTime(reader["EndDateTime"]) : (DateTime?)null,
                                ProfileImage = reader["ProfileImage"] != DBNull.Value ? reader["ProfileImage"].ToString() : string.Empty,
                                UserName = reader["UserName"] != DBNull.Value ? reader["UserName"].ToString() : string.Empty,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                                TotalSeats = reader["TotalSeats"] != DBNull.Value ? Convert.ToInt32(reader["TotalSeats"]) : 0,
                                AllocatedSeats = reader["AllocatedSeats"] != DBNull.Value ? Convert.ToInt32(reader["AllocatedSeats"]) : 0
                            };
                            clubSkills.Add(skill);
                        }
                    }
                    _context.Database.CloseConnection();
                }
                return clubSkills;
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspGetClubSkills",1);
                return null;
            }
        }
        #endregion

        #region Club-skill-book
        public async Task<string> ClubSkillBookAsync(long activityId, long userId, int seatsBooked, DateTime bookingDate, long partnerId, string contactIdFs, int entryBy)
        {
            try
            {
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspClubBookSkill";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ActivityID", SqlDbType.BigInt) { Value = activityId });
                    command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt) { Value = userId });
                    command.Parameters.Add(new SqlParameter("@SeatsBooked", SqlDbType.Int) { Value = seatsBooked });
                    command.Parameters.Add(new SqlParameter("@BookingDate", SqlDbType.DateTime) { Value = bookingDate });
                    command.Parameters.Add(new SqlParameter("@PartnerID", SqlDbType.BigInt) { Value = partnerId });
                    command.Parameters.Add(new SqlParameter("@ContactIDFs", SqlDbType.NVarChar, 200) { Value = contactIdFs });
                    command.Parameters.Add(new SqlParameter("@EntryBy", SqlDbType.Int) { Value = entryBy });

                    await _context.Database.OpenConnectionAsync();

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                return reader["Message"].ToString();
                            }
                        }
                    }

                    await _context.Database.CloseConnectionAsync();

                    return "Booking failed";
                }
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspClubBookSkill", 1);
                return null;
            }
        }
        #endregion

        #region Get-all-Club-tutors
        public async Task<IEnumerable<ClubTutorDTO>> GetAllClubTutorsAsync()
        {
            try
            {
                var tutors = new List<ClubTutorDTO>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetAllClubTotors";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tutors.Add(new ClubTutorDTO
                            {
                                TutorId = reader.GetInt64(reader.GetOrdinal("TutorId")),
                                ProfileImage = reader.IsDBNull(reader.GetOrdinal("ProfileImage")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProfileImage")),
                                ClubName = reader.IsDBNull(reader.GetOrdinal("ClubName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubName")),
                                ClubDescription =reader.IsDBNull(reader.GetOrdinal("ClubDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubDescription"))
                            });
                        }
                    }
                    await _context.Database.CloseConnectionAsync();
                }
                return tutors;
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspGetAllClubTutors", 1);
                return null;
            }
        }
        #endregion

        #region Get-clubTutor - ById
        public async Task<IEnumerable<ClubTutorDTO>> GetClubTutorByIdAsync(long tutorId)
        {
            try
            {
                var tutor = new List<ClubTutorDTO>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetClubTutorsById";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@TutorId", SqlDbType.BigInt) { Value = tutorId });

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tutor.Add(new ClubTutorDTO
                            {
                                TutorId = reader.GetInt64(reader.GetOrdinal("TutorId")),
                                ProfileImage = reader.IsDBNull(reader.GetOrdinal("ProfileImage")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProfileImage")),
                                ClubName = reader.IsDBNull(reader.GetOrdinal("ClubName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubName")),
                                ClubDescription = reader.IsDBNull(reader.GetOrdinal("ClubDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClubDescription"))
                            });
                        }
                    }
                    await _context.Database.CloseConnectionAsync();
                }
                return tutor;
            }
            catch(Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspGetClubTutorsById", 1);
                return null;
            }
        }
        #endregion

        #region Club-like-post
        public async Task LikeClubPostAsync(long clubId, long userId, bool isLiked)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspLikeClubPost";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ClubIDF", SqlDbType.BigInt) { Value = clubId });
                    command.Parameters.Add(new SqlParameter("@UserIDF", SqlDbType.BigInt) { Value = userId });
                    command.Parameters.Add(new SqlParameter("@IsLiked", SqlDbType.Bit) { Value = isLiked });
                    command.Parameters.Add(new SqlParameter("@EntryBy", SqlDbType.BigInt) { Value = userId });

                    await _context.Database.OpenConnectionAsync();
                    await command.ExecuteReaderAsync();
                    await _context.Database.CloseConnectionAsync();
                }
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspLikeClubPost", 1);
            }
        }
        #endregion

        #region Club - comment - post
        public async Task CommentOnClubPostAsync(long clubId, long userId, string comment)
        {
            try
            {
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspCommentOnClubPost";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@ClubIDF", SqlDbType.BigInt) { Value = clubId });
                    command.Parameters.Add(new SqlParameter("@UserIDF", SqlDbType.BigInt) { Value = userId });
                    command.Parameters.Add(new SqlParameter("@Comment", SqlDbType.NVarChar, 1000) { Value = comment });
                    command.Parameters.Add(new SqlParameter("@EntryBy", SqlDbType.BigInt) { Value = userId });

                    await _context.Database.OpenConnectionAsync();
                    await command.ExecuteReaderAsync();
                    await _context.Database.CloseConnectionAsync();
                }
            }
            catch (Exception ex)
            {
                await ErrorLog(1, ex.Message, $"uspCommentOnClubPost",1);
            }
        }
        #endregion
    }
}
