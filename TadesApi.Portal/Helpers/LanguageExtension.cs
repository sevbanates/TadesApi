using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace TadesApi.Portal
{


    public static class LanguageExtension
    {
        public static WebApplicationBuilder ApplyLanguage(this WebApplicationBuilder applicationBuilder)
        {

            //applicationBuilder.Services.AddTransient<ILocalizationService, LocalizationService>();
            //applicationBuilder.Services.AddTransient<ICityService, CityService>();

            applicationBuilder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]{
                                new CultureInfo("tr-TR"),
                                new CultureInfo("en-US"),
                          
                            };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            //set resource path
            applicationBuilder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            return applicationBuilder;
        }


    }
}


