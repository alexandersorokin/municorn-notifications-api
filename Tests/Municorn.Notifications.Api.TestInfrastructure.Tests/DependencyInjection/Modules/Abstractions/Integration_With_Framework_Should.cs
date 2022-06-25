using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.Abstractions
{
    [TestFixture]
    [RegisterSelfTypeModule]
    internal class Integration_With_Framework_Should
    {
        private readonly FixtureServiceProviderFramework framework;

        public Integration_With_Framework_Should() => this.framework = new(serviceCollection => serviceCollection
            .AddFixtureServiceCollectionModuleAttributes(new TypeWrapper(this.GetType()))
            .AddSingleton<IFixtureOneTimeSetUpService, OneTimeIncrement>());

        [OneTimeSetUp]
        public async Task OneTimeSetUp() => await this.framework.RunOneTimeSetUp().ConfigureAwait(false);

        [OneTimeTearDown]
        public async Task OneTimeTearDown() => await this.framework.DisposeAsync().ConfigureAwait(false);

        [Test]
        public void Test() => this.framework.Should().NotBeNull();

        private class OneTimeIncrement : IFixtureOneTimeSetUpService
        {
            private readonly Type type;

            public OneTimeIncrement(Type type) => this.type = type;

            public void Run() => this.type.Should().NotBeNull();
        }
    }
}
