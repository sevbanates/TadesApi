using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TadesApi.Portal.ActionFilters
{
    public class AcceptLanguageHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null)
            {
                throw new Exception("Invalid operation");
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = "accept-language",
                Description = "pass the locale here: examples like => en-US, tr-TR, de-DE",
                Schema = new OpenApiSchema { Type = "String" },
                Example = new OpenApiString("en-US")
            });
        }
    }
}
