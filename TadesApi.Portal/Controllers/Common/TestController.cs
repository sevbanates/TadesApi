
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TadesApi.Core;
using TadesApi.Core.Models.Security;
using TadesApi.Models.Global;
using TadesApi.Portal.Helpers;

namespace TadesApi.Portal.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        private readonly IStringLocalizer<AppGlobalResource> _localizer;

        public TestController(IStringLocalizer<AppGlobalResource> localizer )
        {
            _localizer = localizer;

        }

        [HttpPost]
        [Route("GetData")]
        public ActionResponse<InquirySettings> GetData(TestModel testModel)
        {
            var response = new ActionResponse<InquirySettings>();
            
            response.ReturnMessage.Add(_localizer["test"]);
            return response;
        }

    }
}
