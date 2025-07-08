using TadesApi.Db.Entities;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.BusinessService.AppServices
{
    public interface ISecurityService
    {
       

        [Obsolete("This Action mustnt use")]
        bool CheckAuth(string controllerName, string actionName);

        bool CheckAuthorization(string controllerName, int actionNo);

        

    }
}
