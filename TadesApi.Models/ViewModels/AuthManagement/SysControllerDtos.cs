using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;
using TadesApi.Models.Global;

namespace TadesApi.Models.ViewModels.AuthManagement
{
    public class SysControllerViewModel : BaseModel
    {
        public new int Id { get; set; }
        public string ControllerName { get; set; }
        public string Descr { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }

        public List<SysControllerActionViewModel> SysControllerAction { get; set; }
    }

    public class SysControllerDataViewModel : PagedAndSortedInput
    {
        public string SrcFast { get; set; }
    }
}