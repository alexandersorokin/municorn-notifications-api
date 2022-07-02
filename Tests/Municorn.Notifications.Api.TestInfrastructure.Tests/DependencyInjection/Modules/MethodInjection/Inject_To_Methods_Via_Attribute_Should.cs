using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.MethodInjection
{
    [TestMethodInjectionModule]
    internal class Inject_To_Methods_Via_Attribute_Should : FrameworkServiceProviderFixtureBase
    {
        public Inject_To_Methods_Via_Attribute_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Inject_To_Methods_Via_Attribute_Should))
                .AddSingleton<MockService>())
        {
        }

        [Test]
        public void Inject([InjectParameterDependency] MockService service) => service.Should().NotBeNull();
    }
}
