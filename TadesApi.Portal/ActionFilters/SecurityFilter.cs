using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.AppServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Security;
using TadesApi.CoreHelper;

namespace TadesApi.Portal.ActionFilters
{
    public class SecurityFilter : IAsyncActionFilter
    {
        private ISecurityService _securityService;
        private IUserPreferenceService _userPreferenceService;
        
        public SecurityFilter(ISecurityService securityService, IUserPreferenceService userPreferenceService)
        {
            _securityService = securityService;
            _userPreferenceService = userPreferenceService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var allowAnonymous = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.CustomAttributes.FirstOrDefault(fd => fd.AttributeType == typeof(AllowAnonymousAttribute));
            if(allowAnonymous != null)
            {
                await next();
            }

            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string authHeader = context.HttpContext.Request.Headers["Authorization"];


            var latestToken = authHeader.XSplit(" ")[1].Replace("\"", "");

            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                ClaimsPrincipal principal = new ClaimsPrincipal();

                try
                {
                    principal = TokenManagement.ParseToken(latestToken);


                }
                catch (Exception)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                long userId = int.Parse(principal.FindFirst(ClaimTypes.PrimarySid).Value);

                //*** Kullanıcı başka bilgisayardan login olduysa logout'a atılır.
                //string cacheKey = string.Format(_cacheKeys.SecurityToken, userId);
                //string redisToken = _redisCacheManager.Get<string>(cacheKey);

                //if (redisToken is null || redisToken != latestToken)
                //{
                //    context.Result = new UnauthorizedResult();
                //    _redisCacheManager.Remove(cacheKey);
                //    return;
                //}

                string firstName = principal.FindFirst(ClaimTypes.GivenName).Value;
                string emailAddress = principal.FindFirst(ClaimTypes.Email).Value;
                string username = principal.FindFirst(ClaimTypes.Name).Value;
                int roleId = int.Parse(principal.FindFirst("RoleId").Value);
                var clientId = principal.FindFirst("ClientId")?.Value;

                int.TryParse(context.HttpContext.Request.Headers["TenantId"].FirstOrDefault(), out int tenantId);
                long.TryParse(context.HttpContext.Request.Headers["CompanyId"].FirstOrDefault(), out long companyId);
                int.TryParse(context.HttpContext.Request.Headers["LanguageId"].FirstOrDefault(), out int languageId);


                int timestamp = int.Parse(principal.FindFirst("exp").Value);
                DateTime date = TokenManagement.ConvertFromUnixTimestamp(timestamp);


                var clientsId = !string.IsNullOrEmpty(clientId) ? long.Parse(clientId) : (long?)null;
                var remainingTime = date - DateTime.Now;
                bool isRefToken = false;
                string token = "";
                if (remainingTime.TotalMinutes < 2 && remainingTime.TotalMinutes > 0)
                {
                    isRefToken = true;
                    token = TokenManagement.CreateToken(userId, firstName, emailAddress, username, roleId);
                }
                else if (DateTime.Now > date)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var securityModel = new SecurityModel
                {
                    EmailAddress = emailAddress,
                    FirstName = firstName,
                    UserId = userId,
                    UserName = username,
                    Token = token,
                    TenantId = tenantId,
                    CompanyId = companyId,
                    RoleId = roleId,
                    IsRefreshToken = isRefToken,
                    LanguageId = languageId,
                    ClientId = clientsId
                };


                context.HttpContext.Items["SecurityModel"] = securityModel;

                // User preferences'ları initialize et (accounter için)
                try
                {
                    if (roleId == 102) // Accounter role ID
                    {
                        await Task.Run(() => _userPreferenceService.InitializeSelectedUserFromPreferences());
                    }
                }
                catch
                {
                    // User preferences yüklenememesi critical değil, devam et
                }

                //*** todo kaldırmayın

                //_currentUser.UserId = userId;
                //_currentUser.UserName = username;
                //_currentUser.IsAdmin = roleId == 100;
                //_currentUser.IsSuperUser = roleId == 101;
                //_currentUser.TenantId = tenantId;
                //_currentUser.RoleId = roleId;
                //_currentUser.LanguageId = 1;


               var attr = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.CustomAttributes
                    .FirstOrDefault(fd => fd.AttributeType == typeof(SecurityStateAttribute));

                if (attr != null)
                {
                    int actionNo = (int)attr.ConstructorArguments[0].Value;

                    var controllerName = context.ActionDescriptor.RouteValues["controller"];

                    bool checkAuthorization = _securityService.CheckAuthorization(controllerName, actionNo);


                    if (!checkAuthorization)
                    {
                        //if (actionName.IN(AppConstants.DefActions._List, AppConstants.DefActions._Show))
                        //    context.HttpContext.Response.StatusCode = 425;
                        //else
                        //    context.HttpContext.Response.StatusCode = 422;

                        ActionResponse<object> resp = new();
                        resp.IsSuccess = false;
                        resp.ReturnMessage.Add($"You are not allowed");
                        resp.Entity = null;
                        await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(resp));
                        return;
                    }
                }
                //else
                //{
                //    context.Result = new UnauthorizedResult();
                //    return;

                //}

            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var resultContext = await next();
        }
    }
}
