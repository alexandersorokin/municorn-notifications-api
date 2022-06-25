using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.TestCommunication
{
    [TestCommunicationModule]
    internal class Attribute_Should : FrameworkServiceProviderFixtureBase
    {
        public Attribute_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Attribute_Should))
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddSingleton<SilentLog>())
        {
        }

        [Test]
        public void Provide_Service_Provider_Via_Test() =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this)
                .GetRequiredService<SilentLog>()
                .Should()
                .NotBeNull();
    }
}
