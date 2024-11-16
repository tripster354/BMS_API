using BMS_API.Models.Partner;
using BMS_API.Services;
using BMS_API.Services.Interface;
using BMS_API.Services.Interface.User;
using BudgetManagement.Controllers;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using BMS_API.Models;

namespace BMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : CommonController
    {
        private readonly IFeedService _feedService;
        private readonly IWebHostEnvironment webHostEnvironment;
        public FeedController(BMSContext context, IFeedService feedService, IAuthService authService, IWebHostEnvironment webHostEnvironment) : base(context, authService)
        {
            _context = context;
            _feedService = feedService;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region Create-Feed
        [HttpPost]
        [Route("Create-feed")]
        public async Task<ActionResult<long>> CreateFeed(ModelFeedback feed)
        {
            try
            {
                // Assuming you have an instance of IFeedService
                // GetAuth();
                // if(objUser == null)
                // {
                //     return BadRequest(authFail);
                // }

                var newFeed = await _feedService.AddFeedAsync(feed.PartnerIDF, feed.UserIDF, feed.BannerAttachment, feed.Title, feed.ActivityIDF,
                                                    feed.ActData, feed.EntryBy, feed.Comment, feed.NumberLike,0,0,0,0,0,0);

                return Ok(newFeed);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, data = 0, message = ex.Message });
            }
        }

        #endregion

        #region Get-All-Feeds
        [HttpPost]
        [Route("feed-get-all")]
        public async Task<ActionResult> GetAllFeeds(long? partnerId, long? userId, int pageCrr = 1, string searchKeyWord = null)
        {
            try
            {
                // Uncomment and implement your authentication logic if necessary
                // GetAuth();
                // if(objUser == null)
                // {
                //     return BadRequest(authFail);
                // }

                // Fetch feedbacks using the updated GetFeedsAsync method
                var getAllFeedback = await _feedService.GetFeedsAsync(partnerId, userId, pageCrr, searchKeyWord);

                if (getAllFeedback == null || !getAllFeedback.Any())
                {
                    return Ok(new { status = 1, data = getAllFeedback, message = "No feedback found" });
                }
                return Ok(new { status = 1, data = getAllFeedback, message = "Feed retrieved successfully" });
            }
            catch (Exception ex)
            {
                // Return a more detailed error response if necessary
                return BadRequest(new { status = 0, data = 0, error = ex.Message });
            }
        }

        #endregion
    }
}
