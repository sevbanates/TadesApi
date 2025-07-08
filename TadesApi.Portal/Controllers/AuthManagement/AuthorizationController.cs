using TadesApi.BusinessService.AuthServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using Microsoft.AspNetCore.Mvc;

namespace TadesApi.Portal.Controllers.AuthManagement
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILoginService _authService;

        public AuthorizationController(ILoginService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("send-confirm-code")]
        public ActionResponse<LoginUserInfo> SendConfirmCode(LoginModel input)
        {
            try
            {
                var response = _authService.SendConfirmCode(input);
                return response;
            }

            catch (Exception ex)
            {
                return new ActionResponse<LoginUserInfo>
                {
                    IsSuccess = false,
                    ReturnMessage = new List<string> { ex.Message }
                };
            }
        }

        [HttpPost]
        [Route("login")]
        public ActionResponse<UserViewModel> Login([FromBody] LoginModel input)
        {
            try
            {
                var response = _authService.Login(input);
                return response;
            }

            catch (Exception ex)
            {
                return new ActionResponse<UserViewModel>
                {
                    IsSuccess = false,
                    ReturnMessage = new List<string> { ex.Message }
                };
            }
        }

        [HttpPost]
        [Route("forgot-password")]
        public ActionResponse<bool> ForgotPassword(ForgotPasswordModel input)
        {
            try
            {
                var response = _authService.ForgotPassword(input.Email);
                return response;
            }

            catch (Exception ex)
            {
                return new ActionResponse<bool>
                {
                    IsSuccess = false,
                    ReturnMessage = new List<string> { ex.Message }
                };
            }
        }

        [HttpPut]
        [Route("reset-password")]
        public ActionResponse<bool> ResetPassword(ResetPasswordModel input)
        {
            try
            {
                var response = _authService.ResetPassword(input);
                return response;
            }

            catch (Exception ex)
            {
                return new ActionResponse<bool>
                {
                    IsSuccess = false,
                    ReturnMessage = new List<string> { ex.Message }
                };
            }
        }

        [HttpPut]
        [Route("change-password")]
        public ActionResponse<bool> FirstLoginChangePassword(FirstLoginChangePasswordModel input)
        {
            try
            {
                var response = _authService.FirstLoginChangePassword(input);
                return response;
            }

            catch (Exception ex)
            {
                return new ActionResponse<bool>
                {
                    IsSuccess = false,
                    ReturnMessage = new List<string> { ex.Message }
                };
            }
        }

        [HttpGet]
        [Route("logout")]
        public ActionResponse<bool> Logout()
        {
            try
            {
                var response = _authService.Logout();
                return response;
            }

            catch (Exception ex)
            {
                return new ActionResponse<bool>
                {
                    IsSuccess = false,
                    ReturnMessage = new List<string> { ex.Message }
                };
            }
        }
    }
}