using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal class Provider_Scoped_Test_Should
    {
        [Test]
        public async Task Run()
        {
            var currentTest = TestExecutionContext.CurrentContext.CurrentTest;

            TestAccessor testAccessor = new();
            FixtureServiceProviderFramework framework = new(serviceCollection => serviceCollection
                .AddScoped(sp =>
                {
                    testAccessor = sp.GetRequiredService<TestAccessor>();
                    return Substitute.For<IFixtureSetUpService>();
                }));

            await using (framework.ConfigureAwait(false))
            {
                await framework.RunSetUp(currentTest).ConfigureAwait(false);
            }

            testAccessor.Test.Should().Be(currentTest);
        }
    }
}
