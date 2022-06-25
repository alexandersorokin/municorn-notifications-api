using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Interfaces;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework
{
    internal sealed class ServiceProviderFramework : IAsyncDisposable
    {
        private static readonly ServiceProviderOptions Options = new()
        {
            ValidateOnBuild = true,
            ValidateScopes = true,
        };

        private readonly ServiceProvider serviceProvider;

        public ServiceProviderFramework(Action<IServiceCollection> configureServices)
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
            await this.serviceProvider
                .GetRequiredService<FixtureOneTimeSetUpRunner>()
                .RunAsync().ConfigureAwait(false);

        internal async Task BeforeTestCase(ITest test)
        {
            var scopedServiceProvider = this.serviceProvider.GetRequiredService<ScopesManager>().CreateScope(test);
            scopedServiceProvider.GetRequiredService<TestAccessor>().Test = test;
            await scopedServiceProvider.GetRequiredService<FixtureSetUpRunner>().RunAsync().ConfigureAwait(false);
        }

        internal async Task AfterTestCase(ITest test) =>
            await this.serviceProvider
                .GetRequiredService<ScopesManager>()
                .DisposeScope(test)
                .ConfigureAwait(false);
    }
}
