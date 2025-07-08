using TadesApi.Core;
using TadesApi.Core.Models.ViewModels.AuthManagement;

namespace TadesApi.BusinessService.AuthServices.Interfaces
{
    public interface ILoginService
    {
        ActionResponse<bool> ForgotPassword(string email);
        ActionResponse<LoginUserInfo> SendConfirmCode(LoginModel input);
        ActionResponse<UserViewModel> Login(LoginModel input);
        ActionResponse<bool> ResetPassword(ResetPasswordModel input);
        ActionResponse<bool> FirstLoginChangePassword(FirstLoginChangePasswordModel input);
        ActionResponse<bool> Logout();
    }
}
