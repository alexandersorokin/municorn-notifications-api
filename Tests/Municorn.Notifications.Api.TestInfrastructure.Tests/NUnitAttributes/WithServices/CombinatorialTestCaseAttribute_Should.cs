using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.TestInfrastructure.Logging;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.NUnitAttributes.WithServices
{
    [TestFixture]
    internal class CombinatorialTestCaseAttribute_Should : ITestFixture
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<NUnitTextWriterProvider>();

        [CombinatorialTestCase(10, 1.1f, 100, "provided")]
        [CombinatorialTestCase(11, 1.2d, null)]
        public void Integration<T1, T2>(
            [InjectDependency(typeof(NUnitTextWriterProvider))] object injectFirst,
            [Values] bool automaticData,
            int testCaseData,
            [InjectDependency] NUnitTextWriterProvider injectSecond,
            [Values("string", 777)] T1 automaticInfer,
            T2 testCaseInfer,
            int? testCaseDataConversion,
            [Values(true, null)] bool? valuesConversion,
            [InjectDependency] SetUpFixture setupFixture,
            string optional = "default")
        {
            injectSecond.Should().NotBeNull();
        }

        [CombinatorialTestCase(10)]
        public void Deduce_Generic_From_Case<T>(T value)
        {
            value.Should().NotBeNull();
        }

        [CombinatorialTestCase]
        public void Deduce_Generic_From_Provider<T>([Values("string", 777)] T value)
        {
            value.Should().NotBeNull();
        }

        [CombinatorialTestCase("provided")]
        [CombinatorialTestCase]
        public void Process_Optional_With_Container([InjectDependency] NUnitTextWriterProvider service, string optional = "default")
        {
            optional.Should().NotBeNull();
        }

        [CombinatorialTestCase("provided")]
        [CombinatorialTestCase]
        public void Process_Optional_With_Provider([Values] bool value, string optional = "default")
        {
            optional.Should().NotBeNull();
        }

        [CombinatorialTestCase(10, "provided")]
        [CombinatorialTestCase(11)]
        public void Process_Optional_With_Case(int value, string optional = "default")
        {
            optional.Should().NotBeNull();
        }

        [CombinatorialTestCase("provided")]
        [CombinatorialTestCase]
        public void Process_Optional(string optional = "default")
        {
            optional.Should().NotBeNull();
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        public void Inject_From_Container_And_Case([InjectDependency] NUnitTextWriterProvider service, int value)
        {
            service.Should().NotBeNull();
            value.Should().BePositive();
        }

        [CombinatorialTestCase]
        public void Inject_From_Container([InjectDependency] NUnitTextWriterProvider service)
        {
            service.Should().NotBeNull();
        }

        [CombinatorialTestCase]
        public void Inject_From_Provider([Values(1, 2)] int value)
        {
            value.Should().BePositive();
        }

        [CombinatorialTestCase(1)]
        [CombinatorialTestCase(2)]
        public void Inject_From_Case(int value)
        {
            value.Should().BePositive();
        }

        [CombinatorialTestCase]
        public void Work_Without_Arguments()
        {
            true.Should().BeTrue();
        }
    }
}
