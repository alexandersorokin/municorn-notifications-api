using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Municorn.Notifications.Api.Infrastructure.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Municorn.Notifications.Api.Infrastructure.Swagger
{
    [PrimaryConstructor]
    internal partial class SwaggerDataAnnotationSupportOnRecordConstructorSchemaFilter : ISchemaFilter
    {
        private readonly IPrimaryConstructorProvider primaryConstructorProvider;
        private readonly JsonSerializerOptions jsonOptions;

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (type is not null)
            {
                this.CreateTypeInfo(schema, type);
            }

            this.CreateMemberInfo(schema, context.MemberInfo);
        }

        private void CreateMemberInfo(OpenApiSchema schema, MemberInfo? memberInfo)
        {
            if (memberInfo is null)
            {
                return;
            }

            var type = memberInfo.DeclaringType;
            if (type is null)
            {
                return;
            }

            var constructor = this.primaryConstructorProvider.GetPrimaryConstructorParameters(type);
            if (constructor is null)
            {
                return;
            }

            var parameter = constructor.Parameters
                .SingleOrDefault(p => p.Name == memberInfo.Name);
            if (parameter == null)
            {
                return;
            }

            var attributes = parameter.GetCustomAttributes().ToArray();
            schema.ApplyValidationAttributes(attributes);
        }

        private void CreateTypeInfo(OpenApiSchema schema, Type type)
        {
            var constructor = this.primaryConstructorProvider.GetPrimaryConstructorParameters(type);
            if (constructor is null)
            {
                return;
            }

            var requiredParameters = constructor.Parameters
                .Where(p => p.GetCustomAttributes<RequiredAttribute>().Any())
                .Select(p => p.Name)
                .WhereNotNull()
                .Select(name => this.jsonOptions.PropertyNamingPolicy?.ConvertName(name) ?? name);

            foreach (var requiredParameter in requiredParameters)
            {
                if (!schema.Required.Contains(requiredParameter))
                {
                    schema.Required.Add(requiredParameter);
                }
            }
        }
    }
}
