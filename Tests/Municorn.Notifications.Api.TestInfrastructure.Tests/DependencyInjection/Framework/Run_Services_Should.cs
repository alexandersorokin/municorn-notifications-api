using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal class Run_Services_Should
    {
        [Test]
        public async Task OneTimeSetUp()
        {
            var service = Substitute.For<IFixtureOneTimeSetUpService>();
            var framework = CreateFramework(service);

            await using (framework.ConfigureAwait(false))
            {
                await framework.RunOneTimeSetUp().ConfigureAwait(false);
            }

            service.Received().Run();
        }

        [Test]
        public async Task OneTimeSetUpAsync()
        {
            var service = Substitute.For<IFixtureOneTimeSetUpAsyncService>();
            var framework = CreateFramework(service);

            await using (framework.ConfigureAwait(false))
            {
                await framework.RunOneTimeSetUp().ConfigureAwait(false);
            }

            await service.Received().RunAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task SetUp()
        {
            var service = Substitute.For<IFixtureSetUpService>();
            var framework = CreateFramework(service);

            await using (framework.ConfigureAwait(false))
            {
                var currentTest = TestExecutionContext.CurrentContext.CurrentTest;
                await framework.RunSetUp(currentTest).ConfigureAwait(false);
            }

            service.Received().Run();
        }

        [Test]
        public async Task SetUpAsync()
        {
            var service = Substitute.For<IFixtureSetUpAsyncService>();
            var framework = CreateFramework(service);

            await using (framework.ConfigureAwait(false))
            {
                var currentTest = TestExecutionContext.CurrentContext.CurrentTest;
                await framework.RunSetUp(currentTest).ConfigureAwait(false);
            }

            await service.Received().RunAsync().ConfigureAwait(false);
        }

        private static FixtureServiceProviderFramework CreateFramework<TService>(TService service)
            where TService : class =>
            new(serviceCollection => serviceCollection.AddSingleton(service));
    }
}
