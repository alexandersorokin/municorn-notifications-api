﻿using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.Logging;

namespace Municorn.Notifications.Api.Tests.ApiTests
{
    internal static class ServiceCollectionClientFactoryExtensions
    {
        internal static IServiceCollection RegisterClientFactory(this IServiceCollection serviceCollection) =>
            serviceCollection
                .AddContextualLog()
                .RegisterClientTopologyFactory()
                .AddSingleton<ClientFactory>();
    }
}
