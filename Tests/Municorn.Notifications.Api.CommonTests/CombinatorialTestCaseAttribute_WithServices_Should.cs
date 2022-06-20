using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor;
using Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject;
using Municorn.Notifications.Api.Tests.Logging;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests
{
    [TestFixture]
    internal class CombinatorialTestCaseAttribute_WithServices_Should : IConfigureServices
    {
        public void ConfigureServices(IServiceCollection serviceCollection) =>
            serviceCollection.AddSingleton<ITextWriterProvider, NUnitTextWriterProvider>();

        [CombinatorialTestCase(10, 1.1f, 100, "let")]
        [CombinatorialTestCase(11, 1.2d, null)]
        [Repeat(3)]
        public void Deduce_Generic_With_Optional<T1, T2>(
            [Inject(typeof(ITextWriterProvider))] object injectFirst,
            [Values] bool automaticData,
            int testCaseData,
            [Inject] ITextWriterProvider injectSecond,
            [Values("string", 777)] T1 automaticInfer,
            T2 testCaseInfer,
            int? testCaseDataConversion,
            [Values(true, null)] bool? valuesConversion,
            [Inject] GlobalLog setupFixture,
            string t = "var")
        {
            injectSecond.Should().NotBeNull();
        }

        [CombinatorialTestCase(10, "by")]
        [CombinatorialTestCase(11)]
        [Repeat(3)]
        public void Process_Optional_Without_Generic([Inject] ITextWriterProvider injected, int n, string x = "c")
        {
            injected.Should().NotBeNull();
        }

        [CombinatorialTestCase(10)]
        [Repeat(3)]
        public void Deduce_Case_Generic_Without_Optional<T>(T testCaseInfer)
        {
            testCaseInfer.Should().NotBeNull();
        }

        [CombinatorialTestCase]
        [Repeat(3)]
        public void Deduce_Value_Provider_Generic_Without_Optional<T>([Values("string", 777)] T automaticInfer)
        {
            automaticInfer.Should().NotBeNull();
        }

        [CombinatorialTestCase(10)]
        [CombinatorialTestCase(11)]
        [Repeat(3)]
        public void Inject_From_Container_And_Case([Inject] ITextWriterProvider injected, int value)
        {
            injected.Should().NotBeNull();
            value.Should().BePositive();
        }

        [CombinatorialTestCase]
        [Repeat(3)]
        public void Inject_From_Container([Inject] GlobalLog injected)
        {
            injected.Should().NotBeNull();
        }

        [CombinatorialTestCase]
        [Repeat(3)]
        public void Inject_From_Value_Provider([Values(1, 2)] int value)
        {
            value.Should().BePositive();
        }

        [CombinatorialTestCase(1)]
        [CombinatorialTestCase(2)]
        [Repeat(3)]
        public void Inject_From_Case(int value)
        {
            value.Should().BePositive();
        }

        [CombinatorialTestCase]
        [Repeat(3)]
        public void Work_Without_Arguments()
        {
            true.Should().BeTrue();
        }
    }
}
