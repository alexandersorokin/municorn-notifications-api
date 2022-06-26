using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureActions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.SetUpFixtures.AfterFixtureConstructorCountChilds
{
    [SetUpFixture]
    [TestMethodInjectionModule]
    [FieldInjectionModule]
    internal sealed class SetUpFixture : IFixtureWithServiceProviderFramework, IDisposable
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection
                .AddSingleton(TestContext.Out)
                .AddSingleton<ITextWriterProvider, AdHocTextWriterProvider>()
                .AddSingleton<ILog, TextWriterLog>()
                .AddSingleton<Counter>()
                .AddSingleton<IFixtureOneTimeSetUpService, FixtureOneTimeTimeLogger>()
                .AddScoped<IFixtureSetUpService, TestTimeLogger>();

        [field: FieldDependency]
        public Counter Counter { get; } = default!;

        public void Dispose()
        {
            this.Counter.Value.Should().Be(7);
        }
    }
}
