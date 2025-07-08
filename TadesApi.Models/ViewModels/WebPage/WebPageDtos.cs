using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.WebPage
{
    public class WebPageDto : BaseModel
    {
        public Guid? GuidId { get; set; }
        public string PageTitle { get; set; }
        public long WebSiteId { get; set; }
        public DateTime CreDate { get; set; }
        public long CreUser { get; set; }
       
    }  
    
    public class CreateWebPageDto
    {
        public Guid? GuidId { get; set; }
        public string PageTitle { get; set; }
        public long WebSiteId { get; set; }
        public DateTime CreDate { get; set; }
        public long CreUser { get; set; }
       
    } 
    public class UpdateWebPageDto : CreateWebPageDto, IBaseUpdateModel
    {
        public Guid GuidId { get; set; }
    }
}
