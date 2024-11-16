using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS_API.Services.Interface
{
    public interface ICityServices : ICommon
    {
        Task<Int64> City_Insert(TblCity tblCity);
        Task<Int64> City_Update(TblCity tblCity);
        Task<string> City_Get(Int64 cityIDP);    
        Task<string> City_GetAll(ModelCommonGetAll modelCommonGetAll);
        Task<Int64> City_Delete(Int64 cityIDP);
        Task<Int64> City_ActiveInactive(Int64 stateIDP, Boolean isActive);
        Task<string> City_DDL(Int64 stateIDF);
        Task<string> City_DDL_FindAny(string searchKeyword);

    }
}
