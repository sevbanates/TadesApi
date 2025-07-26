using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Models.Global;

namespace TadesApi.Models.CustomModels
{
  
    public class ControllerModel
    {
        public ControllerModel()
        {
            Actions = new();
        }
        public string ControllerName { get; set; }
        public int RoleId { get; set; }
        public List<ControllerActionModel> Actions { get; set; }

    }
    public class ControllerActionModel
    {
        public string ActionName { get; set; }
        public int ActionNo { get; set; }
        public bool? Active { get; set; }

    }   
    
    public class CustomerSelectModel
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Title { get; set; }
        public string VknTckn { get; set; }
        public bool IsCompany { get; set; }

    }
}
