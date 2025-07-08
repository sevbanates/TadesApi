using TadesApi.Core.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.ViewModels.AuthManagement
{
    public class SysControllerViewModel : BaseModel
    {
        public new int Id { get; set; }
        public string ControllerName { get; set; }
        public string Descr { get; set; }
        public string MenuDescr { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class SysControllerDataViewModel : PagedAndSortedInput
    {
        public string SrcFast { get; set; }
    }
}