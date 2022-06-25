using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor.SetUpFixtures.AfterFixtureConstructorCountChilds
{
    [SetUpFixture]
    [TestMethodInjectionModule]
    [FieldInjectionModule]
    internal sealed class SetUpFixture : IFixtureWithServiceProviderFramework, IDisposable
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddBoundLog()
                .AddFixtureTimeLogger()
                .AddScoped<IFixtureSetUpService, TestTimeLogger>();

        [field: FieldDependency]
        public Counter Counter { get; } = default!;

        public void Dispose()
        {
            this.Counter.Value.Should().Be(7);
        }
    }
}
