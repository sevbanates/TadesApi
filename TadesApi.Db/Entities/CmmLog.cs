using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core;

namespace TadesApi.Db.Entities
{
    public class CmmLog : BaseEntity
    {
        public string? AppCode { get; set; }
        public DateTime Date { get; set; }
        public string? Owner { get; set; }
        public string? TableName { get; set; }
        public string? Text { get; set; }
        public string? RecId { get; set; }
        public string? JsonItem { get; set; }
        public Guid GuidId { get; set; }

    }
}
