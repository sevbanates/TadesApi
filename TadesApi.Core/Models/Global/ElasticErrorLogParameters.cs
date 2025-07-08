using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Global
{
    //ElasticSearch'de "error_log" indexli document model tipi içinde arama yapmak için kullanılan parametre class'ıdır.
    public class ElasticErrorLogParameters : ElasticQueryParameters
    {
        public int? ErrorCode { get; set; }
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";
        public string Method { get; set; } = "";
        public string Service { get; set; } = "";
    }
}
