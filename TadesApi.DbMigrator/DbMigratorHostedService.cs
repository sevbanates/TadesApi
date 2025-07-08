using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VideoPortalApi.Db;

namespace VideoPortalApi.DbMigrator
{
    public class DbMigratorHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        readonly ILogger<DbMigratorHostedService> _logger;
        readonly IServiceScopeFactory _scopeFactory;
        public DbMigratorHostedService(IServiceScopeFactory scopeFactory, ILogger<DbMigratorHostedService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Applying migrations");

            var scope = _scopeFactory.CreateScope();

            try
            {

                var migrationServiceList = scope.ServiceProvider.GetServices<IMigrationService>();
                foreach (var migrationService in migrationServiceList)
                {
                    await migrationService.StartAsync(cancellationToken);
                }
                _hostApplicationLifetime.StopApplication();
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                    await asyncDisposable.DisposeAsync();
                else
                    scope.Dispose();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
