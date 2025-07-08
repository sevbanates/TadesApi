using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;
using TadesApi.CoreHelper;
using TadesApi.Portal.Helpers;

namespace TadesApi.Portal.Controllers.Common
{
    //[Route("api/[Controller]")]
    [Route("api/UIHelper")]
    [ApiController]
    public class UIHelperController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IOptions<FileSettings> _fileSetting;


        public UIHelperController(ICommonService commonService, IOptions<FileSettings> fileSetting)
        {
            _commonService = commonService;
            _fileSetting = fileSetting;
        }
        [HttpGet]
        [Route("icon-list")]
        public ActionResponse<List<string>> GetIconList()
        {
            ActionResponse<List<string>> returnResponse = new();
            try
            {
                string jsonFile = System.IO.File.ReadAllText(_fileSetting.Value.ResourcePrimeIconsFile);
                List<string> iconNameList = JsonConvert.DeserializeObject<List<string>>(jsonFile);

                returnResponse.Entity = iconNameList;
                returnResponse.Token = _token;
                return returnResponse;
            }
            catch (Exception ex)
            {
                return returnResponse.ReturnResponseError("Hata ! GetIconList : " + ex.Message);
            }
        }
        [HttpGet]
        [Route("button-class-list")]
        public ActionResponse<List<string>> GetButtonClassList()
        {
            ActionResponse<List<string>> returnResponse = new();
            try
            {
                string jsonFile = System.IO.File.ReadAllText(_fileSetting.Value.ResourcePrimeButtonClassFile);
                List<string> btnNameList = JsonConvert.DeserializeObject<List<string>>(jsonFile);

                returnResponse.Entity = btnNameList;
                returnResponse.Token = _token;
                return returnResponse;
            }
            catch (Exception ex)
            {
                return returnResponse.ReturnResponseError("Hata ! GetButtonClassList : " + ex.Message);
            }
        }

    }
}
