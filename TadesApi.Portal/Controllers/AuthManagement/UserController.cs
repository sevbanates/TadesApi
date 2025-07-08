using Microsoft.AspNetCore.Mvc;
using TadesApi.BusinessService.AuthServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.ViewModels.Client;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;

namespace TadesApi.Portal.Controllers.AuthManagement;

[Route("api/users")]
[ApiController]
public class UserController : BaseController
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [SecurityState((int)UsersSecurity.List)]
    [HttpGet]
    public PagedAndSortedResponse<UserBasicDto> GetUsers([FromQuery] UserSearchInput input)
    {
        try
        {
            var response = _service.GetMulti(input);
            response.Token = _appSecurity.Token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new PagedAndSortedResponse<UserBasicDto>(), "GetUsers :" + ex.Message);
        }
    }

    [SecurityState((int)UsersSecurity.View)]
    [HttpGet]
    [Route("{id}/{guidId}")]
    public ActionResponse<UserBasicDto> GetUser(long id, Guid guidId)
    {
        try
        {
            var response = _service.GetSingle(id, guidId);
            response.Token = _appSecurity.Token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<UserBasicDto>(), "GetUser :" + ex.Message);
        }
    }   
    [SecurityState((int)UsersSecurity.View)]
    [HttpGet]
    [Route("{userId}")]
    public ActionResponse<UserBasicDto> GetWebSiteUser(long userId)
    {
        try
        {
            var response = _service.GetWebSiteUser(userId);
            response.Token = _appSecurity.Token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<UserBasicDto>(), "GetWebSiteUser :" + ex.Message);
        }
    }

    [SecurityState((int)UsersSecurity.Save)]
    [HttpPost]
    public ActionResponse<UserBasicDto> CreateUser([FromBody] CreateUserDto input)
    {
        try
        {
            var response = _service.Create(input);
            response.Token = _appSecurity.Token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<UserBasicDto>(), "CreateUser :" + ex.Message);
        }
    }

    [SecurityState((int)UsersSecurity.Save)]
    [HttpPut]
    [Route("{id}")]
    public ActionResponse<UserBasicDto> UpdateUser([FromBody] UpdateUserDto input, long id)
    {
        try
        {
            var response = _service.Update(id, input);
            response.Token = _appSecurity.Token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<UserBasicDto>(), "UpdateUser :" + ex.Message);
        }
    }


    [SecurityState((int)UsersSecurity.Delete)]
    [HttpDelete]
    [Route("{id}/{guidId}")]
    public ActionResponse<bool> DeleteUser(long id, Guid guidId)
    {
        try
        {
            var response = _service.Delete(id, guidId);
            response.Token = _appSecurity.Token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<bool>(), "DeleteUser :" + ex.Message);
        }
    }


    [SecurityState((int)UsersSecurity.View)]
    [HttpGet]
    [Route("profile")]
    public ActionResponse<UserBasicDto> GetProfile()
    {
        try
        {
            var response = _service.GetSingle();
            response.Token = _token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<UserBasicDto>(), "GetProfile :" + ex.Message);
        }
    }


    [HttpPut]
    [Route("profile")]
    public ActionResponse<UserBasicDto> UpdateProfile([FromBody] UpdateProfileDto input)
    {
        try
        {
            var response = _service.UpdateProfile(input);
            response.Token = _appSecurity.Token;
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<UserBasicDto>(), "UpdateProfile :" + ex.Message);
        }
    }


    [HttpPut]
    [Route("password")]
    public ActionResponse<bool> UpdateSelfPassword([FromBody] UpdateSelfPasswordDto input)
    {
        try
        {
            var response = _service.UpdateSelfPassword(input);
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<bool>(), "UpdateSelfPassword :" + ex.Message);
        }
    }

    [SecurityState((int)UsersSecurity.Save)]
    [HttpPut]
    [Route("{userId}/password")]
    public ActionResponse<bool> UpdateOthersPassword([FromBody] UpdateOthersPasswordDto input, long userId)
    {
        try
        {
            var response = _service.UpdateOthersPassword(userId, input);
            return response;
        }
        catch (Exception ex)
        {
            return ErrorResponse(new ActionResponse<bool>(), "UpdateOthersPassword :" + ex.Message);
        }
    }


}