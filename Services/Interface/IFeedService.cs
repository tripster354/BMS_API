using BMS_API.Models;
using BMS_API.Models.DTOs;
using BMS_API.Models.Partner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS_API.Services.Interface
{
    public interface IFeedService
    {
        Task<List<FeedDto>> GetFeedsAsync(long? partnerId, long? userId, int pageCrr, string searchKeyWord);
        Task<ModelFeed> GetFeedByIdAsync(long feedId);
        Task<long> AddFeedAsync(
            long? partnerId, long? userId, string banner, string title, long? activityId, List<FeedParticipateDetail> actData, int entryBy, string? comment = null,int feedLike = 0,
            int feedSmile = 0, int feedBook = 0, int feedHeart = 0, int feedNetural = 0, int feedStar = 0, int feedFavourite = 0);
        Task UpdateFeedAsync(ModelFeedback feed);
        Task DeleteFeedAsync(long feedId);
    }
}
