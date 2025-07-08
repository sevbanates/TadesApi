using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Global
{
    public class ElasticConnectionSettings
    {
        public string ElasticSearchUser { get; set; }
        public string ElasticSearchPass { get; set; }
        public string ElasticSearchHost { get; set; }
        public string ElasticLoginIndex { get; set; }
        public string ElasticErrorIndex { get; set; }
        public string ElasticAuditIndex { get; set; }
    }
}
