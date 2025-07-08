using TadesApi.BusinessService.AuthServices.Interfaces;
using TadesApi.Core;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.ViewModels.AuthManagement;
using TadesApi.Models.CustomModels;
using TadesApi.Core.Models.CustomModels;

namespace TadesApi.Portal.Controllers.AuthManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysControllerActionRoleController : BaseController
    {
        private readonly ISysControllerActionRoleBusinessService _service;

        public SysControllerActionRoleController(ISysControllerActionRoleBusinessService service)
        {
            _service = service;
        }


        [SecurityState((int)ControllerActionRoleSecurity.View)]
        [HttpGet]
        [Route("role/{roleId}/controllers")]
        public ActionResponse<ControllerModel> GetControllersByRoleId(int roleId)
        {
            try
            {
                var response = _service.GetControllersByRoleId(roleId);
                response.Token = _appSecurity.Token;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<ControllerModel>(), "GetControllersByRoleId :" + ex.Message);
            }
        }

        [SecurityState((int)ControllerActionRoleSecurity.Save)]
        [HttpPut]
        [Route("controller-actions")]
        public ActionResponse<bool> SaveControllerActions([FromBody] ControllerActionSaveModel input)
        {
            try
            {
                var response = _service.SaveControllerActions(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "SaveControllerActions :" + ex.Message);
            }
        }
    }
}