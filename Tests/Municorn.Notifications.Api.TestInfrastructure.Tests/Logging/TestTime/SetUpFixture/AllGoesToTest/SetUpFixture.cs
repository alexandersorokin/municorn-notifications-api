using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.Logging.TestTime.SetUpFixture.AllGoesToTest
{
    [SetUpFixture]
    internal class SetUpFixture : IFixtureWithServiceProviderFramework
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddContextualLog()
                .AddTestTimeLogger();
    }
}
