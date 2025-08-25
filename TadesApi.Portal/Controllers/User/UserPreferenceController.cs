using Microsoft.AspNetCore.Mvc;
using TadesApi.BusinessService.AppServices.Interfaces;
using TadesApi.Core;
using TadesApi.Models.ViewModels.User;
using TadesApi.Portal.Helpers;

namespace TadesApi.Portal.Controllers.User
{
    [ApiController]
    [Route("api/user-preferences")]
    public class UserPreferenceController : BaseController
    {
        private readonly IUserPreferenceService _userPreferenceService;

        public UserPreferenceController(IUserPreferenceService userPreferenceService)
        {
            _userPreferenceService = userPreferenceService;
        }

        /// <summary>
        /// Accounter'ın erişebileceği kullanıcıları getirir
        /// </summary>
        [HttpGet("accessible-users")]
        public ActionResponse<AccounterUserSelectionResponseDto> GetAccessibleUsers()
        {
            try
            {
                var response = _userPreferenceService.GetAccessibleUsers();
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<AccounterUserSelectionResponseDto>(), 
                    "GetAccessibleUsers Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Accounter'ın seçili kullanıcısını değiştirir
        /// </summary>
        [HttpPost("set-selected-user")]
        public ActionResponse<bool> SetSelectedUser([FromBody] SetSelectedUserDto dto)
        {
            try
            {
                var response = _userPreferenceService.SetSelectedUser(dto);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), 
                    "SetSelectedUser Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Mevcut seçili kullanıcı ID'sini getirir
        /// </summary>
        [HttpGet("selected-user-id")]
        public ActionResponse<long?> GetSelectedUserId()
        {
            try
            {
                var response = _userPreferenceService.GetSelectedUserId();
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<long?>(), 
                    "GetSelectedUserId Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Login sırasında kullanıcı tercihlerini yükler
        /// </summary>
        [HttpPost("initialize-selected-user")]
        public ActionResponse<bool> InitializeSelectedUser()
        {
            try
            {
                var response = _userPreferenceService.InitializeSelectedUserFromPreferences();
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), 
                    "InitializeSelectedUser Error: " + ex.Message);
            }
        }
    }
}
