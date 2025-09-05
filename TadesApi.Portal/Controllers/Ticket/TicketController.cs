using Microsoft.AspNetCore.Mvc;
using TadesApi.BusinessService.TicketServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.Models.ActionsEnum;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;


namespace TadesApi.Portal.Controllers.Ticket
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;
        private readonly ICurrentUser _currentUser;

        public TicketController(ITicketService ticketService, ICurrentUser currentUser)
        {
            _ticketService = ticketService;
            _currentUser = currentUser;
        }

        [SecurityState((int)TicketSecurity.Create)]
        [HttpPost("create")]
        public ActionResponse<TicketDto> Create([FromBody] CreateTicketDto dto)
        {
            try
            {
                var response = _ticketService.CreateTicket(dto, _currentUser.UserId, _currentUser.Email);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<TicketDto>(), "Create Error :" + ex.Message);
            }

        }

        [SecurityState((int)TicketSecurity.Create)]
        [HttpPost("message")]
        public ActionResponse<bool> AddMessage([FromBody] CreateTicketMessageDto dto)
        {
            try
            {
                var response = _ticketService.AddMessage(
                    dto,
                    _currentUser.UserId,
                    _currentUser.UserName,
                    _currentUser.Email,
                    _currentUser.IsAdmin ? "admin" : "user"
                );
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "AddMessage Error :" + ex.Message);
            }

        }

        [SecurityState((int)TicketSecurity.Update)]
        [HttpPut("status/{ticketId}")]
        public ActionResponse<bool> ChangeStatus(long ticketId, [FromBody] TicketStatus status)
        {
            try
            {
                var response = _ticketService.ChangeStatus(ticketId, status);
                response.Token = _appSecurity.Token;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "ChangeStatus Error :" + ex.Message);
            }

        }

        [SecurityState((int)TicketSecurity.List)]
        [HttpGet]
        public PagedAndSortedResponse<TicketDto> GetMulti([FromQuery] TicketSearchInput input)
        {
            try
            {
                var response = _ticketService.GetTickets(input);
                response.Token = _appSecurity.Token;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<TicketDto>(), "GetMulti Error :" + ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}/{guidId}")]
        public ActionResponse<TicketDto> GetSingle(long id, Guid guidId)
        {
            try
            {
                // Admin değilse, kullanıcının kendi verisine erişip erişmediğini kontrol et
                if (_appSecurity.RoleId != 100) // Admin role ID'si 100
                {
                    var ticket = _ticketService.GetTicket(id, guidId);
                    if (!ticket.IsSuccess || ticket.Entity == null)
                    {
                        return ErrorResponse(new ActionResponse<TicketDto>(), "Ticket bulunamadı veya erişim izniniz yok.");
                    }

                    // Kullanıcının kendi ticket'ı mı kontrol et
                    if (ticket.Entity.CreatedBy != _appSecurity.UserId)
                    {
                        return ErrorResponse(new ActionResponse<TicketDto>(), "Bu ticket'a erişim izniniz bulunmamaktadır.");
                    }
                }

                var response = _ticketService.GetTicket(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<TicketDto>(), "GetSingle :" + ex.Message);
            }
        }
    }
}