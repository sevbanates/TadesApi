using System;
using System.Linq;
using TadesApi.Db.Entities;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;

namespace TadesApi.BusinessService.AppServices
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IRepository<SysStringResource> _localizationRepo;
        private readonly ICurrentUser _session;

        public LocalizationService(IRepository<SysStringResource> localizationRepo, ICurrentUser session)
        {
            _session = session;
            _localizationRepo = localizationRepo;
        }
        // public string GetLocMessage(string msgKey)
        // {
        //     var langId = 1;
        //     //*** TODO 
        //     if (_session != null || _session.LanguageId.IsInitial())
        //     {
        //         langId = _session.LanguageId;
        //     }
        //
        //     SysStringResource sysStringResource = _redisCache.GetLocalizationMessage(msgKey);
        //     if (sysStringResource != null)
        //         return sysStringResource.Value;
        //
        //
        //     var rest = _localizationRepo.TableNoTracking.Where(x => x.SysLanguageId == 1 && x.ResourceKey == msgKey).FirstOrDefault();
        //     if (rest == null)
        //         throw new NotImplementedException();
        //
        //     _redisCache.AddLocalizationMessage(rest, msgKey);
        //     return rest.Value;
        //
        // }

        public SysStringResource GetStringResource(string resourceKey, int languageId)
        {
            return _localizationRepo.TableNoTracking.FirstOrDefault(x =>
                x.ResourceKey.Trim().ToLower() == resourceKey.Trim().ToLower()
                && x.SysLanguageId == languageId);
        }
    }
}
