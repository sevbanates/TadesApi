using TadesApi.Core.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.ViewModels.AuthManagement
{
    public class SysMenuViewModel : BaseModel
    {
        public new int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string MenuPath { get; set; }
        public string ImageSel { get; set; }
        public bool? IsChild { get; set; }
        public bool IsParent { get; set; }
        public string ParentCode { get; set; }
        public int? Module { get; set; }
        public string Status { get; set; }
    }

    public class SysMenuDataViewModel : PagedAndSortedInput
    {
        public string SrcFast { get; set; }
    }
}