using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface IPageMenuService : ICommon
    {
        Task<Int32> PageMenu_Insert(TblPageMenu tblPageMenu);
        Task<Int32> PageMenu_Update(TblPageMenu tblPageMenu);
        Task<Int32> PageMenu_Delete(Int64 pageMenuIDP);
        Task<string> PageMenu_Get(Int64 pageMenuIDP);
        Task<string> PageMenu_GetAll();
        Task<Int32> PageMenu_Active_InActive(Int64 departmentIDP, Boolean isActive);
        Task<string> PageMenu_GetDetail(string pageMenuName);
    }
}
