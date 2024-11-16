using BMS_API.Services.Interface;
using BudgetManagement.Models.Utility;
using System.Threading.Tasks;
using System;
using BMS_API.Models;

namespace BMS_API.Services.Interface
{
    public interface ITicketIssueTypeService : ICommon
    {
        Task<Int64> TicketIssueType_Insert(TicketIssueType modelTicketIssueType);
        Task<Int64> TicketIssueType_Update(TicketIssueType modelTicketIssueType);
        Task<string> TicketIssueType_Get(Int64 ticketIssueTypeIDP);
        Task<string> TicketIssueType_GetAll(ModelCommonGetAll param);
        Task<Int64> TicketIssueType_ActiveInactive(Int64 ticketIssueTypeIDP, Boolean isActive);
        Task<string> TicketIssueType_DDL();
        Task<Int64> TicketIssueType_Delete(Int64 ticketIssueTypeIDP);
    }
}
