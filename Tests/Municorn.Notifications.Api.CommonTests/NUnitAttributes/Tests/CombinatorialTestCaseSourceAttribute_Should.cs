using FluentAssertions;
using NUnit.Framework;

namespace Municorn.Notifications.Api.Tests.NUnitAttributes.Tests
{
    [TestFixture]
    internal class CombinatorialTestCaseSourceAttribute_Should
    {
        private static readonly TestCaseData[] ValueData =
        {
            new(1),
            new(2),
        };

        private static readonly TestCaseData[] ReturnValueData =
        {
            new(1)
            {
                ExpectedResult = 1,
            },
        };

        [CombinatorialTestCaseSource(nameof(ReturnValueData))]
        public int Return_Value(int value) => value;

        [CombinatorialTestCaseSource(nameof(ValueData))]
        public void Deduce_Generic<T>(T value)
        {
            value.Should().NotBeNull();
        }

        [CombinatorialTestCaseSource(nameof(ValueData))]
        public void Process_Optional(int value, string optional = "default")
        {
            optional.Should().NotBeNull();
        }

        [CombinatorialTestCaseSource(nameof(ValueData))]
        public void Inject_From_Provider([Values] bool provided, int value)
        {
            value.Should().BePositive();
        }

        [CombinatorialTestCaseSource(nameof(ValueData))]
        public void Inject(int value)
        {
            value.Should().BePositive();
        }
    }
}
