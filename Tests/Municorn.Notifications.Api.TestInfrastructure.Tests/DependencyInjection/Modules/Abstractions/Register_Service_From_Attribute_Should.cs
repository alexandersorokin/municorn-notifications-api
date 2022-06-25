using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.Abstractions
{
    [TestFixture]
    [RegisterSelfTypeModule]
    internal class Register_Service_From_Attribute_Should
    {
        [Test]
        public void Register_Service()
        {
            var sp = new ServiceCollection()
                .AddFixtureServiceCollectionModuleAttributes(this.GetType())
                .BuildServiceProvider();

            sp.GetRequiredService<Type>().Should().NotBeNull();
        }

        [Test]
        public void Use_Type_Information_To_Register()
        {
            var type = this.GetType();

            var sp = new ServiceCollection()
                .AddFixtureServiceCollectionModuleAttributes(type)
                .BuildServiceProvider();

            sp.GetRequiredService<Type>().Should().Be(type);
        }
    }
}
