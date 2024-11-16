using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS_API.Services.Interface
{
    public interface IStateServices : ICommon
    {
        Task<Int64> State_Insert(Tblstate tblstate);
        Task<Int64> State_Update(Tblstate tblstate);
        Task<string> State_Get(Int64 StateIDP);
        Task<string> State_GetAll(ModelCommonGetAll modelCommonGetAll);
        Task<Int64> State_ActiveInactive(Int64 stateIDP, Boolean isActive);
        Task<Int64> State_Delete(Int64 StateIDP);
        Task<string> State_DDL(Int64 CountryIDF);

    }
}
