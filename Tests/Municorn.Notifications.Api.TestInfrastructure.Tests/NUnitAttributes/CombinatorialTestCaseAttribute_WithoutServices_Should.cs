using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.NUnitAttributes
{
    [TestFixture]
    internal class CombinatorialTestCaseAttribute_WithoutServices_Should
    {
        [CombinatorialTestCase(10, 1.1f, 100, "provided")]
        [CombinatorialTestCase(11, 1.2d, null)]
        public void Integration<T1, T2>(
            [Values] bool automaticData,
            int testCaseData,
            [Values("string", 777)] T1 automaticInfer,
            T2 testCaseInfer,
            int? testCaseDataConversion,
            [Values(true, null)] bool? valuesConversion,
            string optional = "default")
        {
            automaticInfer.Should().NotBeNull();
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
        public void Inject_From_Provider_And_Case([Values] bool provided, int value)
        {
            value.Should().BePositive();
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
