using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.NUnitAttributes
{
    [TestFixture]
    internal class CombinatorialTestCaseSourceAttribute_Constructors_Should
    {
        private static IEnumerable<TestCaseData> GetValueData(int value) => new[] { new TestCaseData(value) };

        private static IEnumerable<TestCaseData> ValueData => GetValueData(1);

        [CombinatorialTestCaseSource(nameof(ValueData))]
        [CombinatorialTestCaseSource(nameof(GetValueData), new object[] { 1 })]
        [CombinatorialTestCaseSource(typeof(Cases))]
        [CombinatorialTestCaseSource(typeof(CaseMethod), nameof(CaseMethod.ClassValueData))]
        [CombinatorialTestCaseSource(typeof(CaseMethod), nameof(CaseMethod.GetClassValueData), new object[] { 1 })]
        public void Consume_Value(int value) => value.Should().Be(1);

        private static class CaseMethod
        {
            internal static IEnumerable<TestCaseData> GetClassValueData(int value) => new[] { new TestCaseData(value) };

            internal static IEnumerable<TestCaseData> ClassValueData => GetClassValueData(1);
        }

        private class Cases : IEnumerable<int>
        {
            public IEnumerator<int> GetEnumerator()
            {
                yield return 1;
            }

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }
    }
}
