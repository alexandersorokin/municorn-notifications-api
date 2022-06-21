using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    [PrimaryConstructor]
    public sealed partial class CombinatorialTestCaseSourceAttribute : DisableCombiningStrategyAttribute, ITestBuilder
    {
        private static readonly MethodInfo? GetTestCasesForImplementation = typeof(TestCaseSourceAttribute)
            .GetMethod("GetTestCasesFor", BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly TestCaseSourceAttribute implementation;

        public CombinatorialTestCaseSourceAttribute(string sourceName)
            : this(new TestCaseSourceAttribute(sourceName))
        {
        }

        public CombinatorialTestCaseSourceAttribute(Type sourceType, string sourceName, object?[]? methodParams)
            : this(new TestCaseSourceAttribute(sourceType, sourceName, methodParams))
        {
        }

        public CombinatorialTestCaseSourceAttribute(Type sourceType, string sourceName)
            : this(new TestCaseSourceAttribute(sourceType, sourceName))
        {
        }

        public CombinatorialTestCaseSourceAttribute(string sourceName, object?[]? methodParams)
            : this(new TestCaseSourceAttribute(sourceName, methodParams))
        {
        }

        public CombinatorialTestCaseSourceAttribute(Type sourceType)
            : this(new TestCaseSourceAttribute(sourceType))
        {
        }

        public string? Category
        {
            get => this.implementation.Category;
            set => this.implementation.Category = value;
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