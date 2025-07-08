using TadesApi.Core.Security;
using TadesApi.Portal.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using TadesApi.Core;

namespace TadesApi.Portal.Helpers
{
    [ServiceFilter(typeof(SecurityFilter))]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class BaseController : ControllerBase
    {
        public SecurityModel _appSecurity
        {
            get
            {
                var securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);
                return securityModel;
            }
            private set
            {
            }
        }
        public string _token
        {
            get
            {
                if (_appSecurity.IsRefreshToken)
                {
                    var securityModel = (SecurityModel)(HttpContext.Items["SecurityModel"]);
                    return _appSecurity.Token;
                }
                else
                {
                    return "";
                }
            }
            private set
            {
            }
        }

        public ActionResponse<T> ErrorResponse<T>(ActionResponse<T> response, string message)
        {
            response.IsSuccess = false;
            response.ReturnMessage.Add(message);
            return response;
        }
        
        public PagedAndSortedResponse<T> ErrorResponse<T>(PagedAndSortedResponse<T> response, string message)
        {
            response.IsSuccess = false;
            response.ReturnMessage.Add(message);
            return response;
        }

    }
}
