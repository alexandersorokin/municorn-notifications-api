using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class AsyncLocalTestCaseServiceResolver
    {
        private readonly IConfigureServices fixture;

        internal TService ResolveService<TService>()
            where TService : notnull =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetFixtureServiceProviderMap()
                .GetScope(this.fixture)
                .ServiceProvider
                .GetRequiredService<TService>();
    }
}
