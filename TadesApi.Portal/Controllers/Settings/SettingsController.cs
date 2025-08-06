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


    }
}