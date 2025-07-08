using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.WebSite
{
    public class WebSiteSearchDto : PagedAndSortedSearchInput
    {
        public string? WebUrl { get; set; }
        public string? Title { get; set; }
        public long? UserId { get; set; }
    }
}
