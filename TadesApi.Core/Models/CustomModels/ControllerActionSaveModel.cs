using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.CustomModels
{
    public record ControllerActionSaveModel
    {
        public  string ControllerName { get; init; }
        public int RoleId { get; init; }
        public List<ControllerActionCustomModel> Actions { get; init; }
    }

    public record ControllerActionCustomModel
    {
        public string ActionName { get; init; }
        public int ActionNo { get; init; }
        public bool Active { get; init; }
    }
}
