using BudgetManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using BudgetManagement.Models.Utility;

namespace BMS_API.Services.Interface
{
    public interface ISettingsService : ICommon
    {
        Task<string> Admin_GetSettings();
        Task<long> Admin_SMTP(SettingsSMTP settingsSMTP);
        //Task<long> Admin_ScoreRance(SettingsScoreRange scoreRange);
    }
}