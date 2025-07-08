using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalApi.Core;
using VideoPortalApi.Core.Caching;
using VideoPortalApi.Core.Email;
using VideoPortalApi.Core.Masstransit;
using VideoPortalApi.Core.Models.Global;
using VideoPortalApi.Db;
using VideoPortalApi.Db.Entities.AppDbContext;
using VideoPortalApi.Db.Infrastructure;
using VideoPortalApi.QueService.Email.AuthManagement.Interfaces;
using VideoPortalApi.QueService.Email.AuthManagement.Services;
using VideoPortalApi.QueService.Email.Contact.Interfaces;
using VideoPortalApi.QueService.Email.RoomInviteManagement.Interfaces;
using VideoPortalApi.QueService.Email.RoomInviteManagement.Services;
using VideoPortalApi.QueService.Email.ScheduleEventManagement.Interfaces;
using VideoPortalApi.QueService.Email.ScheduleEventManagement.Services;
using VideoPortalApi.RabbitMqService.TaskManager;

namespace VideoPortalApi.RabbitMqService
{
    class Program
    {

        public static IConfigurationRoot Configuration;

        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .CreateLogger();
            try
            {
                // Start!
                MainAsync(args).Wait();
                return 1;

            }
            catch
            {
                return 0;
            }
        }
        static async Task MainAsync(string[] args)
        {
            Log.Information("Creating service collection");
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                Log.Information("Starting service");
                await serviceProvider.GetService<App>().Run(args);
                
                while(true)
                {
                    await Task.Delay(100);
                }

                Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service...");
                Console.Read();
                throw ex;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));


            serviceCollection.AddLogging();

            // Build configuration
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Log.Information($"Environment : {env}");
            var confBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            if (string.IsNullOrEmpty(env))
            {
                confBuilder.AddJsonFile($"appsettings.json", false, true);
            }
            else
            {
                confBuilder.AddJsonFile($"appsettings.{env}.json", false, true);
            }

            confBuilder.AddEnvironmentVariables();
            Configuration = confBuilder.Build();

            string str = "";
            foreach (var provider in Configuration.Providers.ToList())
            {
                str += provider.ToString() + "\n";
            }
            Log.Information(str);

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(Configuration);
            serviceCollection.AddTransient<App>();

            //ConnectionStrings
            ConnectionStrings connectionStrings = new ConnectionStrings();
            Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);
            serviceCollection.AddSingleton<ConnectionStrings>(connectionStrings);

            //AppSettings
            AppConfigs appsettings = new AppConfigs();
            Configuration.GetSection("AppConfigs").Bind(appsettings);
            serviceCollection.AddSingleton<AppConfigs>(appsettings);


            serviceCollection.Configure<FileSettings>(Configuration.GetSection("FileSettings"));
            serviceCollection.Configure<AppConfigs>(Configuration.GetSection("AppConfigs"));
            serviceCollection.AddTransient<IEncryption, Encryption>();

            // Add App
            serviceCollection.AddTransient<App>();

            //DbConnection
            serviceCollection.AddScoped<CacheKeys>();
            serviceCollection.AddDbContext<AdminDbContext>(options => options.UseSqlServer(connectionStrings.DefaultConnection));
            serviceCollection.AddScoped(typeof(IAdminRepository<>), typeof(AdminRepository<>));
            serviceCollection.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionStrings.DefaultConnection));

            serviceCollection.AddTransient<IJobSender, JobSender>();
            serviceCollection.AddTransient<IAuthMailJob, AuthMailJob>();
            serviceCollection.AddTransient<IScheduleEventJob, ScheduleEventJob>();
            serviceCollection.AddTransient<IRoomInviteJob, RoomInviteJob>();

            serviceCollection.AddDistributedRedisCache(option =>
            {
                option.Configuration = appsettings.RedisConnonctionString;
                option.InstanceName = "master";
            });

            serviceCollection.ConsumerExtensions(appsettings);

            //serviceCollection.ConsumerExtensions(appsettings);
            //serviceCollection.AddSingleton<UtiltoDb>();//**Todo






            var serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }

}
