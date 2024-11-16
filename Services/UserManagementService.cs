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
using BMS_API.Models.Utility;
using Microsoft.AspNetCore.Http;
using System.IO;
using BMS_API.Models.DTOs;
using System.Collections.Generic;

namespace BMS_API.Services
{
    public class UserManagementService : CommonService, IUserManagementService
    {
        public UserManagementService(BMSContext context) : base(context)
        {
        }

        public AuthorisedUser ObjUser { get; set; }


        #region INSERT
        public async Task<Int64> UserManagement_Insert(ModelUserManagement modelUserManagement)
        {
            try
            {
                // Save files and get their paths
                string uploadDocPath = modelUserManagement.UploadDoc != null ? await SaveFileNewAsync(modelUserManagement.UploadDoc) : null;
                string videoPath = modelUserManagement.Video != null ? await SaveFileNewAsync(modelUserManagement.Video) : null;
                string profileImagePath = modelUserManagement.ProfileImage != null ? await SaveFileNewAsync(modelUserManagement.ProfileImage) : null;

                // Output parameter for UserIDP
                SqlParameter paramUserIDP = new SqlParameter
                {
                    ParameterName = "@UserIDP",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                // Other parameters
                SqlParameter paramFullName = new SqlParameter("@FullName", modelUserManagement.FullName);
                SqlParameter paramMobileNumber = new SqlParameter("@MobileNumber", modelUserManagement.MobileNumber);
                SqlParameter paramEmail = new SqlParameter("@Email", modelUserManagement.Email);
                SqlParameter paramInstaLink = new SqlParameter("@InstaLink", modelUserManagement.InstaLink ?? (object)DBNull.Value);
                SqlParameter paramTwitterLink = new SqlParameter("@TwitterLink", modelUserManagement.TwitterLink ?? (object)DBNull.Value);
                SqlParameter paramLinkedInLink = new SqlParameter("@LinkedInLink", modelUserManagement.LinkedInLink ?? (object)DBNull.Value);
                SqlParameter paramWebSite = new SqlParameter("@WebSite", modelUserManagement.WebSite ?? (object)DBNull.Value);
                SqlParameter paramActivityInterestName = new SqlParameter("@ActivityInterestName", modelUserManagement.ActivityInterestName ?? (object)DBNull.Value);
                SqlParameter paramYearsOfExperience = new SqlParameter("@YearsOfExperience",
            modelUserManagement.YearsOfExperience.HasValue ? (object)modelUserManagement.YearsOfExperience.Value : DBNull.Value);
                SqlParameter paramUploadDocName = new SqlParameter("@UploadDocName", uploadDocPath ?? (object)DBNull.Value);
                SqlParameter paramUploadDocumentName = new SqlParameter("@UploadDocumentName", modelUserManagement.UploadedDocumentName ?? (object)DBNull.Value);
                SqlParameter paramVideo = new SqlParameter("@Video", videoPath ?? (object)DBNull.Value);
                SqlParameter paramProfileImage = new SqlParameter("@ProfileImage", profileImagePath ?? (object)DBNull.Value);
                SqlParameter paramPassword = new SqlParameter("@Password", modelUserManagement.Password ?? (object)DBNull.Value);

                // Output parameter for IsDuplicate
                SqlParameter paramIsDuplicate = new SqlParameter
                {
                    ParameterName = "@IsDuplicate",
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = System.Data.ParameterDirection.Output
                };

                // Prepare SQL command with corrected parameter names
                var paramSqlQuery = "EXECUTE dbo.uspVendor_Create @UserIDP OUTPUT, @FullName, @MobileNumber, @Email, @Password, @InstaLink, @TwitterLink, @LinkedInLink, @WebSite, @ActivityInterestName, @YearsOfExperience, @UploadDocName, @UploadDocumentName, @Video,  @ProfileImage, @IsDuplicate OUTPUT";

                // Execute the SQL command
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramUserIDP, paramFullName, paramMobileNumber, paramEmail, paramPassword, paramInstaLink, paramTwitterLink, paramLinkedInLink, paramWebSite, paramActivityInterestName, paramYearsOfExperience, paramUploadDocName, paramUploadDocumentName,paramVideo,  paramProfileImage, paramIsDuplicate);

                // Check for duplicate and return UserIDP if valid
                if (Convert.ToBoolean(paramIsDuplicate.Value))
                {
                    return -1; // Indicate a duplicate user
                }

                // Return the UserIDP value if it exists
                return paramUserIDP.Value != DBNull.Value ? Convert.ToInt64(paramUserIDP.Value) : 0;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspsysUser_Insert", 1);
                return 0; // Return 0 in case of error
            }
        }


        public async Task<ModelUserManagementDTO> GetVendorById(long userId)
        {
            ModelUserManagementDTO vendor = null; // Initialize vendor as null

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT * FROM sysUser WHERE UserID = @UserIDP";
                command.CommandType = System.Data.CommandType.Text;

                // Add the parameter for UserIDP
                var userIdParam = new SqlParameter("@UserIDP", System.Data.SqlDbType.BigInt)
                {
                    Value = userId
                };
                command.Parameters.Add(userIdParam);

                 _context.Database.OpenConnection(); // Open the database connection

                using (var reader =  await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows && await reader.ReadAsync()) // Read the first row only
                    {
                        vendor = new ModelUserManagementDTO
                        {
                            UserIDP = reader["UserID"] != DBNull.Value ? Convert.ToInt64(reader["UserID"]) : 0,
                            FullName = reader["FullName"]?.ToString(),
                            MobileNumber = reader["MobileNumber"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            InstaLink = reader["InstaLink"]?.ToString(),
                            TwitterLink = reader["TwitterLink"]?.ToString(),
                            LinkedInLink = reader["LinkedInLink"]?.ToString(),
                            WebSite = reader["WebSite"]?.ToString(),
                            ActivityInterestName = reader["ActivityInterestName"]?.ToString(),
                            YearsOfExperience = reader["YearsOfExperience"] != DBNull.Value ? Convert.ToInt32(reader["YearsOfExperience"]) : 0,
                            UploadedDocumentName = reader["UploadDocumentName"]?.ToString(),
                            Token = reader["Token"].ToString(),
                            UploadDocPath = reader["UploadDocName"]?.ToString(),
                            ProfileImagePath = reader["ProfileImage"]?.ToString(),
                            VideoPath = reader["Video"]?.ToString(),
                            Password = reader["Password"]?.ToString(),
                            
                            // Add other fields as necessary
                        };
                    }
                }
            }
            return vendor; // Return the single vendor object
        }


        // Helper method to convert file to byte array
        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private async Task<string> SaveFileNewAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            // Define the directory path where files will be saved
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            // Generate unique file name
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            // Save file to the physical path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Construct the relative URL path
            string baseUrl = "https://bookmyskills.co.in/Uploads/";
            var fileUrl = Path.Combine(baseUrl, uniqueFileName).Replace("\\", "/");

            return fileUrl;
        }


        // Ensure to include the SaveFileAsync method
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file.Length > 0)
            {
                // Define the path where you want to save the file
                var filePath = Path.Combine("Your/Upload/Path", file.FileName);

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filePath; // Return the path of the saved file
            }
            return string.Empty;
        }


        #endregion INSERT


        #region UPDATE
        //public async Task<Int64> UserManagement_Update(ModelUserManagement modelUserManagement)
        //{
        //    try
        //    {
        //        SqlParameter paramUserIDP = new SqlParameter("@UserID", modelUserManagement.UserIDP);
        //        SqlParameter paramFullName = new SqlParameter("@FullName", modelUserManagement.FullName);
        //        SqlParameter paramMobileNumber = new SqlParameter("@MobileNumber", modelUserManagement.MobileNumber);
        //        SqlParameter paramEmail = new SqlParameter("@Email", modelUserManagement.Email);
        //        SqlParameter paramInstaLink = new SqlParameter("@InstaLink", modelUserManagement.InstaLink ?? (object)DBNull.Value);
        //        SqlParameter paramTwitterLink = new SqlParameter("@TwitterLink", modelUserManagement.TwitterLink ?? (object)DBNull.Value);
        //        SqlParameter paramLinkedInLink = new SqlParameter("@LinkedInLink", modelUserManagement.LinkedInLink ?? (object)DBNull.Value);
        //        SqlParameter paramWebSite = new SqlParameter("@WebSite", modelUserManagement.WebSite ?? (object)DBNull.Value);
        //        SqlParameter paramActivityInterestName = new SqlParameter("@ActivityInterestName", modelUserManagement.ActivityInterestName ?? (object)DBNull.Value);
        //        SqlParameter paramYearsOfExperience = new SqlParameter("@YearsOfExperience", (object)modelUserManagement.YearsOfExperience ?? DBNull.Value);
        //        SqlParameter paramUploadDocName = new SqlParameter("@UploadDocName", modelUserManagement.UploadDocName ?? (object)DBNull.Value);
        //        SqlParameter paramVideo = new SqlParameter("@Video", modelUserManagement.Video);
        //        SqlParameter paramProfileImage = new SqlParameter("@ProfileImage", modelUserManagement.ProfileImage ?? (object)DBNull.Value);
        //        SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
        //        SqlParameter paramIsDuplicate = new SqlParameter
        //        {
        //            ParameterName = "@IsDuplicate",
        //            SqlDbType = System.Data.SqlDbType.Bit,
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        // Update the stored procedure call
        //        var paramSqlQuery = "EXECUTE dbo.uspVendor_Update @UserID, @FullName, @MobileNumber, @Email, @InstaLink, @TwitterLink, @LinkedInLink, @WebSite, @ActivityInterestName, @YearsOfExperience, @UploadDocName, @Video, @ProfileImage, @EntryBy, @IsDuplicate OUTPUT";

        //        // Execute the stored procedure
        //        await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramUserIDP, paramFullName, paramMobileNumber, paramEmail, paramInstaLink, paramTwitterLink, paramLinkedInLink, paramWebSite, paramActivityInterestName, paramYearsOfExperience, paramUploadDocName, paramVideo, paramProfileImage, paramEntryBy, paramIsDuplicate);

        //        // Check for duplicates and return the result
        //        return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : Convert.ToInt64(paramUserIDP.Value);
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"uspsysUser_Update", 1);
        //        return 0;
        //    }
        //}
        #endregion UPDATE


        #region GET
        public async Task<string> UserManagement_Get(Int64 userIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspsysUser_Get";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = userIDP });

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
                await ErrorLog(1, e.Message, $"uspsysUser_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET

        #region GET_INVITE
        public async Task<string> UserManagement_Get_Invite(Int64 userIDP)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspsysUser_Get_Invite";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = userIDP });

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
                await ErrorLog(1, e.Message, $"uspsysUser_Get", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET_INVITE


        #region GET_ALL
        public async Task<string> UserManagement_GetAll(ModelCommonGetAll param)
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspsysUser_GetAll";
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
                await ErrorLog(1, e.Message, $"uspsysUser_GetAll", 1);
                return "Error, Something wrong!";
            }
        }
        #endregion GET_ALL


        #region ACTIVE-INACTIVE
        public async Task<Int64> UserManagement_ActiveInactive(Int64 userIDP, Boolean isActive)
        {
            try
            {
                SqlParameter paramUserIDP = new SqlParameter("@UserIDP", userIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsActive = new SqlParameter("@IsActive", isActive);

                var paramSqlQuery = "EXECUTE dbo.uspsysUser_Update_ActiveInActive @UserIDP, @IsActive, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIDP, paramIsActive, paramEntryBy);

                return 1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspsysUser_Update_ActiveInActive", 1);
                return 0;
            }
        }
        #endregion ACTIVE-INACTIVE

        #region DDL
        public async Task<string> UserManagement_DDL()
        {
            try
            {
                string strResponse = "";
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "uspsysUser_DDL";
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
                await ErrorLog(1, e.Message, $"uspsysUser_DDL", 1);
                return "";
            }
        }
        #endregion DDL

        #region DELETE
        public async Task<Int64> UserManagement_Delete(Int64 userIDP)
        {
            try
            {
                SqlParameter paramUserIDP = new SqlParameter("@UserIDP", userIDP);
                SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
                SqlParameter paramIsDeleted = new SqlParameter
                {
                    ParameterName = "@IsDeleted",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                var paramSqlQuery = "EXECUTE dbo.uspsysUser_Delete @UserIDP, @EntryBy, @IsDeleted OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIDP, paramEntryBy, paramIsDeleted);

                return (Convert.ToBoolean(paramIsDeleted.Value) == true) ? 1 : -1;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspsysUser_Delete", 1);
                return 0;
            }
        }
        #endregion DELETE

        #region UPDATE-MYPROFILE
        //public async Task<Int64> User_Update_MyProfile(ModelUserManagement modelUserManagement)
        //{
        //    try
        //    {
        //        SqlParameter paramUserIdp = new SqlParameter("@UserID", ObjUser.UserID);
        //        SqlParameter paramFullName = new SqlParameter("@FullName", modelUserManagement.FullName);
        //        SqlParameter paramMobileNumber = new SqlParameter("@MobileNumber", modelUserManagement.MobileNumber);
        //        SqlParameter paramEmail = new SqlParameter("@Email", modelUserManagement.Email);
        //        SqlParameter paramInstaLink = new SqlParameter("@InstaLink", modelUserManagement.InstaLink ?? (object)DBNull.Value);
        //        SqlParameter paramTwitterLink = new SqlParameter("@TwitterLink", modelUserManagement.TwitterLink ?? (object)DBNull.Value);
        //        SqlParameter paramLinkedInLink = new SqlParameter("@LinkedInLink", modelUserManagement.LinkedInLink ?? (object)DBNull.Value);
        //        SqlParameter paramWebSite = new SqlParameter("@WebSite", modelUserManagement.WebSite ?? (object)DBNull.Value);
        //        SqlParameter paramActivityInterestName = new SqlParameter("@ActivityInterestName", modelUserManagement.ActivityInterestName ?? (object)DBNull.Value);
        //        SqlParameter paramYearsOfExperience = new SqlParameter("@YearsOfExperience", (object)modelUserManagement.YearsOfExperience ?? DBNull.Value);
        //        SqlParameter paramUploadDocName = new SqlParameter("@UploadDocName", modelUserManagement.UploadDocName ?? (object)DBNull.Value);
        //        SqlParameter paramVideo = new SqlParameter("@Video", modelUserManagement.Video);
        //        SqlParameter paramProfileImage = new SqlParameter("@ProfileImage", modelUserManagement.ProfileImage ?? (object)DBNull.Value);
        //        SqlParameter paramEntryBy = new SqlParameter("@EntryBy", ObjUser.UserID);
        //        SqlParameter paramIsDuplicate = new SqlParameter
        //        {
        //            ParameterName = "@IsDuplicate",
        //            SqlDbType = System.Data.SqlDbType.Bit,
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        // Update the stored procedure call
        //        var paramSqlQuery = "EXECUTE dbo.uspVendor_Update_MyProfile @UserID, @FullName, @MobileNumber, @Email, @InstaLink, @TwitterLink, @LinkedInLink, @WebSite, @ActivityInterestName, @YearsOfExperience, @UploadDocName, @Video, @ProfileImage, @EntryBy, @IsDuplicate OUTPUT";

        //        // Execute the stored procedure
        //        await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramUserIdp, paramFullName, paramMobileNumber, paramEmail, paramInstaLink, paramTwitterLink, paramLinkedInLink, paramWebSite, paramActivityInterestName, paramYearsOfExperience, paramUploadDocName, paramVideo, paramProfileImage, paramEntryBy, paramIsDuplicate);

        //        // Check for duplicates and return the result
        //        return (Convert.ToBoolean(paramIsDuplicate.Value) == true) ? -1 : ObjUser.UserID;
        //    }
        //    catch (Exception e)
        //    {
        //        await ErrorLog(1, e.Message, $"uspsysUser_Update_MyProfile", 1);
        //        return 0;
        //    }
        //}
        #endregion UPDATE-MYPROFILE

        #region UPDATE-MYCREDENTIAL

        public async Task<Int64> User_Update_MyCredential(ModelUpdateCredential updateCredential)
        {
            try
            {
                SqlParameter paramUserIdp = new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID };
                SqlParameter paramPassword = new SqlParameter() { ParameterName = "@Password", SqlDbType = SqlDbType.NVarChar, Value = updateCredential.Password };
                SqlParameter paramOldPassword = new SqlParameter() { ParameterName = "@OldPassword", SqlDbType = SqlDbType.NVarChar, Value = updateCredential.OldPassword };
                SqlParameter paramEntryBy = new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.Int, Value = ObjUser.UserID };

                SqlParameter paramIsDone = new SqlParameter
                {
                    ParameterName = "@IsDone",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output,
                };

                var paramSqlQuery = "EXECUTE dbo.uspsysUser_Update_MyCredential @UserIDP, @Password, @OldPassword, @EntryBy, @IsDone OUTPUT";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIdp, paramPassword, paramOldPassword, paramEntryBy, paramIsDone);

                return Convert.ToInt32(paramIsDone.Value);
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspsysUser_Update_MyCredential", 1);
                return 0;
            }
        }
        #endregion UPDATE-MYCREDENTIAL

        #region UPDATE-MYNOTIFICATION
        public async Task<Int64> User_Update_MyNotification(ModelNotification modelNotification)
        {
            try
            {
                SqlParameter paramUserIdp = new SqlParameter() { ParameterName = "@UserIDP", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID };
                SqlParameter paramNotificationInApp = new SqlParameter() { ParameterName = "@NotificationInApp", SqlDbType = SqlDbType.Bit, Value = modelNotification.NotificationInApp };
                SqlParameter paramNotificationEmail = new SqlParameter() { ParameterName = "@NotificationEmail", SqlDbType = SqlDbType.Bit, Value = modelNotification.NotificationEmail };
                SqlParameter paramNotificationSMS = new SqlParameter() { ParameterName = "@NotificationSMS", SqlDbType = SqlDbType.Bit, Value = modelNotification.NotificationSMS };
                SqlParameter paramEntryBy = new SqlParameter() { ParameterName = "@EntryBy", SqlDbType = SqlDbType.Int, Value = ObjUser.UserID };

                var paramSqlQuery = "EXECUTE dbo.uspsysUser_Update_MyNotification @UserIDP, @NotificationInApp, @NotificationEmail,@NotificationSMS, @EntryBy";
                _context.Database.ExecuteSqlRaw(paramSqlQuery, paramUserIdp, paramNotificationInApp, paramNotificationEmail, paramNotificationSMS, paramEntryBy);

                return 1;
            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspsysUser_Update_MyNotification", 1);
                return 0;
            }
        }

        public Task<long> UserManagement_Update(ModelUserManagement modelUserManagement)
        {
            throw new NotImplementedException();
        }

        public Task<long> User_Update_MyProfile(ModelUserManagement modelUserManagement)
        {
            throw new NotImplementedException();
        }
        #endregion UPDATE-MYNOTIFICATION

        #region Vendor-login-with-mobile

        public async Task<VendorLoginResponse> LoginWithMobileOTP(string loginInput, int otp)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "uspVendorLogin"; // Your stored procedure
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Set up parameters
                command.Parameters.Add(new SqlParameter("@LoginInput", loginInput));
                command.Parameters.Add(new SqlParameter("@OTP", otp));

                await _context.Database.OpenConnectionAsync();

                DbDataReader ddr = await command.ExecuteReaderAsync();
                DataTable dt = new DataTable();
                dt.Load(ddr);

                // Check if the result contains the "Status" column
                if (dt.Columns.Contains("Status") && Convert.ToInt32(dt.Rows[0]["Status"]) == 0)
                {
                    // Handle case when user is not found
                    return null; // Or return some error object/message if preferred
                }

                // If columns exist, return the user data
                if (dt.Rows.Count > 0 && dt.Columns.Contains("UserID"))
                {
                    return new VendorLoginResponse
                    {
                        UserID = dt.Rows[0]["UserID"] != DBNull.Value ? Convert.ToInt64(dt.Rows[0]["UserID"]) : 0,
                        FullName = dt.Rows[0]["FullName"]?.ToString() ?? "Unknown",
                        Email = dt.Rows[0]["Email"]?.ToString() ?? "Unknown",
                        MobileNumber = dt.Rows[0]["MobileNumber"]?.ToString() ?? "Unknown",
                        InstaLink = dt.Rows[0]["InstaLink"]?.ToString() ?? "Unknown",
                        TwitterLink = dt.Rows[0]["TwitterLink"]?.ToString() ?? "Unknown",
                        LinkedInLink = dt.Rows[0]["LinkedInLink"]?.ToString() ?? "Unknown",
                        Website = dt.Rows[0]["WebSite"]?.ToString() ?? "Unknown",
                        ActivityInterestName = dt.Rows[0]["ActivityInterestName"]?.ToString() ?? "Unknown",
                        YearsOfExperience = dt.Rows[0]["YearsOfExperience"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["YearsOfExperience"]) : 0,
                        UserType = (int)ICommon.UserType.Vendor, // Assuming you still want to set this but have no actual UserType in db
                        CurrentToken = dt.Rows[0]["GeneratedToken"]?.ToString() // Return the generated token
                    };
                }

                return null; // User not found or login failed
            }
        }


        #endregion

        #region activityInterestNames
        public async Task<List<ActivityInterestDTO>> GetAllInterestActivityNames()
        {
            List<ActivityInterestDTO> interests = new List<ActivityInterestDTO>();

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT ActivityId, ActivityInterestName FROM tblActivityInterests";
                    command.CommandType = CommandType.Text;

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            interests.Add(new ActivityInterestDTO
                            {
                                ActivityID = reader.GetInt32(0),
                                ActivityInterestName = reader.GetString(1),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Option 1: Log the exception and rethrow or return null/empty list depending on how you want to handle this.
                Console.WriteLine(ex.Message);  // Log or use a logging library
                return new List<ActivityInterestDTO>(); // Return empty list if there's an error
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();  // Ensure connection is closed
            }

            return interests;  // Return the collected activity interests
        }

        #endregion
    }
}
