using BMS_API.Models;
using BMS_API.Services.Interface;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace BMS_API.Services
{
    public class GalleryService : CommonService, IGalleryService
    {
        public GalleryService(BMSContext context) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }



        #region GetGalleryImageByUser
        public async Task<List<tblGalleryImageResponse>> GetGalleryImagesByUserAsync(long UserIDF)
        {
            var galleryImagesList = new List<tblGalleryImageResponse>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT * FROM tblGallery WHERE UserIDF = @UserIDF";
                command.CommandType = System.Data.CommandType.Text;

                var userIDFParam = new SqlParameter("@UserIDF", System.Data.SqlDbType.BigInt)
                {
                    Value = UserIDF
                };
                command.Parameters.Add(userIDFParam);

                _context.Database.OpenConnection();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";

                    while (await reader.ReadAsync())
                    {
                        var galleryImage = new tblGalleryImageResponse
                        {
                            GalleryID = reader["GalleryID"] != DBNull.Value ? (long)reader["GalleryID"] : 0,
                            UserIDF = reader["UserIDF"] != DBNull.Value ? (long)reader["UserIDF"] : 0,
                            ImageUrl = reader["ImageUrl"] != DBNull.Value
                                ? baseBannerUrl + reader["ImageUrl"].ToString()
                                : null
                        };

                        galleryImagesList.Add(galleryImage);
                    }
                }
            }

            return galleryImagesList;
        }

        //public async Task<tblGalleryImageResponse> GetGalleryImageByUser(long UserIDF)
        //{
        //    tblGalleryImageResponse GalleryImages = null;

        //    using (var command = _context.Database.GetDbConnection().CreateCommand())
        //    {
        //        command.CommandText = "select * from tblGallery where UserIDF = @UserIDF";
        //        command.CommandType = System.Data.CommandType.Text;

        //        var UserIDFParam = new SqlParameter("@UserIDF", System.Data.SqlDbType.BigInt)
        //        {
        //            Value = UserIDF
        //        };
        //        command.Parameters.Add(UserIDFParam);

        //        _context.Database.OpenConnection();

        //        using (var reader = await command.ExecuteReaderAsync())
        //        {
        //            if (reader.HasRows && await reader.ReadAsync())
        //            {
        //                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";
        //                string ImageUrl = reader["ImageUrl"]?.ToString();

        //                GalleryImages = new tblGalleryImageResponse
        //                {
        //                    GalleryID = reader["GalleryID"] != DBNull.Value ? (long)reader["GalleryID"] : 0, // Set the ActivityId
        //                    UserIDF = reader["UserIDF"] != DBNull.Value ? (long)reader["UserIDF"] : 0, // Set the ActivityId
        //                    ImageUrl = !string.IsNullOrEmpty(ImageUrl) ? baseBannerUrl + ImageUrl : null
        //                };
        //            }
        //        }
        //    }

        //    return GalleryImages;
        //}
        #endregion


        #region GetPostsById
        public async Task<tblGallery> GetGalleryImageById(long GalleryID)
        {
            tblGallery Gallery = null;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select * from tblGallery where GalleryID = @GalleryID";
                command.CommandType = System.Data.CommandType.Text;

                var GalleryIDParam = new SqlParameter("@GalleryID", System.Data.SqlDbType.BigInt)
                {
                    Value = GalleryID
                };
                command.Parameters.Add(GalleryIDParam);

                _context.Database.OpenConnection();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows && await reader.ReadAsync())
                    {
                        string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";
                        string ImageUrl = reader["ImageUrl"]?.ToString();

                        Gallery = new tblGallery
                        {

                            GalleryID = reader["GalleryID"] != DBNull.Value ? (long)reader["GalleryID"] : 0, // Set the ActivityId
                            ActivityIDF = reader["ActivityIDF"] != DBNull.Value ? (long)reader["ActivityIDF"] : 0, // Set the ActivityId
                            UserIDF = reader["UserIDF"] != DBNull.Value ? (long)reader["UserIDF"] : 0, // Set the ActivityId
                            ImageUrl = !string.IsNullOrEmpty(ImageUrl) ? baseBannerUrl + ImageUrl : null,
                            CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime?)reader["CreatedDate"] : null,

                        };
                    }
                }
            }

            return Gallery;
        }
        #endregion

        #region POst INSERT
        public async Task<Int64> Gallery_Insert(tblGallery modelActivity)
        {
            try
            {
                SqlParameter GalleryID = new SqlParameter
                {
                    ParameterName = "@GalleryID",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramActivityIDF = new SqlParameter("@ActivityIDF", (object)modelActivity.ActivityIDF ?? DBNull.Value);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramImageUrl = new SqlParameter("@ImageUrl", (object)modelActivity.ImageUrl ?? DBNull.Value);
                SqlParameter paramCreatedDate = new SqlParameter("@CreatedDate", (object)modelActivity.CreatedDate ?? DBNull.Value);


                var paramSqlQuery = "EXECUTE dbo.InsertUpdateGallery @GalleryID OUTPUT, @ActivityIDF, @UserIDF, @ImageUrl, @CreatedDate";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, GalleryID, paramActivityIDF, paramUserIDF, paramImageUrl, paramCreatedDate);

                return Convert.ToInt64(GalleryID.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPosts_Insert_Update", 1);
                return 0;
            }
        }
        #endregion POst Insert

        #region POst UPDATE
        public async Task<Int64> Gallery_Update(tblGallery modelActivity)
        {
            try
            {

                SqlParameter paramPostID = new SqlParameter("@GalleryID", (object)modelActivity.GalleryID ?? DBNull.Value);
                SqlParameter paramActivityIDF = new SqlParameter("@ActivityIDF", (object)modelActivity.ActivityIDF ?? DBNull.Value);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramImageUrl = new SqlParameter("@ImageUrl", (object)modelActivity.ImageUrl ?? DBNull.Value);
                SqlParameter paramCreatedDate = new SqlParameter("@CreatedDate", (object)modelActivity.CreatedDate ?? DBNull.Value);


                var paramSqlQuery = "EXECUTE dbo.InsertUpdateGallery @GalleryID, @ActivityIDF, @UserIDF, @ImageUrl, @CreatedDate";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramPostID, paramActivityIDF, paramUserIDF, paramImageUrl, paramCreatedDate);

                return 0;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPosts_Insert_Update", 1);
                return 0;
            }
        }
        #endregion POst UPDATE


    }
}
