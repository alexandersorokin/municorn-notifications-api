using System.Threading.Tasks;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal class Framework_Should
    {
        [Test]
        public async Task Create()
        {
            var framework = new FixtureServiceProviderFramework(_ => { });
            await using (framework.ConfigureAwait(false))
            {
                framework.Should().NotBeNull();
            }
        }
    }
}
