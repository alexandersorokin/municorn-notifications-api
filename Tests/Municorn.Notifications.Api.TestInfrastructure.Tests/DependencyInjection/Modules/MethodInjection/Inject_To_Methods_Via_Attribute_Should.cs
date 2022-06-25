using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.MethodInjection
{
    [TestMethodInjectionModule]
    internal class Inject_To_Methods_Via_Attribute_Should : FrameworkServiceProviderFixtureBase
    {
        public Inject_To_Methods_Via_Attribute_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(new TypeWrapper(typeof(Inject_To_Methods_Via_Attribute_Should)))
                .AddSingleton(new SilentLog()))
        {
        }

        [Test]
        public void Inject([InjectDependency] SilentLog service) => service.Should().NotBeNull();
    }
}
