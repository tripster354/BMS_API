using BMS_API.Models.DTOs;
using BMS_API.Models.User;
using BMS_API.Services.Interface.User;
using BudgetManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO.Compression;
using System.Threading.Tasks;

namespace BMS_API.Services
{
    public class UserService : IUserService
    {
        private readonly BMSContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(BMSContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region Create-User
        public async Task<tblUser> CreateUserAsync(tblUser user)
        {
            try
            {
                Guid newRowGuid = Guid.NewGuid();  // Generate a new RowGUID for the user

                // Generate a random 10-digit LoginToken
                Random random = new Random();
                long loginToken = random.Next(1000000000, int.MaxValue);  // Generate a random number between 1 billion and int.MaxValue (which will give you a 10-digit number)

                SqlParameter parameterUserIDP = new SqlParameter
                {
                    ParameterName = "@UserIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output,
                };

                SqlParameter parameterRowGUID = new SqlParameter("@RowGUID", newRowGuid);
                SqlParameter parameterEntryBy = new SqlParameter("@EntryBy", user.EntryBy);
                SqlParameter parameterFirstName = new SqlParameter("@FirstName", user.FirstName);
                SqlParameter parameterLastName = new SqlParameter("@LastName", (object)user.LastName ?? DBNull.Value);
                SqlParameter parameterFullName = new SqlParameter("@FullName", (object)user.FullName ?? DBNull.Value);
                SqlParameter parameterEmail = new SqlParameter("@EmailID", user.EmailID);
                SqlParameter parameterMobileNo = new SqlParameter("@MobileNo", (object)user.MobileNo ?? DBNull.Value);
                SqlParameter parameterProfileImage = new SqlParameter("@ProfileImage", (object)user.ProfileImage ?? DBNull.Value);
                SqlParameter parameterAddress = new SqlParameter("@Address", (object)user.Address ?? DBNull.Value);
                SqlParameter parameterAboutMe = new SqlParameter("@AboutMe", (object)user.AboutMe ?? DBNull.Value);
                SqlParameter parameterPassword = new SqlParameter("@Password", (object)user.Password ?? DBNull.Value);

                // Social Media Details
                SqlParameter parameterSocialGoogle = new SqlParameter("@SocialGoogle", (object)user.SocialGoogle ?? DBNull.Value);
                SqlParameter parameterSocialFacebook = new SqlParameter("@SocialFacebook", (object)user.SocialFacebook ?? DBNull.Value);
                SqlParameter parameterSocialLinkedIn = new SqlParameter("@SocialLinkedIn", (object)user.SocialLinkedIn ?? DBNull.Value);
                SqlParameter parameterSocialInstagram = new SqlParameter("@SocialInstagram", (object)user.SocialInstagram ?? DBNull.Value);
                SqlParameter parameterSocialTweeter = new SqlParameter("@SocialTweeter", (object)user.SocialTweeter ?? DBNull.Value);
                SqlParameter parameterSocialTelegram = new SqlParameter("@SocialTelegram", (object)user.SocialTelegram ?? DBNull.Value);
                SqlParameter parameterSocialOther = new SqlParameter("@SocialOther", (object)user.SocialOther ?? DBNull.Value);

                // Additional optional parameters
                SqlParameter parameterTotalFollowers = new SqlParameter("@TotalFollowers", (object)user.TotalFollowers ?? DBNull.Value);
                SqlParameter parameterTotalConnection = new SqlParameter("@TotalConnection", (object)user.TotalConnection ?? DBNull.Value);
                SqlParameter parameterRefrenceLink = new SqlParameter("@RefrenceLink", (object)user.RefrenceLink ?? DBNull.Value);
                SqlParameter parameterRefrenceLinkUsed = new SqlParameter("@RefrenceLinkUsed", (object)user.RefrenceLinkUsed ?? DBNull.Value);
                SqlParameter parameterOTP = new SqlParameter("@OTP", (object)user.OTP ?? DBNull.Value);
                SqlParameter parameterLoginToken = new SqlParameter("@LoginToken", loginToken);  // Use the generated login token
                SqlParameter parameterInterestIDs = new SqlParameter("@InterestIDs", (object)user.InterestIDs ?? DBNull.Value);
                SqlParameter parameterRegistrationDate = new SqlParameter("@RegistrationDate", (object)user.RegistrationDate ?? DBNull.Value);
                SqlParameter parameterEntryDate = new SqlParameter("@EntryDate", (object)user.EntryDate ?? DBNull.Value);

                SqlParameter parameterIsActive = new SqlParameter("@IsActive", user.IsActive);
                SqlParameter parameterIsDeleted = new SqlParameter("@IsDeleted", user.IsDeleted);

                SqlParameter parameterIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };

                // Construct SQL query to call the stored procedure
                string sqlQuery = "EXECUTE dbo.uspUser_Insert @UserIDP OUTPUT, @RowGUID, @EntryBy, @FirstName, @LastName, @FullName, @EmailID, @MobileNo, @ProfileImage, @Address, " +
                    "@AboutMe, @SocialGoogle, @SocialFacebook, @SocialLinkedIn, @SocialInstagram, @SocialTweeter, @SocialTelegram, @SocialOther, @TotalFollowers, @TotalConnection, " +
                    "@RefrenceLink, @RefrenceLinkUsed, @IsActive, @Password, @OTP, @LoginToken, @InterestIDs, @RegistrationDate, @EntryDate, @IsDeleted, @IsDuplicate OUTPUT";

                // Execute SQL
                await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                    parameterUserIDP,
                    parameterRowGUID,
                    parameterEntryBy,
                    parameterFirstName,
                    parameterLastName,
                    parameterFullName,
                    parameterEmail,
                    parameterMobileNo,
                    parameterProfileImage,
                    parameterAddress,
                    parameterAboutMe,
                    parameterSocialGoogle,
                    parameterSocialFacebook,
                    parameterSocialLinkedIn,
                    parameterSocialInstagram,
                    parameterSocialTweeter,
                    parameterSocialTelegram,
                    parameterSocialOther,
                    parameterTotalFollowers,
                    parameterTotalConnection,
                    parameterRefrenceLink,
                    parameterRefrenceLinkUsed,
                    parameterIsActive,
                    parameterPassword,
                    parameterOTP,
                    parameterLoginToken,
                    parameterInterestIDs,
                    parameterRegistrationDate,
                    parameterEntryDate,
                    parameterIsDeleted,
                    parameterIsDuplicate);

                if (Convert.ToInt32(parameterIsDuplicate.Value) == 1)
                {
                    return null; // Duplicate user found
                }

                user.UserIDP = Convert.ToInt64(parameterUserIDP.Value); // Set the new UserIDP for the user
                user.LoginToken = loginToken;  // Set the generated LoginToken for the user
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in CreateUserAsync: {ex.Message}");
                return null;
            }
        }


        #endregion


        #region user-Profile
        public async Task<IEnumerable<UserProfile>> GetUserProfileAsync(int UserId)
        {
            List<UserProfile> userReviews = new List<UserProfile>();
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UserProfile";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.Int, Value = UserId });

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            userReviews.Add(new UserProfile
                            {
                                UserId = Convert.ToInt32(reader["USERID"]),
                                ProfileImage = Convert.ToString(reader["PROFILEIMAGE"]),
                                SkillNumber = reader["SKILLNUMBER"].ToString(),
                                TotalFollower = reader["TOTALFOLLOWERS"].ToString(),
                                UserName = reader["FULLNAME"].ToString(),
                                Description = Convert.ToString(reader["DESCRIPTION"]),
                                Location = Convert.ToString(reader["LOCATION"]),
                            });
                        }
                    }
                    return userReviews;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetReviewsAsync: {ex.Message}");
                throw;
            }
        }
        #endregion


        #region user-Interest
        public async Task<IEnumerable<UserInterest>> GetUserInterestAsync(long UserId)
        {
            List<UserInterest> userReviews = new List<UserInterest>();
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "UserInterest";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            userReviews.Add(new UserInterest
                            {
                                InterestId = Convert.ToString(reader["INTERESTIDP"]),
                                InterestImage = Convert.ToString(reader["INTERESTIMAGE"]),
                                InterestName = Convert.ToString(reader["INTERESTNAME"]),
                            });
                        }
                    }
                    return userReviews;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetReviewsAsync: {ex.Message}");
                throw;
            }
        }
        #endregion


        #region Gell-All-Users
        public async Task<string> GetAllUsersAsync()
        {
            try
            {
                string strResponse = "";
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetAllUsers";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    _context.Database.OpenConnection();

                    DbDataReader ddr = await command.ExecuteReaderAsync();

                    DataSet ds = new DataSet();
                    ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "Users";

                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                return strResponse;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in getting all users details : {ex.Message}"); 
                return null;
            }
        }
        #endregion

        #region Get-User-ById
        public async Task<string> GetUserByIdAsync(int id)
        {
            try
            {
                string strResponse = "";

                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "useGetUserById";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@UserID",
                        SqlDbType = SqlDbType.Int,
                        Value = id
                    });

                    _context.Database.OpenConnection();

                    DbDataReader ddr = await command.ExecuteReaderAsync();

                    DataSet ds = new DataSet();
                    ds.Tables.Add().Load(ddr);
                    ds.Tables[0].TableName = "User";

                    strResponse = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                return strResponse;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in getting the user by id: {ex.Message}"); 
                return null;
            }
        }
        #endregion

        #region Update-User
        public async Task UpdateUserAsync(int id, tblUser user)
        {
            try
            {
                SqlParameter parameterUserIDP = new SqlParameter("@UserIDP", id);
                SqlParameter parameterFirstName = new SqlParameter("@FirstName", user.FirstName);
                SqlParameter parameterLastName = new SqlParameter("@LastName", user.LastName);
                SqlParameter parameterEMailId = new SqlParameter("@EmailID", user.EmailID);
                SqlParameter parameterMobileNo = new SqlParameter("@MobileNo", (object)user.MobileNo ?? DBNull.Value);
                SqlParameter parameterIsActive = new SqlParameter("@IsActive", user.IsActive);

                SqlParameter parameterIsNotFound = new SqlParameter
                {
                    ParameterName = "@IsNotFound",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                string sqlQuery = "EXECUTE dbo.uspUser_Update @UserIDP, @FirstName, @LastName, @EmailID, @MobileNo, @IsActive, @IsNotFound OUTPUT";

                await _context.Database.ExecuteSqlRawAsync(sqlQuery, sqlQuery, parameterUserIDP, parameterFirstName, parameterLastName, parameterEMailId, parameterMobileNo, parameterIsActive, parameterIsNotFound);

                if(Convert.ToInt32(parameterIsNotFound.Value) == 1)
                {
                    throw new KeyNotFoundException($"User with ID {id} is not found");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateUserAsync : {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Delete-User
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                
                SqlParameter parameterUserIDP = new SqlParameter("@UserIDP", id);

                
                string sqlQuery = "EXECUTE dbo.uspUser_Delete @UserIDP";
                int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameterUserIDP);

                
                if (rowsAffected <= 0)
                {
                    _logger.LogWarning($"No user found with ID {id} to delete.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteUserAsync: {ex.Message}");
                throw;  
            }
        }
        #endregion

        #region user-reviews
        public async Task<IEnumerable<UserReviewDTO>> GetUserReviewsAsync(int activityId)
        {
            List<UserReviewDTO> userReviews = new List<UserReviewDTO>();
            try
            {
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspGetUserReviews";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter{ ParameterName = "@ActivityID", SqlDbType = SqlDbType.BigInt, Value = activityId });

                    await _context.Database.OpenConnectionAsync();

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            userReviews.Add(new UserReviewDTO
                            {
                                ReviewID = Convert.ToInt64(reader["ReviewID"]),
                                Rating = Convert.ToInt32(reader["Rating"]),
                                ReviewText = reader["ReviewText"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                UserImage = reader["UserImage"].ToString(),
                                ReviewDate = Convert.ToDateTime(reader["ReviewDate"]),
                                LikeCount = Convert.ToInt32(reader["LikeCount"]),
                                DislikeCount = Convert.ToInt32(reader["DislikeCount"])
                            });
                        }
                    }
                    return userReviews;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in GetReviewsAsync: {ex.Message}");
                throw;
            }
        }
        #endregion

        #region user-profile-update
        public async Task<bool> UpdateUserProfileAsync(UserProfileDTO userProfile)
        {
            try
            {
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspUpdateUserProfile";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    await _context.Database.OpenConnectionAsync();

                    command.Parameters.Add(new SqlParameter { ParameterName = "@UserID", SqlDbType = SqlDbType.BigInt, Value = userProfile.UserID });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@FirstName", SqlDbType = SqlDbType.NVarChar, Size = 50, Value = userProfile.FirstName });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar, Size = 50, Value = userProfile.LastName });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@MobileNo", SqlDbType = SqlDbType.NVarChar, Size = 30, Value = userProfile.MobileNo });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@EmailID", SqlDbType = SqlDbType.NVarChar, Size = 50, Value = userProfile.EmailID });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@ProfileImage", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.ProfileImage });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@Address", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.Address });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@SocialGoogle", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.SocialGoogle });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@SocialFacebook", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.SocialFacebook });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@SocialLinkedIn", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.SocialLinkedIn });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@SocialInstagram", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.SocialInstagram });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@SocialTweeter", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.SocialTweeter });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@SocialTelegram", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.SocialTelegram });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@SocialOther", SqlDbType = SqlDbType.NVarChar, Size = 200, Value = userProfile.SocialOther });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@AboutMe", SqlDbType = SqlDbType.NVarChar, Size = 500, Value = userProfile.AboutMe });

                    var result = await command.ExecuteNonQueryAsync();

                    return result > 0;
                }
            }  
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateUserProfileAsync: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
