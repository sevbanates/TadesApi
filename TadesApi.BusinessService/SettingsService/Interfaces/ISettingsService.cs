using System;
using System.Collections.Generic;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Settings.Accounter;

namespace TadesApi.BusinessService.SettingsService.Interfaces
{ 
    public interface ISettingsService
    {
        ActionResponse<AccounterRequestDto> CreateAccounterRequest(CreateAccounterRequestDto dto); 
        ActionResponse<AccounterRequestDto> GetRequests();
        ActionResponse<AccounterRequestDto> ChangeStatus(AccounterRequestDto dto);
        //ActionResponse<bool> AddMessage(CreateTicketMessageDto dto, long senderId, string senderName, string senderEmail, string senderType);
        //ActionResponse<bool> ChangeStatus(long ticketId, TicketStatus status);
        //ActionResponse<TicketDto> GetTicket(long ticketId, Guid guidId);
        //PagedAndSortedResponse<TicketDto> GetTickets(PagedAndSortedSearchInput input);
    }
}