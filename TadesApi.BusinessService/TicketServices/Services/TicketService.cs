using System;
using System.Collections.Generic;
using AutoMapper;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Invoice;
using System.Linq;
public class TicketService : BaseServiceNg<Ticket, TicketDto, CreateTicketDto, UpdateTicketDto>, ITicketService
{
    private readonly IRepository<TicketMessage> _messageRepo;

    public TicketService(IRepository<Ticket> entityRepository, ILocalizationService locManager, IMapper mapper, ICurrentUser session, IRepository<TicketMessage> messageRepo) : base(entityRepository, locManager, mapper, session)
    {
        _messageRepo = messageRepo;
    }

    public ActionResponse<long> CreateTicket(CreateTicketDto dto, long userId, string userEmail)
    {
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = TicketStatus.Open,
            Priority = dto.Priority,
            Category = dto.Category,
            CreatedBy = userId,
            CreatedByEmail = userEmail,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _entityRepository.Insert(ticket);
        return new ActionResponse<long> { IsSuccess = true, Entity = ticket.Id };
    }

    public ActionResponse<bool> AddMessage(TicketMessageDto dto, long senderId, string senderName, string senderEmail,
        string senderType)
    {
        var message = new TicketMessage
        {
            TicketId = dto.TicketId,
            SenderId = senderId,
            SenderName = senderName,
            SenderEmail = senderEmail,
            SenderType = senderType,
            Message = dto.Message,
            Attachments = dto.Attachments != null ? string.Join(",", dto.Attachments) : null,
            CreatedAt = DateTime.Now,
            IsInternal = dto.IsInternal
        };
        _messageRepo.Insert(message);

        // Ticket güncelleme
        var ticket = _entityRepository.GetById(dto.TicketId);
        ticket.UpdatedAt = DateTime.Now;
        _entityRepository.Update(ticket);

        return new ActionResponse<bool> { IsSuccess = true, Entity = true };
    }

    public ActionResponse<bool> ChangeStatus(long ticketId, TicketStatus status)
    {
        var ticket = _entityRepository.GetById(ticketId);
        if (ticket == null)
            return new ActionResponse<bool> { IsSuccess = false, ReturnMessage = new List<string> { "Ticket bulunamadý." } };

        ticket.Status = status;
        ticket.UpdatedAt = DateTime.Now;
        _entityRepository.Update(ticket);

        return new ActionResponse<bool> { IsSuccess = true, Entity = true };
    }

    public ActionResponse<TicketDto> GetTicket(long ticketId)
    {
        var ticket = _entityRepository.TableNoTracking.FirstOrDefault(x => x.Id == ticketId);
        if (ticket == null)
            return new ActionResponse<TicketDto> { IsSuccess = false, ReturnMessage = new List<string> { "Ticket bulunamadý." } };

        // TicketDto'ya map et (AutoMapper veya manuel)
        // ...
        return new ActionResponse<TicketDto> { IsSuccess = true, Entity = new TicketDto() /* mapped ticket */ };
    }

    public PagedAndSortedResponse<TicketDto> GetTickets(PagedAndSortedSearchInput input)
    {
        throw new NotImplementedException();
    }
}