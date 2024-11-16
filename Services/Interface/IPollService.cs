using BMS_API.Models;
using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;

namespace BMS_API.Services.Interface
{
    public interface IPollService : ICommon
    {
        Task<Int64> Poll_Insert(Poll modelPoll);
        Task<Int64> Poll_Update(Poll modelPoll);
        Task<string> Poll_Get(Int64 pollIDP);
        Task<string> Poll_GetAll(ModelCommonGetAll param);
        Task<Int64> Poll_ActiveInactive(Int64 pollIDP, Boolean isActive);
        //Task<string> Poll_DDL();
        Task<Int64> Poll_Delete(Int64 pollIDP);
    }
}
