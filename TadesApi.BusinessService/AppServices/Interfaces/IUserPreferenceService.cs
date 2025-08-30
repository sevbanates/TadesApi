using TadesApi.Core;
using TadesApi.Models.ViewModels.User;

namespace TadesApi.BusinessService.AppServices.Interfaces
{
    public interface IUserPreferenceService
    {
        ActionResponse<AccounterUserSelectionResponseDto> GetAccessibleUsers();
        ActionResponse<bool> SetSelectedUser(SetSelectedUserDto dto);
        ActionResponse<long?> GetSelectedUserId();
        ActionResponse<bool> InitializeSelectedUserFromPreferences();
    }
}




