using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.WebPageContent
{
    public class WebPageContentDto : BaseModel
    {
        public long WebPageId { get; set; }
        public string WebPageGuidId { get; set; }
        public string ContentName { get; set; }
        public string SectionKey { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public DateTime ModifyDate { get; set; }
        public long ModifyUser { get; set; }
    }
}
