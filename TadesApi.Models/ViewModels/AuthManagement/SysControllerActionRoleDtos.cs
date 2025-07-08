using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.AuthManagement
{
    public partial class SysControllerActionRoleViewModel : BaseModel
    {
        public new int Id { get; set; }
        public int RoleId { get; set; }
        public string ActionName { get; set; }
        public int ActionNo { get; set; }
        public string Controller { get; set; }
    }

    public class SysControllerActionRoleDataViewModel : PagedAndSortedInput
    {
        public string Search { get; set; }
    }
}