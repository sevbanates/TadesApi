using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Models.ViewModels.Common
{
    public class ControllerActionModel
    {
        public int RoleId { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public bool? IsInquiry { get; set; }
        public bool? IsFree { get; set; }

        //[JsonIgnore]
        //public string GetSetKey
        //{
        //    get
        //    {

        //        return $"c:{ControllerName}:n:{ActionName}";

        //    }
        //}

    }
}
