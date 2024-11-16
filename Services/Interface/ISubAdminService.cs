using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;
using BMS_API.Models.Utility;

namespace BMS_API.Services.Interface
{
    public interface ISubAdminService : ICommon
    {
        Task<Int64> SubAdmin_Insert(SubAdmin modelSubAdmin, string connection);
        Task<Int64> SubAdmin_Update(SubAdmin modelSubAdmin, string connection);
        Task<string> SubAdmin_Get(Int64 subAdminIDP);
        Task<string> SubAdmin_GetAll(ModelCommonGetAll param);
        Task<Int64> SubAdmin_ActiveInactive(Int64 subAdminIDP, Boolean isActive);
        Task<Int64> SubAdmin_Delete(Int64 subAdminIDP);
        Task<string> SubAdmin_Get_Permission_By_ModuleIDF(Model_Get_Permission paramGetPermission);
    }
}
