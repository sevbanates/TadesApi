using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.CustomModels;
using TadesApi.Models.ViewModels.Client;
using TadesApi.Models.ViewModels.Inquiry;
using TadesApi.Models.ViewModels.Message;
using TadesApi.Models.ViewModels.ScheduleEvent;
using TadesApi.Portal.Helpers;

namespace TadesApi.Portal.Controllers.Common
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMemoryCache _memoryCache;
        


        public DashboardController(IDashboardService dashboardService, IMemoryCache memoryCache
           )
        {
            _dashboardService = dashboardService;
            _memoryCache = memoryCache;
            
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("clear-cache")]
        public IActionResult ClearCache()
        {
            if (_memoryCache is MemoryCache cache)
            {
                cache.Clear();
            }

            return Accepted();
        }


     



        

        [HttpPost]
        [Route("messages")]
        public PagedAndSortedResponse<MessageWithSenderDto> GetMessages(PagedAndSortedInput input)
        {
            try
            {
                var response = _dashboardService.GetMessages(input);
                response.Token = _token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<MessageWithSenderDto>(), "GetMessages :" + ex.Message);
            }
        }    
    }
}