using System;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Invoice;

public interface ITicketService : IBaseServiceNg<CreateTicketDto, UpdateTicketDto, TicketDto, PagedAndSortedInput>
{
    ActionResponse<long> CreateTicket(CreateTicketDto dto, long userId, string userEmail);
    ActionResponse<bool> AddMessage(TicketMessageDto dto, long senderId, string senderName, string senderEmail, string senderType);
    ActionResponse<bool> ChangeStatus(long ticketId, TicketStatus status);
    ActionResponse<TicketDto> GetTicket(long ticketId, Guid guidId);
    PagedAndSortedResponse<TicketDto> GetTickets(PagedAndSortedSearchInput input);
}