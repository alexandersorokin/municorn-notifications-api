using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    public sealed class FixtureServiceProviderFramework : IAsyncDisposable
    {
        private static readonly ServiceProviderOptions Options = new()
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private readonly ServiceProvider serviceProvider;

        public FixtureServiceProviderFramework(Action<IServiceCollection> configureServices)
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<ScopesManager>()
                .AddSingleton<FixtureOneTimeSetUpRunner>()
                .AddScoped<FixtureSetUpRunner>()
                .AddScoped<TestAccessor>();
            configureServices(serviceCollection);

            this.serviceProvider = serviceCollection.BuildServiceProvider(Options);
        }

        public async ValueTask DisposeAsync() => await this.serviceProvider.DisposeAsync().ConfigureAwait(false);

        internal async Task BeforeTestSuite() =>
            await this.GetRequiredService<FixtureOneTimeSetUpRunner>().RunAsync().ConfigureAwait(false);

        internal async Task BeforeTestCase(ITest test)
        {
            var scopedServiceProvider = this.GetScopesManager().CreateScope(test);
            scopedServiceProvider.GetRequiredService<TestAccessor>().Test = test;
            await scopedServiceProvider.GetRequiredService<FixtureSetUpRunner>().RunAsync().ConfigureAwait(false);
        }

        internal async Task AfterTestCase(ITest test) =>
            await this.GetScopesManager().DisposeScope(test).ConfigureAwait(false);

        private ScopesManager GetScopesManager() => this.GetRequiredService<ScopesManager>();

        private TService GetRequiredService<TService>()
            where TService : notnull =>
            this.serviceProvider.GetRequiredService<TService>();
    }
}
