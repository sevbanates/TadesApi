using System;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;

namespace TadesApi.BusinessService.TicketServices.Interfaces
{
    public interface ITicketService : IBaseServiceNg<CreateTicketDto, UpdateTicketDto, TicketDto, PagedAndSortedInput>
    {
        ActionResponse<TicketDto> CreateTicket(CreateTicketDto dto, long userId, string userEmail);

        ActionResponse<bool> AddMessage(CreateTicketMessageDto dto, long senderId, string senderName,
            string senderEmail, string senderType);

        ActionResponse<bool> ChangeStatus(long ticketId, TicketStatus status);
        ActionResponse<TicketDto> GetTicket(long ticketId, Guid guidId);
        PagedAndSortedResponse<TicketDto> GetTickets(TicketSearchInput input);
    }
}