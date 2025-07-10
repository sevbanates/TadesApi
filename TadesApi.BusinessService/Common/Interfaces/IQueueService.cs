
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Security;

namespace TadesApi.BusinessService.CommonServices.interfaces
{
    public interface IQueueService
    {
        void AddLog<T>(T entity, string message, SecurityModel securityModel);
        //void CheckImzager();
        //void CalculateDelaerOrder();
    }
}
