using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureServiceProviderManagers;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    public sealed class FixtureServiceProviderFramework : IAsyncDisposable
    {
        private readonly FixtureServiceProviderManager manager;

        public FixtureServiceProviderFramework(Action<IServiceCollection> configureServices)
        {
            this.manager = new(serviceCollection =>
            {
                serviceCollection
                    .AddSingleton<FixtureOneTimeSetUpRunner>()
                    .AddScoped<FixtureSetUpRunner>()
                    .AddScoped<TestAccessor>();
                configureServices(serviceCollection);
            });
        }

        public async ValueTask DisposeAsync() => await this.manager.DisposeAsync().ConfigureAwait(false);

        public async Task RunOneTimeSetUp() =>
            await this.manager.GetRequiredService<FixtureOneTimeSetUpRunner>().RunAsync().ConfigureAwait(false);

        public async Task RunSetUp(ITest test)
        {
            var serviceProvider = this.manager.CreateScope(test);
            serviceProvider.GetRequiredService<TestAccessor>().Test = test;
            await serviceProvider.GetRequiredService<FixtureSetUpRunner>().RunAsync().ConfigureAwait(false);
        }

        public async Task RunTearDown(ITest test) => await this.manager.DisposeScopeAsync(test).ConfigureAwait(false);
    }
}
