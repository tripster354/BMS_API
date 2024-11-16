using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS_API.Services.Interface
{
    public interface lCountryServices : ICommon
    {
        Task<Int64> Country_Insert(TblCountry tblCountry);
        Task<Int64> Country_Update(TblCountry tblCountry);
        Task<string> Country_Get(long countryIDP);
        Task<string> Country_GetAll(ModelCommonGetAll modelCommonGetAll);
        Task<Int64> Country_ActiveInactive(Int64 countryIDP, Boolean isActive);
        Task<Int64> Country_Delete(long countryIPD);
        Task<string> Country_DDL();
        Task<string> CountryCurrency_DDL();

    }
}
