using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
public class TicketService : BaseServiceNg<Ticket, TicketDto, CreateTicketDto, UpdateTicketDto>, ITicketService
{
    private readonly IRepository<TicketMessage> _messageRepo;
    private readonly IRepository<User> _userRepo;

    public TicketService(IRepository<Ticket> entityRepository, ILocalizationService locManager, IMapper mapper, ICurrentUser session, IRepository<TicketMessage> messageRepo, IRepository<User> userRepo) : base(entityRepository, locManager, mapper, session)
    {
        _messageRepo = messageRepo;
        _userRepo = userRepo;
    }

    public ActionResponse<long> CreateTicket(CreateTicketDto dto, long userId, string userEmail)
    {
        var date = DateTime.Now;
        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = TicketStatus.Open,
            Priority = dto.Priority,
            Category = dto.Category,
            CreatedBy = userId,
            CreatedByEmail = userEmail,
            CreatedAt = date,
            UpdatedAt = date
        };
        _entityRepository.Insert(ticket);

        var user = _userRepo.TableNoTracking.FirstOrDefault(x => x.Id == userId);
        var ticketMessage = new TicketMessage
        {
            CreatedAt = date,
            Message = dto.Message,
            TicketId = ticket.Id,
            Ticket = ticket,
            SenderId = userId,
            SenderEmail = userEmail,
            SenderName = user.FirstName + " " + user.LastName,
            SenderType = "_session.IsAccounter ? \"Muhasebeci\" : \"Kullanýcý\""

        };

        _messageRepo.Insert(ticketMessage);
        
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

    public ActionResponse<TicketDto> GetTicket(long ticketId, Guid guidId)
    {
        var ticket = _entityRepository.TableNoTracking.Include(x=> x.Messages).FirstOrDefault(x => x.Id == ticketId && x.GuidId == guidId);
        if (ticket == null)
            return new ActionResponse<TicketDto> { IsSuccess = false, ReturnMessage = new List<string> { "Ticket bulunamadý." } };

        // TicketDto'ya map et (AutoMapper veya manuel)
        // ...
        return new ActionResponse<TicketDto> { IsSuccess = true, Entity = _mapper.Map<TicketDto>(ticket) /* mapped ticket */ };
    }

    public PagedAndSortedResponse<TicketDto> GetTickets(PagedAndSortedSearchInput input)
    {
        throw new NotImplementedException();
    }
}