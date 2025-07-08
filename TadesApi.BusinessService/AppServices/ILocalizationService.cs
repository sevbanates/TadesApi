using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Db.Entities;
using TadesApi.Db.Entities;

namespace TadesApi.BusinessService.AppServices
{
    public interface ILocalizationService
    {
        //public string GetLocMessage(string msgKey);
        SysStringResource GetStringResource(string resourceKey, int languageId);
    }
}
