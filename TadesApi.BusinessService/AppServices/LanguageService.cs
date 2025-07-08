using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Db.Entities;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;

namespace TadesApi.BusinessService.AppServices
{
    public class LanguageService : ILanguageService
    {
        private readonly IRepository<SysLanguage> _languageRepo;

        public LanguageService(IRepository<SysLanguage> languageRepo)
        {
            _languageRepo = languageRepo;
        }

        public IEnumerable<SysLanguage> GetLanguages()
        {
            return _languageRepo.TableNoTracking.ToList();
        }

        public SysLanguage GetLanguageByCulture(string culture)
        {
            return _languageRepo.TableNoTracking.FirstOrDefault(x =>
                x.Culture.Trim().ToLower() == culture.Trim().ToLower());
        }
    }
}
