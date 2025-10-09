using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Yourttoo.Api
{
    public class AcceptLanguageHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            // Si ya existe otro parámetro igual, no lo duplica
            if (operation.Parameters.All(p => p.Name != "Accept-Language"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Accept-Language",
                    In = ParameterLocation.Header,
                    Description = "Idioma del contenido (es-ES, en-US...)",
                    Required = false,
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }
        }
    }
}
