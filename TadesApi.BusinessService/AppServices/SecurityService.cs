
using TadesApi.Db.Entities;
using System.Collections.Generic;
using System.Linq;
using TadesApi.Core.Session;
using TadesApi.Db.Infrastructure;
using System;
using TadesApi.Core.Caching;
using Microsoft.Extensions.Caching.Memory;
using TadesApi.Db.Entities;

namespace TadesApi.BusinessService.AppServices
{
    public class SecurityService : ISecurityService
    {
        private readonly ICurrentUser _session;
        private readonly IRepository<SysControllerActionRole> _controllerActionRoleRepo;
        private readonly IRepository<SysControllerActionTotal> _controllerActionTotalRepo;

        private readonly CacheKeys _cacheKeys;
        private IMemoryCache _cache;

        public SecurityService(ICurrentUser session, IRepository<SysControllerActionRole> controllerActionRole, CacheKeys cacheKeys, IMemoryCache cache, IRepository<SysControllerActionTotal> controllerActionTotalRepo)
        {
            _session = session;
            _cacheKeys = cacheKeys;
            _controllerActionRoleRepo = controllerActionRole;
            _cache = cache;
            _controllerActionTotalRepo = controllerActionTotalRepo;
        }

        public bool CheckAuthorization(string controllerName, int actionNo)
        {
            if (_session.IsAdmin)
                return true;

            string controllerActionTotalKey = string.Format(_cacheKeys.ControllerActionTotal, controllerName, _session.RoleId);

            if (!_cache.TryGetValue(controllerActionTotalKey, out SysControllerActionTotal totalActionRec))
            {

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                totalActionRec = _controllerActionTotalRepo.TableNoTracking.FirstOrDefault(x => x.Controller == controllerName && x.RoleId == _session.RoleId);

                if (totalActionRec == null)
                {
                    return false;
                }

                _cache.Set(controllerActionTotalKey, totalActionRec, cacheEntryOptions);

            }

            //*** BITWISE
            if (actionNo == (totalActionRec.Total & actionNo))
            {
                return true;
            }
            return false;
        }


        public bool CheckAuth(string controllerName, string actionName)
        {
            if (_session.IsAdmin)
                return true;
            var result = _controllerActionRoleRepo.Table.Any(s => s.Controller == controllerName && s.ActionName == actionName && s.RoleId == _session.RoleId);
            return result;
        }


    }
}
