using Microsoft.AspNetCore.Mvc;
using TadesApi.BusinessService.SettingsService.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Models.ViewModels.Settings.Accounter;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;
using System;
using Microsoft.AspNetCore.Authorization;

namespace TadesApi.Portal.Controllers.Settings
{
    [ApiController]
    [Route("api/settings")]
    public class SettingsController : BaseController
    {
        private readonly ISettingsService _settingsService;
        private readonly ICurrentUser _currentUser;

        public SettingsController(ISettingsService settingsService, ICurrentUser currentUser)
        {
            _settingsService = settingsService;
            _currentUser = currentUser;
        }

        [HttpPost("create-accounter-request")]
        public ActionResponse<AccounterRequestDto> Create(CreateAccounterRequestDto dto)
        {
            try
            {
                var response = _settingsService.CreateAccounterRequest(dto);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<AccounterRequestDto>(), "Create Error :" + ex.Message);
            }

        }   
        
        [HttpGet("get-requests")]
        public ActionResponse<AccounterRequestDto> GetRequests()
        {
            try
            {
                var response = _settingsService.GetRequests();
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<AccounterRequestDto>(), "GetRequests Error :" + ex.Message);
            }

        }  
        
        [HttpPut("get-requests")]
        public ActionResponse<AccounterRequestDto> ChangeStatus([FromBody] AccounterRequestDto dto)
        {
            try
            {
                var response = _settingsService.ChangeStatus(dto);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<AccounterRequestDto>(), "ChangeStatus Error :" + ex.Message);
            }

        }

        // Email onay linki: /api/settings/accounter-request/approve?id=123&guid=...
        [HttpGet("accounter-request/approve")]
        [AllowAnonymous]
        public ContentResult ApproveAccounterRequest([FromQuery] long id, [FromQuery] Guid guid)
        {
            var dto = new AccounterRequestDto
            {
                Id = id,
                GuidId = guid,
                Status = AccounterRequestStatus.Approved
            };

            var result = _settingsService.ChangeStatus(dto);
            var success = result.IsSuccess;
            var html = $"<html><head><meta charset='utf-8'/><title>İşlem {(success ? "Başarılı" : "Hatalı")}</title></head><body style='font-family:Arial,sans-serif;padding:24px;text-align:center;'><h2>Muhasebeci İsteği {(success ? "Onaylandı" : "Onaylanamadı")}</h2><p>İşlem durumu: {(success ? "Başarılı" : string.Join("<br>", result.ReturnMessage))}</p></body></html>";
            return new ContentResult { Content = html, ContentType = "text/html" };
        }

    }
}