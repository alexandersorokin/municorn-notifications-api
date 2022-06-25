using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules
{
    internal abstract class FrameworkServiceProviderFixtureBase
    {
        private readonly FixtureServiceProviderFramework framework;

        protected FrameworkServiceProviderFixtureBase(Action<IServiceCollection> configureServices) =>
            this.framework = new(configureServices);

        private static Test CurrentTest => TestExecutionContext.CurrentContext.CurrentTest;

        [OneTimeSetUp]
        protected async Task OneTimeSetUp() => await this.framework.RunOneTimeSetUp().ConfigureAwait(false);

        [SetUp]
        protected async Task SetUp() => await this.framework.RunSetUp(CurrentTest).ConfigureAwait(false);

        [TearDown]
        protected async Task TearDown() => await this.framework.RunTearDown(CurrentTest).ConfigureAwait(false);

        [OneTimeTearDown]
        protected async Task OneTimeTearDown() => await this.framework.DisposeAsync().ConfigureAwait(false);
    }
}
