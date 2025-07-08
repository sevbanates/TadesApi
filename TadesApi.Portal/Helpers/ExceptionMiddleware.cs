using Newtonsoft.Json;
using System.Net;

namespace TadesApi.Portal.Helpers
{
    public class ErrorResultModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //static readonly ILogger Log = Serilog.Log.ForContext<ExceptionMiddleware>();
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var now = DateTime.UtcNow;
            //Log.Error($"Exception From Middleware {now:HH:mm:ss} : {ex}");
            return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResultModel()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = ex.Message
            }));
        }
    }
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
