using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Infrastructure.Swagger;
using Municorn.Notifications.Api.NotificationFeature.App;
using Municorn.Notifications.Api.NotificationFeature.View;

namespace Municorn.Notifications.Api.Infrastructure
{
    internal static class ServiceRegistrar
    {
        internal static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .RegisterWaiter()
                .RegisterRequestConverter()
                .RegisterSwagger()

                .AddControllers()
                .AddJsonOptions(options =>
                {
                    var converters = options.JsonSerializerOptions.Converters;
                    converters.Add(new JsonStringEnumConverter());
                    converters.Add(new PolymorphicConverter<SendNotificationRequest>());
                })
                .AddFluentValidation(x =>
                {
                    x.ImplicitlyValidateChildProperties = true;
                    x.RegisterValidatorsFromAssemblyContaining<NotificationsController>(lifetime: ServiceLifetime.Singleton);
                });
        }
    }
}