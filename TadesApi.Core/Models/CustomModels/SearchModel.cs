using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Security;

namespace TadesApi.Core.Models.CustomModels
{

        [Serializable]
        public class SearchRequest
        {
            public string SrcType { get; set; }
            public string SrcText { get; set; }

        }
        public class SearchResponse
        {
            public SearchResponse()
            {
                DisplayColumns = new List<ColumnInfo>();
            }
            public object EntityList { get; set; }
            public List<ColumnInfo> DisplayColumns { get; set; }


        }
}
