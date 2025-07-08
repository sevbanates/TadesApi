using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace TadesApi.Portal.ActionFilters
{
    public sealed class LanguageActionFilter : ActionFilterAttribute
    {
        private readonly ILogger logger;
        private readonly IOptions<RequestLocalizationOptions> localizationOptions;

        public LanguageActionFilter(ILoggerFactory loggerFactory, IOptions<RequestLocalizationOptions> options)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            logger = loggerFactory.CreateLogger(nameof(LanguageActionFilter));
            localizationOptions = options;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string culture = context.RouteData.Values["culture"]?.ToString();

            if (!string.IsNullOrWhiteSpace(culture))
            {
                logger.LogInformation($"Setting the culture from the URL: {culture}");

#if DNX46
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
#else
                CultureInfo.CurrentCulture = new CultureInfo(culture);
#endif
            }

            base.OnActionExecuting(context);
        }
    }
}
