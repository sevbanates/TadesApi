using Hangfire;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Net;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.AuthServices.Interfaces;
using TadesApi.BusinessService.AuthServices.Services;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.BusinessService.Common.Services;
using TadesApi.BusinessService.CustomerServices.Interfaces;
using TadesApi.BusinessService.CustomerServices.Services;
using TadesApi.BusinessService.InquiryServices.Interfaces;
using TadesApi.BusinessService.InquiryServices.Services;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.BusinessService.InvoiceServices.Services;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.BusinessService.LibraryServices.Services;
using TadesApi.BusinessService.MessageServices.Interfaces;
using TadesApi.BusinessService.MessageServices.Services;
using TadesApi.BusinessService.SettingsService.Interfaces;
using TadesApi.BusinessService.SettingsService.Services;
using TadesApi.BusinessService.TicketServices.Interfaces;
using TadesApi.BusinessService.TicketServices.Services;
using TadesApi.Core;
using TadesApi.Portal;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
//{
//    HostAppId = "MyReportApp",
//    Storage = new FileStorage(), 
//    ReportSourceResolver = new UriReportSourceResolver(Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports"))
//});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(); 


builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", corsBuilder =>
    {
        bool IsOriginAllowed(string origin) => origin switch
        {
            "https://localhost:4200" => true,
            "http://localhost:4200" => true,
            "https://portal.globalpsychsolutions.com" => true,
            "https://meet.globalpsychsolutions.com" => true,
            _ => false
        };

        corsBuilder
            .WithOrigins(
                "https://localhost:4200, http://localhost:4200, https://portal.globalpsychsolutions.com, https://meet.globalpsychsolutions.com")
            .AllowAnyMethod()
            .AllowAnyHeader();
        corsBuilder
            .SetIsOriginAllowed(IsOriginAllowed);
    });
    
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        corsBuilder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin();
    });
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options => { options.UseMemberCasing(); });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AcceptLanguageHeaderFilter>();

    c.AddSecurityDefinition("Bearer", //Name the security scheme
        new OpenApiSecurityScheme
        {
            Description = "Custom Authorization Token Header Using the Bearer scheme.",
            Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
            Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer", //The name of the previously defined security scheme.
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});


//*** Extensions
builder.AddAppServices();

//*** Common Resolvers
builder.Services.AddTransient<IFileHelper, FileHelper>();
builder.Services.AddTransient<IEmailHelper, EmailHelper>();
builder.Services.AddTransient<ICommonService, CommonService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();

//*** AuthManagement Resolvers
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISysControllerActionRoleBusinessService, SysControllerActionRoleBusinessService>();



//*** LibraryService Resolvers
builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<ITicketService, TicketService>();
builder.Services.AddTransient<ILibraryService, LibraryService>();
builder.Services.AddTransient<ISettingsService, SettingsService>();

//*** Inquiry Resolvers
builder.Services.AddTransient<IInquiryService, InquiryService>();

builder.Services.AddTransient<IMessageService, MessageService>();

builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.Configure<SqlLocalizationOptions>(o => o.UseTypeFullNames = true);
builder.Services.TryAdd(new ServiceDescriptor(typeof(IStringLocalizer), typeof(SqlStringLocalizer), ServiceLifetime.Singleton));
builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new("en-US"),
            new("tr-TR"),
            new("de-DE")
        };

        options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });

builder.Services.AddMvc().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).ToList();
        var response = new ActionResponse<bool>
        {
            IsSuccess = false,
            ReturnMessage = errors,
            Entity = false
        };

        return new OkObjectResult(response);
    };
});

var app = builder.Build();

app.UseRequestLocalization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();
app.UseDefaultFiles();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseCors("DefaultPolicy");
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard();
app.UseHangfireServer();
app.Run();