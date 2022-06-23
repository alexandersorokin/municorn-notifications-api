using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Communicat1ion;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Fields;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.AfterFixtureConstructor.SetUpFixtures.Service
{
    [SetUpFixture]
    [TestMethodInjectionModule]
    internal class SetUpFixture : IWithFields
    {
        public void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton<ILog, SilentLog>();

        [field: FieldDependency]
        internal ILog Service { get; } = default!;
    }
}
