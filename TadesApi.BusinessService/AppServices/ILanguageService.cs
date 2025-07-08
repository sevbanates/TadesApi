using System.Collections.Generic;
using TadesApi.Db.Entities;
using TadesApi.Db.Entities;

namespace TadesApi.BusinessService.AppServices
{
    public interface ILanguageService
    {
        IEnumerable<SysLanguage> GetLanguages();
        SysLanguage GetLanguageByCulture(string culture);
    }
}
