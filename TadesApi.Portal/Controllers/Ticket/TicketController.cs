using Microsoft.AspNetCore.Mvc;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;

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

    [HttpPost("create")]
    public ActionResponse<long> Create([FromBody] CreateTicketDto dto)
    {
        var response = _ticketService.CreateTicket(dto, _currentUser.UserId, _currentUser.Email);
        return response;
    }

    [HttpPost("message")]
    public ActionResponse<bool> AddMessage([FromBody] TicketMessageDto dto)
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

    [HttpPut("status/{ticketId}")]
    public ActionResponse<bool> ChangeStatus(long ticketId, [FromBody] TicketStatus status)
    {
        var response = _ticketService.ChangeStatus(ticketId, status);
        return response;
    }

    [HttpGet]
    public PagedAndSortedResponse<TicketDto> GetMulti([FromQuery] PagedAndSortedSearchInput input)
    {
        try
        {
            var response = _ticketService.GetMulti(input);
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