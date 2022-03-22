using Microsoft.AspNetCore.Builder;

namespace Municorn.Notifications.Api.Infrastructure
{
    internal static class Pipeline
    {
        internal static void Configure(IApplicationBuilder builder)
        {
            builder
                .UseRouting()
                .UseSwagger()
                .UseSwaggerUI()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
