using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Framework
{
    [TestFixture]
    internal sealed class Integration_Create_Scope_For_Test_With_Different_Service_Instances_Should : IDisposable
    {
        private const int RepeatCount = 3;

        private readonly ConcurrentDictionary<AutoResolveService, bool> storage = new();
        private readonly FixtureServiceProviderFramework framework;

        public Integration_Create_Scope_For_Test_With_Different_Service_Instances_Should() => this.framework = new(serviceCollection => serviceCollection
            .AddSingleton(this.storage)
            .AddScoped<IFixtureSetUpService, AutoResolveService>());

        private static Test CurrentTest => TestExecutionContext.CurrentContext.CurrentTest;

        [SetUp]
        public async Task SetUp() => await this.framework.RunSetUp(CurrentTest).ConfigureAwait(false);

        [TearDown]
        public async Task TearDown() => await this.framework.RunTearDown(CurrentTest).ConfigureAwait(false);

        [Test]
        [Repeat(RepeatCount)]
        public void Test1() => this.storage.Should().HaveCountGreaterThan(1);

        [Test]
        public void Test2() => this.storage.Should().HaveCountGreaterThan(1);

        public void Dispose()
        {
            const int notRepeatedTests = 1;
            const int repeatedTests = 1;
            const int repeatedTestRuns = repeatedTests * RepeatCount;
            this.storage.Should().HaveCount(notRepeatedTests + repeatedTestRuns);
        }

        private sealed class AutoResolveService : IFixtureSetUpService
        {
            private readonly ConcurrentDictionary<AutoResolveService, bool> storage;

            public AutoResolveService(ConcurrentDictionary<AutoResolveService, bool> storage) => this.storage = storage;

            public void Run() => this.storage.TryAdd(this, true);
        }
    }
}
