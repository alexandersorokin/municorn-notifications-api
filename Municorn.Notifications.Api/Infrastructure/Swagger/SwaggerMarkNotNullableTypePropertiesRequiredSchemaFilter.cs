using System.Linq;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Municorn.Notifications.Api.Infrastructure.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Municorn.Notifications.Api.Infrastructure.Swagger
{
    [PrimaryConstructor]
    internal partial class SwaggerMarkNotNullableTypePropertiesRequiredSchemaFilter : ISchemaFilter
    {
        private readonly IPropertiesProvider propertiesProvider;
        private readonly JsonSerializerOptions jsonOptions;

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var properties = this.propertiesProvider
                .GetProperties(context.Type)
                .Select(propertyInfo => propertyInfo.Name)
                .Select(name => this.jsonOptions.PropertyNamingPolicy?.ConvertName(name) ?? name)
                .ToHashSet();

            var required = schema.Required;
            var nonNullableProperties = schema.Properties
                .Where(kvp => !kvp.Value.Nullable)
                .Select(kvp => kvp.Key)
                .Where(name => properties.Contains(name))
                .Where(name => !required.Contains(name));

            foreach (var property in nonNullableProperties)
            {
                required.Add(property);
            }
        }
    }
}