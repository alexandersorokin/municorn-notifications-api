using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.FixtureServiceProviderManagers
{
    internal sealed class FixtureServiceProviderManager : IServiceProvider, IAsyncDisposable
    {
        private static readonly ServiceProviderOptions Options = new()
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private readonly ServiceProvider serviceProvider;

        internal FixtureServiceProviderManager(Action<IServiceCollection> configureServices)
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<FixtureServiceProviderScopesManager>();
            configureServices(serviceCollection);

            this.serviceProvider = serviceCollection.BuildServiceProvider(Options);
        }

        public async ValueTask DisposeAsync() => await this.serviceProvider.DisposeAsync().ConfigureAwait(false);

        public object GetService(Type serviceType) => this.serviceProvider.GetService(serviceType);

        internal IServiceProvider CreateScope(ITest test) => this.GetScopesManager().CreateScope(test);

        internal async Task DisposeScope(ITest test) => await this.GetScopesManager().DisposeScope(test).ConfigureAwait(false);

        private FixtureServiceProviderScopesManager GetScopesManager() =>
            this.serviceProvider.GetRequiredService<FixtureServiceProviderScopesManager>();
    }
}
