using BMS_API.Models.DTOs;
using BMS_API.Services.Interface;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Data.Common;
using BMS_API.Models;
using System.Globalization;
using System.Linq.Expressions;

namespace BMS_API.Services
{

    public class PostService : CommonService, IPostsService
    {
        public PostService(BMSContext context) : base(context)
        {
            _context = context;
        }
        public AuthorisedUser ObjUser { get; set; }


        #region GetPostsById
        public async Task<tblPosts> GetPostsById(long PostID)
        {
            tblPosts Post = null;

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select * from tblPosts where PostID = @PostID";
                command.CommandType = System.Data.CommandType.Text;

                var PostIdParam = new SqlParameter("@PostID", System.Data.SqlDbType.BigInt)
                {
                    Value = PostID
                };
                command.Parameters.Add(PostIdParam);

                _context.Database.OpenConnection();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows && await reader.ReadAsync())
                    {
                        string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";
                        string PostImage = reader["PostImage"]?.ToString();

                        Post = new tblPosts
                        {

                            PostID = reader["PostID"] != DBNull.Value ? (long)reader["PostID"] : 0, // Set the ActivityId
                            ActivityIDF = reader["ActivityIDF"] != DBNull.Value ? (long)reader["ActivityIDF"] : 0, // Set the ActivityId
                            UserIDF = reader["UserIDF"] != DBNull.Value ? (long)reader["UserIDF"] : 0, // Set the ActivityId
                            PostImage = !string.IsNullOrEmpty(PostImage) ? baseBannerUrl + PostImage : null,
                            PostDescription = reader["PostDescription"]?.ToString(),
                            PostTitle = reader["PostTitle"]?.ToString(),
                            //Comments = reader["Comments"]?.ToString(),
                            LikeStatus = reader["LikeStatus"] != DBNull.Value ? (int)reader["Likes"] : 0,
                            CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime?)reader["CreatedDate"] : null,
 
                        };
                    }
                }
            }

            return Post;
        }
        #endregion

        #region Comments-By-post
        public async Task<bool> InsCommentsBypost(long PostID, string CommentText)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "Update tblPosts set Comments = @CommentText where PostID = @PostID";
                    command.CommandType = System.Data.CommandType.Text;

                    var PostIdParam = new SqlParameter("@PostID", System.Data.SqlDbType.BigInt)
                    {
                        Value = PostID
                    };
                    var CommentTextParam = new SqlParameter("@CommentText", System.Data.SqlDbType.NVarChar)
                    {
                        Value = CommentText
                    };
                    command.Parameters.Add(CommentTextParam);
                    command.Parameters.Add(PostIdParam);

                    await _context.Database.OpenConnectionAsync();

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    // Return true if at least one row was affected, otherwise false
                    return rowsAffected > 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        #region SetPostLikeStatus
        public async Task<bool> SetPostLikeStatus(long PostID, int LikeStatus)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {


                    command.CommandText = "PostLikeDisLike";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter paramUserID = new SqlParameter("@UserID", ObjUser.UserID);
                    var PostIdParam = new SqlParameter("@PostID", System.Data.SqlDbType.BigInt)
                    {
                        Value = PostID
                    };
                    var LikeStatusParam = new SqlParameter("@LikeStatus", System.Data.SqlDbType.Int)
                    {
                        Value = LikeStatus
                    };
                    command.Parameters.Add(LikeStatusParam);
                    command.Parameters.Add(PostIdParam);
                    command.Parameters.Add(paramUserID);

                    await _context.Database.OpenConnectionAsync();

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    // Return true if at least one row was affected, otherwise false
                    return rowsAffected > 0;
                }

            }
            catch (Exception ex) 
            {
                throw ex;
            }
           
        }

        #endregion

        #region SetPostLikeStatus
        public async Task<bool> DisLikePostStatus(long PostID, int LikeStatus)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {

                    command.CommandText = "Update tblPosts set LikeStatus = 0 where PostID = @PostID";
                    command.CommandType = System.Data.CommandType.Text;

                    var PostIdParam = new SqlParameter("@PostID", System.Data.SqlDbType.BigInt)
                    {
                        Value = PostID
                    };
                    var LikeStatusParam = new SqlParameter("@LikeStatus", System.Data.SqlDbType.Int)
                    {
                        Value = LikeStatus
                    };
                    command.Parameters.Add(LikeStatusParam);
                    command.Parameters.Add(PostIdParam);

                    await _context.Database.OpenConnectionAsync();

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    // Return true if at least one row was affected, otherwise false
                    return rowsAffected > 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion



        #region POst INSERT
        public async Task<Int64> Posts_Insert(tblPosts modelActivity)
        {
            try
            {
                SqlParameter PostID = new SqlParameter
                {
                    ParameterName = "@PostID",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };
                SqlParameter paramActivityIDF = new SqlParameter("@ActivityIDF", (object)modelActivity.ActivityIDF ?? DBNull.Value);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramPostDescription = new SqlParameter("@PostDescription", (object)modelActivity.PostDescription ?? DBNull.Value);
                SqlParameter paramCreatedDate = new SqlParameter("@CreatedDate", (object)modelActivity.CreatedDate ?? DBNull.Value);
                SqlParameter paramPostImage = new SqlParameter("@PostImage", (object)modelActivity.PostImage ?? DBNull.Value);
                SqlParameter paramPostTitle = new SqlParameter("@PostTitile", (object)modelActivity.PostTitle ?? DBNull.Value);


                var paramSqlQuery = "EXECUTE dbo.InsertUpdatePosts @PostID OUTPUT, @ActivityIDF, @UserIDF, @PostDescription, @CreatedDate, @PostImage,@PostTitile";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, PostID, paramActivityIDF, paramUserIDF, paramPostDescription, paramCreatedDate, paramPostImage,paramPostTitle);

                return Convert.ToInt64(PostID.Value);

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPosts_Insert_Update", 1);
                return 0;
            }
        }
        #endregion POst Insert


        #region POst UPDATE
        public async Task<Int64> Posts_Update(tblPosts modelActivity)
        {
            try
            {

                SqlParameter paramPostID = new SqlParameter("@PostID", (object)modelActivity.PostID ?? DBNull.Value);
                SqlParameter paramActivityIDF = new SqlParameter("@ActivityIDF", (object)modelActivity.ActivityIDF ?? DBNull.Value);
                SqlParameter paramUserIDF = new SqlParameter("@UserIDF", ObjUser.UserID);
                SqlParameter paramPostDescription = new SqlParameter("@PostDescription", (object)modelActivity.PostDescription ?? DBNull.Value);
                SqlParameter paramCreatedDate = new SqlParameter("@CreatedDate", (object)modelActivity.CreatedDate ?? DBNull.Value);
                SqlParameter paramPostImage = new SqlParameter("@PostImage", (object)modelActivity.PostImage ?? DBNull.Value);
                SqlParameter paramPostTitle = new SqlParameter("@PostTitile", (object)modelActivity.PostTitle ?? DBNull.Value);


                var paramSqlQuery = "EXECUTE dbo.InsertUpdatePosts @PostID, @ActivityIDF, @UserIDF, @PostDescription, @CreatedDate, @PostImage,@PostTitile";
                await _context.Database.ExecuteSqlRawAsync(paramSqlQuery, paramPostID, paramActivityIDF, paramUserIDF, paramPostDescription, paramCreatedDate, paramPostImage,paramPostTitle);

                return 0;

            }
            catch (Exception e)
            {
                await ErrorLog(1, e.Message, $"uspPosts_Insert_Update", 1);
                return 0;
            }
        }
        #endregion POst UPDATE


        #region Get PostsAsync
        public async Task<object> GetPostsAsync(Int64 UserId)
        {
            try
            {
                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";

                List<Posts> trendingSkills = new List<Posts>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "[GetPosts]";
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
                            var trendingSkill = new Posts
                            {
                                CategoryID = (int)(reader["CategoryID"] != DBNull.Value ? Convert.ToInt32(reader["CategoryID"]) : (int?)0),
                                CategoryName = reader["CategoryName"].ToString(),
                                PostID = (int)(reader["PostID"] != DBNull.Value ? Convert.ToInt32(reader["PostID"]) : (int?)0),
                                LikeStatus = (int)(reader["LikeStatus"] != DBNull.Value ? Convert.ToInt32(reader["LikeStatus"]) : (int?)0),
                                PostImage = reader["PostImage"] != DBNull.Value ? baseBannerUrl + reader["PostImage"].ToString() : string.Empty,
                                SkillName = reader["SkillName"].ToString(),
                                PostDescription = reader["PostDescription"].ToString(),
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
                await ErrorLog(1, e.Message, $"Posts", 1);
                return "Error, something went wrong!";
            }
        }

        #endregion

        #region Get All PostsAsync
        public async Task<object> GetAllPostsAsync(int page, int per_page)
        {
            try
            {
                string baseBannerUrl = "https://bookmyskills.co.in/Uploads/";

                List<PostsDetails> trendingSkills = new List<PostsDetails>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "[GetAllPosts]";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@UserId", SqlDbType = SqlDbType.BigInt, Value = ObjUser.UserID });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@Page", SqlDbType = SqlDbType.Int, Value = page });
                    command.Parameters.Add(new SqlParameter() { ParameterName = "@PerPage", SqlDbType = SqlDbType.Int, Value = per_page });

                    _context.Database.OpenConnection();

                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var trendingSkill = new PostsDetails
                            {
                                PostID = (int)(reader["PostID"] != DBNull.Value ? Convert.ToInt32(reader["PostID"]) : (int?)0),
                                LikeStatus = (int)(reader["LikeStatus"] != DBNull.Value ? Convert.ToInt32(reader["LikeStatus"]) : (int?)0),
                                like_count = (int)(reader["like_count"] != DBNull.Value ? Convert.ToInt32(reader["like_count"]) : (long?)0),
                                PostImage = reader["PostImage"] != DBNull.Value ? baseBannerUrl + reader["PostImage"].ToString() : string.Empty,
                                UserProfile = reader["UserProfile"] != DBNull.Value ? baseBannerUrl + reader["UserProfile"].ToString() : string.Empty,
                                PostDescription = reader["PostDescription"].ToString(),
                                PostTitle = reader["PostTitle"].ToString(),
                                FullName = reader["FullName"].ToString(),
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
                await ErrorLog(1, e.Message, $"Posts", 1);
                return "Error, something went wrong!";
            }
        }

        #endregion
    }
}
