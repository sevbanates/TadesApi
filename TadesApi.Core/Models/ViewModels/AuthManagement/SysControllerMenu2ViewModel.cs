using TadesApi.Core.Models.Global;

namespace TadesApi.Core.Models.ViewModels.AuthManagement
{
    public class SysControllerMenu2ViewModel : BaseModel
    {
    

        public new int Id { get; set; }
        public string Controller { get; set; }
        public string Menu { get; set; }


    }

    public class SysControllerMenu2DataViewModel : PagedAndSortedInput
    {
        public string SrcFast { get; set; }
    }
}