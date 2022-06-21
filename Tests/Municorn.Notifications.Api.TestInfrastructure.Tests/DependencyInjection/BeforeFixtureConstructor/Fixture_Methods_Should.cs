﻿using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixtureConstructor.Tests
{
    [TestFixtureInjectable]
    [TestModule]
    internal class Fixture_Methods_Should
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            false.Should().BeTrue();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            false.Should().BeTrue();
        }

        [SetUp]
        public void SetUp()
        {
            true.Should().BeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            true.Should().BeTrue();
        }

        [Test]
        [Repeat(2)]
        public void Case()
        {
            true.Should().BeTrue();
        }

        [TestCase(10)]
        [TestCase(11)]
        [Repeat(2)]
        public void Cases(int value)
        {
            true.Should().BeTrue();
        }
    }
}