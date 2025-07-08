using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Db
{
    public interface IMigrationService
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}
