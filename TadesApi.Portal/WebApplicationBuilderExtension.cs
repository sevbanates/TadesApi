using AutoMapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using TadesApi.Db.Entities.AppDbContext;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.CommonServices.interfaces;
using TadesApi.BusinessService.CommonServices.services;
using TadesApi.Core;
using TadesApi.Core.Caching;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.Db;
using TadesApi.Db.Infrastructure;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers.Map;

namespace TadesApi.Portal;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder applicationBuilder)
    {
        IConfiguration configuration = applicationBuilder.Configuration;

        //Automapper
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();
        applicationBuilder.Services.AddSingleton(mapper);

        applicationBuilder.Services.AddScoped<SecurityFilter>();


        //applicationBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = "",
        //        ValidAudience = "",
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Betech.EDeclaration.Common.TokenManagement"))
        //    };
        //});
        applicationBuilder.Services.Configure<AppConfigs>(applicationBuilder.Configuration.GetSection("AppConfigs"));


        var connectionStrings = new ConnectionStrings();
        configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

        var appsettings = new AppConfigs();
        applicationBuilder.Configuration.GetSection("AppConfigs").Bind(appsettings);
        applicationBuilder.Services.AddSingleton(appsettings);

        applicationBuilder.Services.AddHttpContextAccessor();
        //applicationBuilder.Services.AddSingleton<ITenantIdentifier, UrlTenantIdentifier>();
        applicationBuilder.Services.AddTransient<ICurrentUser, CurrentUser>();


        applicationBuilder.Services.AddScoped<CacheKeys>();
        applicationBuilder.Services.AddDbContext<AdminDbContext>(options => options.UseSqlServer(connectionStrings.DefaultConnection));
        applicationBuilder.Services.AddScoped(typeof(IAdminRepository<>), typeof(AdminRepository<>));
        applicationBuilder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionStrings.DefaultConnection, b => b.MigrationsAssembly("TadesApi.Db"))
                .EnableDetailedErrors()
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
        );
        applicationBuilder.Services.AddHangfire(x =>
            x.UseSqlServerStorage(applicationBuilder.Configuration.GetConnectionString("HangFireeConnectionString")));
        applicationBuilder.Services.AddDbContext<BtcDbContext>();
        applicationBuilder.Services.AddScoped<IBetechContextProvider, BetechContextProvider>();
        applicationBuilder.Services.AddScoped(typeof(IRepository<>), typeof(GeneralRepository<>));

        //** Redis
        // applicationBuilder.Services.AddSingleton<RedisServer>();
        // applicationBuilder.Services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
        // applicationBuilder.Services.AddTransient<IRedisFuncs, RedisFuncs>();
        // applicationBuilder.Services.AddTransient<IRedisFuncService, RedisFuncService>();
        // applicationBuilder.Services.AddTransient<IRedisCacheService, RedisCacheService>();

        applicationBuilder.Services.Configure<FileSettings>(applicationBuilder.Configuration.GetSection("FileSettings"));
        applicationBuilder.Services.Configure<AppConfigs>(applicationBuilder.Configuration.GetSection("AppConfigs"));
        applicationBuilder.Services.Configure<AwsSettings>(applicationBuilder.Configuration.GetSection("AwsSettings"));
        

        applicationBuilder.Services.AddTransient<IEncryption, Encryption>();

        applicationBuilder.Services.AddMemoryCache();
        
        //*** Single Class
        applicationBuilder.Services.AddTransient<ISecurityService, SecurityService>();
        
        applicationBuilder.Services.AddTransient<ILocalizationService, LocalizationService>();
        applicationBuilder.Services.AddTransient<IQueueService, QueueService>();
        applicationBuilder.Services.AddTransient<IJobService, JobService>();

        applicationBuilder.Services.AddSwaggerGen(c =>
        {
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.IgnoreObsoleteActions();
            c.IgnoreObsoleteProperties();
            c.CustomSchemaIds(type => type.FullName);
        });
        
        
        return applicationBuilder;
    }
}