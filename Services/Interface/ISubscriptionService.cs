using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface ISubscriptionService : ICommon
    {
       
        Task<Int64> Subscription_Insert(Subscription modelSubscription);
        Task<Int64> Subscription_Update(Subscription modelSubscription);
        Task<string> Subscription_Get(Int64 subscriptionIDP);
        Task<string> Subscription_GetAll(ModelCommonGetAll param);
        Task<Int64> Subscription_ActiveInactive(Int64 subscriptionIDP, Boolean isActive);
        Task<Int64> Subscription_Delete(Int64 subscriptionIDP);
    }
}
