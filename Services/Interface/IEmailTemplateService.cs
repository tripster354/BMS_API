using BudgetManagement.Models;
using System.Threading.Tasks;
using System;
using BMS_API.Services.Interface;

namespace BudgetManagement.Services
{
    public interface IEmailTemplateService : ICommon
    {
        Task<Int64> EmailTemplate_Update(MstEmailTemplate mstEmail);
        Task<string> EmailTemplate_Get(Int64 emailTemplateIDP);
        Task<string> EmailTemplate_GetByTemplateType(Byte EmailTemplateType);
        Task<string> EmailTemplate_GetAll();
    }
}
