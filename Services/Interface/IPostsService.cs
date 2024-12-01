using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface IPostsService : ICommon
    {
        Task<object> GetPostsAsync(Int64 UserId);
        Task<Int64> Posts_Update(tblPosts modelActivity);
        Task<Int64> Posts_Insert(tblPosts modelActivity);
        Task<tblPosts> GetPostsById(long PostID);
    }
}
