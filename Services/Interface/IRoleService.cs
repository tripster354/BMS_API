using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface IRoleService : ICommon
    {
        Task<Int64> Role_Insert(Role modelRole);
        Task<Int64> Role_Update(Role modelRole);
        Task<string> Role_Get(Int64 roleIDP);
        Task<string> Role_GetAll(ModelCommonGetAll param);
        Task<Int64> Role_ActiveInactive(Int64 roleIDP, Boolean isActive);
        Task<string> Role_DDL();
        Task<Int64> Role_Delete(Int64 roleIDP);
    }
}
