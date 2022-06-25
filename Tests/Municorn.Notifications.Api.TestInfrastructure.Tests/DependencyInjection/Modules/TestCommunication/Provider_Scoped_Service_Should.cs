using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Vostok.Logging.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.TestCommunication
{
    internal class Provider_Scoped_Service_Should : FrameworkServiceProviderFixtureBase
    {
        public Provider_Scoped_Service_Should()
            : base(serviceCollection => serviceCollection
                .AddTestCommunication()
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddSingleton(new SilentLog()))
        {
        }

        [Test]
        public void Provider_Service_Provider_Via_Test() =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this)
                .GetRequiredService<SilentLog>()
                .Should()
                .NotBeNull();

        [Test]
        [Repeat(3)]
        public void Provider_Service_Provider_Via_Test_Repeated() =>
            TestExecutionContext.CurrentContext.CurrentTest
                .GetServiceProvider(this)
                .GetRequiredService<SilentLog>()
                .Should()
                .NotBeNull();
    }
}
