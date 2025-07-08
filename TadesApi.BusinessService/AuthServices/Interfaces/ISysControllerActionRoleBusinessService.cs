using TadesApi.Core;
using TadesApi.Core.Models.CustomModels;
using TadesApi.Models.CustomModels;

namespace TadesApi.BusinessService.AuthServices.Interfaces
{
    public interface ISysControllerActionRoleBusinessService 
    {
        ActionResponse<ControllerModel> GetControllersByRoleId(int roleId);
        ActionResponse<bool> SaveControllerActions(ControllerActionSaveModel listModel);

    }
}
