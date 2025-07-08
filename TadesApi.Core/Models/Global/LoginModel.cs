using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Global
{
    public class LoginLogModel
    {
        public string UserID { get; set; }
        public DateTime PostDate { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsServiceUser { get; set; }
        public bool IsMobile { get; set; }
        public string UnqDeviceId { get; set; }


    }
}
