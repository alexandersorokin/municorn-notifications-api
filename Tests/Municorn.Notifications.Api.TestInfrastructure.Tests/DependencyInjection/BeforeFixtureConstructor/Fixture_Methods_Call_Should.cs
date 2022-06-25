using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.FixtureBuilder;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.BeforeFixtureConstructor
{
    [TestFixtureInjectable]
    internal sealed class Fixture_Methods_Call_Should : IDisposable
    {
        private readonly Counter oneTimSetUpCounter = new();
        private readonly Counter oneTimeTearDownCounter = new();
        private readonly Counter setUpCounter = new();
        private readonly Counter tearDownCounter = new();
        private readonly Counter caseCounter = new();
        private readonly Counter casesCounter = new();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.oneTimSetUpCounter.Increment();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.oneTimeTearDownCounter.Increment();
        }

        [SetUp]
        public void SetUp()
        {
            this.setUpCounter.Increment();
        }

        [TearDown]
        public void TearDown()
        {
            this.tearDownCounter.Increment();
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            this.caseCounter.Increment();
            true.Should().BeTrue();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            this.casesCounter.Increment();
            true.Should().BeTrue();
        }

        public void Dispose()
        {
            using (new AssertionScope())
            {
                this.oneTimSetUpCounter.Value.Should().Be(1);
                this.oneTimeTearDownCounter.Value.Should().Be(1);
                this.setUpCounter.Value.Should().Be(6);
                this.tearDownCounter.Value.Should().Be(6);
                this.caseCounter.Value.Should().Be(2);
                this.casesCounter.Value.Should().Be(4);
            }
        }
    }
}
