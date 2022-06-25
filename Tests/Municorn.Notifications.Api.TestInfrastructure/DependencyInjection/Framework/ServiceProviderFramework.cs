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
                .AddSingleton<FixtureOneTimeSetUpRunner>()
                .AddSingleton<TestCaseScopesManager>()
                .AddScoped<TestAccessor>()
                .AddScoped<FixtureSetUpRunner>();
            configureServices(serviceCollection);

            this.serviceProvider = serviceCollection.BuildServiceProvider(Options);
        }

        public async ValueTask DisposeAsync() => await this.serviceProvider.DisposeAsync().ConfigureAwait(false);

        internal async Task BeforeTestSuite() =>
            await this.serviceProvider
                .GetRequiredService<FixtureOneTimeSetUpRunner>()
                .RunAsync().ConfigureAwait(false);

        internal async Task BeforeTestCase(ITest test) =>
            await this.serviceProvider
                .GetRequiredService<TestCaseScopesManager>()
                .BeforeTestCase(test)
                .ConfigureAwait(false);

        internal async Task AfterTestCase(ITest test) =>
            await this.serviceProvider
                .GetRequiredService<TestCaseScopesManager>()
                .AfterTestCase(test)
                .ConfigureAwait(false);
    }
}
