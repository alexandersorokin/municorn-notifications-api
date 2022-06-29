using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    internal class Inject_Field_From_Base_Class_Should : Inject_Field_From_Base_Class
    {
        public Inject_Field_From_Base_Class_Should()
            : base(serviceCollection => serviceCollection
                .AddFieldInjection(typeof(Inject_Field_From_Base_Class_Should))
                .AddSingleton<MockService>())
        {
        }

        [Test]
        public void Case() => this.Service.Should().NotBeNull();

        [TestCase(10)]
        [TestCase(11)]
        public void Cases(int value) => this.Service.Should().NotBeNull();
    }
}
