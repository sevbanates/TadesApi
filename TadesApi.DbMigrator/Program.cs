using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using VideoPortalApi.Core;
using VideoPortalApi.Db;
using VideoPortalApi.Db.Entities.AppDbContext;
using VideoPortalApi.Db.SeedData;

namespace VideoPortalApi.DbMigrator
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .WriteTo.Async(c => c.Console())
                .CreateLogger();

            await CreateHostBuilder(args).RunConsoleAsync();

            await Console.Out.WriteLineAsync("Bitti");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(connectionString, b => b.MigrationsAssembly("VideoPortalApi.DbMigrator")));

                    services.Configure<FileSettings>(hostContext.Configuration.GetSection("FileSettings"));

                    services.AddTransient<IMigrationService, AppDbMigrationService<AppDbContext>>();
                    services.AddTransient<IDataSeeder, SysControllerActionRoleSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, SysRoleSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, SysControllerActionSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, UserSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, SysLanguageSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, SysStringResourceSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, ClientSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, SysControllerActionTotalSeeder<AppDbContext>>();
                    services.AddTransient<IDataSeeder, LibrarySeeder<AppDbContext>>();


                    services.AddHostedService<DbMigratorHostedService>();
                }).UseSerilog();
    }
}