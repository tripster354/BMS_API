using BMS_API.Models;
using BMS_API.Models.DTOs;
using BMS_API.Models.Partner;
using BMS_API.Services.Interface;
using BudgetManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BMS_API.Services
{
    public class FeedService : IFeedService
    {
        private readonly BMSContext _context;
        private readonly ILogger<FeedService> _logger;
        public FeedService(BMSContext context, ILogger<FeedService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<long> AddFeedAsync(
            long? partnerId, long? userId, string banner, string title, long? activityId, List<FeedParticipateDetail> actData, int entryBy, string? comment = null, int feedLike = 0,
            int feedSmile = 0, int feedBook = 0, int feedHeart = 0, int feedNetural = 0, int feedStar = 0, int feedFavourite = 0)
        {
            var feedIdParam = new SqlParameter
            {
                ParameterName = "@FeedIDP",
                SqlDbType = SqlDbType.BigInt,
                Direction = ParameterDirection.Output
            };

            // Table-valued parameter for Feed_Particpate_Detail (actData)
            var actDataTable = new DataTable();
            actDataTable.Columns.Add("UserID", typeof(long));
            actDataTable.Columns.Add("ContactID", typeof(long));

            foreach (var detail in actData)
            {
                actDataTable.Rows.Add(detail.UserID, detail.ContactID);
            }

            var actDataParam = new SqlParameter
            {
                ParameterName = "@actData",
                TypeName = "DBO.Feed_Particpate_Detail",
                SqlDbType = SqlDbType.Structured,
                Value = actDataTable
            };

            // Execute stored procedure using ExecuteSqlRawAsync
            await _context.Database.ExecuteSqlRawAsync(
                "[dbo].[uspFeed_Insert] @FeedIDP OUTPUT, @PartnerIDF, @UserIDF, @Banner, @Title, @ActivityIDF, @actData, @EntryBy",
                feedIdParam,
                new SqlParameter("@PartnerIDF", (object)partnerId ?? DBNull.Value),
                new SqlParameter("@UserIDF", (object)userId ?? DBNull.Value),
                new SqlParameter("@Banner", (object)banner ?? DBNull.Value),
                new SqlParameter("@Title", (object)title ?? DBNull.Value),
                new SqlParameter("@ActivityIDF", (object)activityId ?? DBNull.Value),
                actDataParam,
                new SqlParameter("@EntryBy", entryBy)
            );

            var feedId = (long)feedIdParam.Value;

            // Insert into tblFeedParticipate for reactions and comments
            foreach (var participant in actData)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    @"INSERT INTO tblFeedParticipate
            (FeedIDF, PartnerIDF, UserIDF, ContactIDF, Comment, FeedLike, FeedSmile, FeedBook, FeedHeart, FeedNetural, FeedStar, FeedFavourite, CreatedDate, IsDeleted, EntryBy, EntryDate)
            VALUES (@FeedIDF, @PartnerIDF, @UserIDF, @ContactIDF, @Comment, @FeedLike, @FeedSmile, @FeedBook, @FeedHeart, @FeedNetural, @FeedStar, @FeedFavourite, DBO.MyCountryCurrentTime(), 0, @EntryBy, DBO.MyCountryCurrentTime())",
                    new SqlParameter("@FeedIDF", feedId),
                    new SqlParameter("@PartnerIDF", partnerId),
                    new SqlParameter("@UserIDF", participant.UserID),
                    new SqlParameter("@ContactIDF", participant.ContactID),
                    new SqlParameter("@Comment", (object)comment ?? DBNull.Value),
                    new SqlParameter("@FeedLike", feedLike),
                    new SqlParameter("@FeedSmile", feedSmile),
                    new SqlParameter("@FeedBook", feedBook),
                    new SqlParameter("@FeedHeart", feedHeart),
                    new SqlParameter("@FeedNetural", feedNetural),
                    new SqlParameter("@FeedStar", feedStar),
                    new SqlParameter("@FeedFavourite", feedFavourite),
                    new SqlParameter("@EntryBy", entryBy)
                );
            }

            return feedId;
        }


        #region Get-All-Feeds
        public async Task<List<FeedDto>> GetFeedsAsync(long? partnerId, long? userId, int pageCrr, string searchKeyWord)
        {
            var feeds = new List<FeedDto>();

            // SQL command to call the stored procedure
            var sqlCommand = "EXEC dbo.uspFeed_GetAll @PartnerIDF, @UserIDF, @PageCrr, @SearchKeyWord";

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlCommand;
                command.CommandType = System.Data.CommandType.Text;

                // Add parameters
                var partnerIdParam = command.CreateParameter();
                partnerIdParam.ParameterName = "@PartnerIDF";
                partnerIdParam.Value = partnerId ?? (object)DBNull.Value; // Handle nullable
                command.Parameters.Add(partnerIdParam);

                var userIdParam = command.CreateParameter();
                userIdParam.ParameterName = "@UserIDF";
                userIdParam.Value = userId ?? (object)DBNull.Value; // Handle nullable
                command.Parameters.Add(userIdParam);

                var pageCrrParam = command.CreateParameter();
                pageCrrParam.ParameterName = "@PageCrr";
                pageCrrParam.Value = pageCrr;
                command.Parameters.Add(pageCrrParam);

                var searchKeyWordParam = command.CreateParameter();
                searchKeyWordParam.ParameterName = "@SearchKeyWord";
                searchKeyWordParam.Value = searchKeyWord ?? (object)DBNull.Value; // Handle nullable
                command.Parameters.Add(searchKeyWordParam);

                await _context.Database.OpenConnectionAsync();

                using (var result = await command.ExecuteReaderAsync())
                {
                    while (await result.ReadAsync())
                    {
                        // Map the result to FeedDto
                        var feed = new FeedDto
                        {
                            FeedID = result.GetInt64(result.GetOrdinal("FeedIDP")),
                            IsOwner = result.GetInt32(result.GetOrdinal("IsOwner")) == 1,
                            BannerImage = result.GetString(result.GetOrdinal("BannerImage")),
                            Title = result.GetString(result.GetOrdinal("Title")),
                            ActivityID = result.GetInt64(result.GetOrdinal("ActivityIDF")),
                            NumberLike = result.GetString(result.GetOrdinal("NumberLike")),
                            NumberSmile = result.GetString(result.GetOrdinal("NumberSmile")),
                            NumberBook = result.GetString(result.GetOrdinal("NumberBook")),
                            NumberHeart = result.GetString(result.GetOrdinal("NumberHeart")),
                            //NumberNetural = result.GetString(result.GetOrdinal("NumberNetural")),
                            NumberStar = result.GetString(result.GetOrdinal("NumberStar")),
                            NumberFavourite = result.GetString(result.GetOrdinal("NumberFavourite")),
                            CreatedDate = result.GetDateTime(result.GetOrdinal("CreatedDate")),
                            FullName = result.GetString(result.GetOrdinal("FullName")),
                            ProfileImage = result.GetString(result.GetOrdinal("ProfileImage")),
                            ActivityTitle = result.GetString(result.GetOrdinal("ActivityTitle")),
                            //InterestIDF = result.GetInt64(result.GetOrdinal("InterestIDF")),
                        };

                        feeds.Add(feed);
                    }
                }
            }

            return feeds;
        }

        #endregion

        public Task DeleteFeedAsync(long feedId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ModelFeed> GetFeedByIdAsync(long feedId)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateFeedAsync(ModelFeedback feed)
        {
            throw new System.NotImplementedException();
        }
    }
}
