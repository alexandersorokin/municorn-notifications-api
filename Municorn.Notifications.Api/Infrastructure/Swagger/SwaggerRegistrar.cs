using System.Linq;
using System.Reflection;
using MicroElements.Swashbuckle.FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Municorn.Notifications.Api.Infrastructure.Reflection;

namespace Municorn.Notifications.Api.Infrastructure.Swagger
{
    internal static class SwaggerRegistrar
    {
        internal static IServiceCollection RegisterSwagger(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton<IPropertiesProvider, CachedPropertiesProvider>()
                .AddSingleton<IPrimaryConstructorProvider, PrimaryConstructorProvider>()
                .AddSwaggerGen(
                    options =>
                    {
                        options.SwaggerDoc(
                            "v1",
                            new OpenApiInfo
                            {
                                Title = "Notifications API",
                                Version = "v1",
                            });

                        // Support nullable flag on all objects and enums
                        options.UseAllOfToExtendReferenceSchemas();

                        // Respect nullable C# 8 annotation for nullable schema property
                        options.SupportNonNullableReferenceTypes();

                        // Mark not nullable reference & value type properties as required
                        options.SchemaFilter<SwaggerMarkNotNullableTypePropertiesRequiredSchemaFilter>();

                        // Support data annotation validation attributes on positional record constructors
                        options.SchemaFilter<SwaggerDataAnnotationSupportOnRecordConstructorSchemaFilter>();

                        options.UseOneOfForPolymorphism();
                        options.SelectDiscriminatorNameUsing(baseType =>
                        {
                            return baseType.Assembly.DefinedTypes
                                .Where(type => type.IsAssignableTo(baseType))
                                .Select(assignableTypes => assignableTypes.GetCustomAttribute<DiscriminatorAttribute>())
                                .Any(attribute => attribute != null)
                                ? PolymorphicConverter.PropertyName
                                : null;
                        });
                        options.SelectDiscriminatorValueUsing(subType => subType.GetCustomAttribute<DiscriminatorAttribute>()?.Value);
                    })

                .AddFluentValidationRulesToSwagger()
                .AddSingleton<FluentValidationRule, NotEmptyFluentValidationRule>();
    }
}
