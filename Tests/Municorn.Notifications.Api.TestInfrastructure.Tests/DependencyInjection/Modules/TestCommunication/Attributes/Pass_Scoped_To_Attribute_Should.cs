using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.TestCommunication;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.TestCommunication.Attributes
{
    internal class Pass_Scoped_To_Attribute_Should : FrameworkServiceProviderFixtureBase
    {
        public Pass_Scoped_To_Attribute_Should()
            : base(serviceCollection => serviceCollection
                .AddTestCommunication()
                .AddSingleton<ITest>(TestExecutionContext.CurrentContext.CurrentTest)
                .AddScoped<MockService>())
        {
        }

        [Test]
        [Receive_ScopedProvider_Service]
        public void Simple_Case() => true.Should().BeTrue();

        [Test]
        [Repeat(2)]
        [Receive_ScopedProvider_Service]
        public void Repeat_Case() => true.Should().BeTrue();

        [TestCase(10)]
        [TestCase(11)]
        public void Test_Case(int value) => value.Should().BePositive();
    }
}
