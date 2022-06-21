using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    [PrimaryConstructor]
    internal sealed partial class CombinatorialTestCaseSourceAttribute : DisableCombiningStrategyAttribute, ITestBuilder
    {
        private static readonly MethodInfo? GetTestCasesForImplementation = typeof(TestCaseSourceAttribute)
            .GetMethod("GetTestCasesFor", BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly TestCaseSourceAttribute implementation;

        public CombinatorialTestCaseSourceAttribute(string sourceName)
            : this(new TestCaseSourceAttribute(sourceName))
        {
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test? suite)
        {
            var testCaseDatas = this.GetTestCasesFor(method);
            var testMethods = testCaseDatas
                .SelectMany(parameters =>
                {
                    ITestBuilder attribute = new CombinatorialTestCaseAttribute(parameters);
                    return attribute.BuildFrom(method, suite);
                })
                .ToArray();

            return testMethods.Any()
                ? testMethods
                : CombinatorialTestCaseAttribute.CreateNoCases(method, suite);
        }

        private IEnumerable<ITestCaseData> GetTestCasesFor(IMethodInfo methodInfo)
        {
            var method = GetTestCasesForImplementation ?? throw new InvalidOperationException("Reflection method is not found");
            var testCases = method.Invoke(this.implementation, new object[] { methodInfo }) as IEnumerable<ITestCaseData>;
            return testCases ?? throw new InvalidOperationException("Reflection call returns nothing");
        }
    }
}