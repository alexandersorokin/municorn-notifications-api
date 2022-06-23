using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Fields;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.SetUpFixtures.AfterFixtureConstructorCountChilds
{
    [SetUpFixture]
    [FieldDependenciesModule]
    internal sealed class SetUpFixture : ITestFixture, IDisposable
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddBoundLog()
                .AddFixtureTimeLogger()
                .AddScoped<IFixtureSetUp, TestTimeLogger>();

        [field: FieldDependency]
        public Counter Counter { get; } = default!;

        public void Dispose()
        {
            this.Counter.Value.Should().Be(7);
        }
    }
}
