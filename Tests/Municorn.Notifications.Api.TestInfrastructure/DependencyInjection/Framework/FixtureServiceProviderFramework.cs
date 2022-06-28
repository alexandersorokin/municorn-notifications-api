using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.FixtureServiceProviderManagers;
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
            var scopedServiceProvider = this.manager.CreateScope(test);
            scopedServiceProvider.GetRequiredService<TestAccessor>().Test = test;
            await scopedServiceProvider.GetRequiredService<FixtureSetUpRunner>().RunAsync().ConfigureAwait(false);
        }

        public async Task RunTearDown(ITest test) => await this.manager.DisposeScope(test).ConfigureAwait(false);
    }
}
