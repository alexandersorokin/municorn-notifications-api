using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests
{
    [TestFixture]
    internal class CombinatorialTestCaseAttribute_WithoutServices_Should
    {
        [CombinatorialTestCase(10, 1.1f, 100, "let")]
        [CombinatorialTestCase(11, 1.2d, null)]
        [Repeat(3)]
        public void Deduce_Generic_With_Optional<T1, T2>(
            [Values] bool automaticData,
            int testCaseData,
            [Values("string", 777)] T1 automaticInfer,
            T2 testCaseInfer,
            int? testCaseDataConversion,
            [Values(true, null)] bool? valuesConversion,
            string t = "var")
        {
            automaticInfer.Should().NotBeNull();
        }

        [CombinatorialTestCase(10, "by")]
        [CombinatorialTestCase(11)]
        [Repeat(3)]
        public void Process_Optional_Without_Generic([Values] bool injected, int n, string x = "c")
        {
            n.Should().BePositive();
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
        public void Inject_From_Value_Provider_And_Case([Values] bool x, int value)
        {
            value.Should().BePositive();
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
