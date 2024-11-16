using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BMS_API.Services.Interface
{
    public interface lBankServices : ICommon
    {
        Task<Int64> Bank_Insert(Bank bank);
        Task<Int64> Bank_Update(Bank bank);
        Task<string> Bank_Get(long BankIDP);
        Task<string> Bank_GetAll(ModelCommonGetAll modelCommonGetAll);
        Task<Int64> Bank_ActiveInactive(Int64 BankIDP, Boolean isActive);
        Task<Int64> Bank_Delete(long BankIPD);
        Task<string> Bank_DDL();

    }
}
