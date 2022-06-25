using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.Abstractions
{
    [TestFixture]
    internal class Register_Service_From_Base_Class_Should : BaseClassWithModule
    {
        [Test]
        public void Register_Service()
        {
            var sp = new ServiceCollection()
                .AddFixtureServiceCollectionModuleAttributes(new TypeWrapper(this.GetType()))
                .BuildServiceProvider();

            sp.GetRequiredService<Type>().Should().NotBeNull();
        }
    }
}
