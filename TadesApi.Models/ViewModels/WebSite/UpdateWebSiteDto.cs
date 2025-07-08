using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.WebSite
{
    public class UpdateWebSiteDto : BaseModel, IBaseUpdateModel
    {
        public string WebUrl { get; set; }
        public string Title { get; set; }
        public long? UserId { get; set; }
        public DateTime CreDate { get; set; }
        public int CreUser { get; set; }

        public Guid GuidId { get; set; }
        public DateTime? HostingStartDate { get; set; }
        public DateTime? HostingEndDate { get; set; }
    }
}
