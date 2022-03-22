using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Municorn.Notifications.Api.Infrastructure
{
    public static class NotificationApiApplication
    {
        public static IHost CreateHost(HostSettings hostSettings) =>
            Host
                .CreateDefaultBuilder()
                .ConfigureLogging(builder => hostSettings.ConfigureLogging(builder.ClearProviders()))
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseUrls(hostSettings.Uri.AbsoluteUri)
                    .ConfigureServices(ServiceRegistrar.ConfigureServices)
                    .Configure(Pipeline.Configure)
                    .UseDefaultServiceProvider(options =>
                    {
                        options.ValidateScopes = true;
                        options.ValidateOnBuild = true;
                    }))
                .Build();
    }
}