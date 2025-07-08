using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Db.Entities.AppDbContext;

namespace TadesApi.BusinessService.AppServices
{
    public class SqlStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly ConcurrentDictionary<string, IStringLocalizer> _resourceLocalizations = new ConcurrentDictionary<string, IStringLocalizer>();
        private readonly IServiceProvider _serviceProvider;

        public SqlStringLocalizerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            SqlStringLocalizer sqlStringLocalizer = new SqlStringLocalizer(GetAllFromDatabaseForResource(resourceSource.FullName), resourceSource.FullName);
            _resourceLocalizations.GetOrAdd(resourceSource.FullName, sqlStringLocalizer);
            if (_resourceLocalizations.Keys.Contains(resourceSource.Name))
            {
                return _resourceLocalizations[resourceSource.Name];
            }

            sqlStringLocalizer = new SqlStringLocalizer(GetAllFromDatabaseForResource(resourceSource.Name), resourceSource.Name);
            return _resourceLocalizations.GetOrAdd(resourceSource.Name, sqlStringLocalizer);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            if (_resourceLocalizations.Keys.Contains(baseName + location))
            {
                return _resourceLocalizations[baseName + location];
            }

            var sqlStringLocalizer = new SqlStringLocalizer(GetAllFromDatabaseForResource(baseName + location), baseName + location);
            return _resourceLocalizations.GetOrAdd(baseName + location, sqlStringLocalizer);
        }

        private Dictionary<string, string> GetAllFromDatabaseForResource(string resourceKey)
        {
            Dictionary<string, string> dict;
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dict = context.SysStringResource.Include(s => s.SysLanguage).Where(s => s.ResourceName.StartsWith(resourceKey)).ToDictionary(kvp => (kvp.ResourceKey + "." + kvp.SysLanguage.Culture), kvp => kvp.Value);

            }

            return dict;
        }
    }
}
