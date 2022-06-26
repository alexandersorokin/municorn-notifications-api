using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureActions.SetUpFixtures.Service
{
    [SetUpFixture]
    internal class SetUpFixture : IWithFields
    {
        public void SetUpServices(IServiceCollection serviceCollection) => serviceCollection
            .AddTestMethodInjection()
            .AddSingleton<MockService>();

        [field: FieldDependency]
        internal MockService Service { get; } = default!;
    }
}
