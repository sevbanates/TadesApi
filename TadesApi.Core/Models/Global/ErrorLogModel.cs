using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Global
{
    public class ErrorLogModel
    {
        public int UserID { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }
        public string Service { get; set; }
        public DateTime PostDate { get; set; }
        public int ErrorCode { get; set; }
        public bool IsMobile { get; set; }
        public string UnqDeviceId { get; set; }
    }
}
