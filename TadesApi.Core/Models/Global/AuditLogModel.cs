using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Global
{
    public record AuditLogModel(long UserID, string JsonModel, string ClassName, string Operation, DateTime PostDate, bool IsMobile, string UnqDeviceId);

}
