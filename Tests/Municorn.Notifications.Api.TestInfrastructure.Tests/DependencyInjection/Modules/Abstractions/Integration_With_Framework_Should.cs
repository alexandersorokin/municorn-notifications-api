using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.Abstractions
{
    [RegisterSelfTypeModule]
    internal class Integration_With_Framework_Should : FrameworkServiceProviderFixtureBase
    {
        public Integration_With_Framework_Should()
            : base(serviceCollection => serviceCollection
                .AddFixtureServiceCollectionModuleAttributes(typeof(Integration_With_Framework_Should))
                .AddSingleton<IFixtureOneTimeSetUpService, OneTimeIncrement>())
        {
        }

        [Test]
        public void Test() => true.Should().BeTrue();

        private class OneTimeIncrement : IFixtureOneTimeSetUpService
        {
            private readonly Type type;

            public OneTimeIncrement(Type type) => this.type = type;

            public void Run() => this.type.Should().NotBeNull();
        }
    }
}
