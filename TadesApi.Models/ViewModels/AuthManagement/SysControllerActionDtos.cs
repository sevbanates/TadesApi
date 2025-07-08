using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;
using TadesApi.Models.Global;

namespace TadesApi.Models.ViewModels.AuthManagement
{
    public class SysControllerActionViewModel : BaseModel
    {
        public new int Id { get; set; }
        public string ControllerId { get; set; }
        public string ActionName { get; set; }
        public int ActionNo { get; set; }
        public string Code { get; set; }

        public SysControllerViewModel SysController { get; set; }
     
    }
}
