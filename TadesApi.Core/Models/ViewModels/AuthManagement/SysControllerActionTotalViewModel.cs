using TadesApi.Core.Models.Global;

namespace TadesApi.Core.Models.ViewModels.AuthManagement
{
    public partial class SysControllerActionTotalViewModel : BaseModel
    {
        public new int Id { get; set; }
        public int Total { get; set; }
        public string Controller { get; set; }
        public int RoleId { get; set; }
    }
    
    public class RoleBasicDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
