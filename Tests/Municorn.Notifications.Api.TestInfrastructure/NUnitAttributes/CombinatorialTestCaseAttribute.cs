using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Municorn.Notifications.Api.TestInfrastructure.NUnitAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    [PrimaryConstructor]
    public sealed partial class CombinatorialTestCaseAttribute : DisableCombiningStrategyAttribute, ITestBuilder
    {
        private static readonly ParameterDataSourceProvider DataProvider = new();
        private static readonly CombinatorialStrategy CombinatorialStrategy = new();
        private static readonly NUnitTestCaseBuilder TestCaseBuilder = new();

        private readonly ITestCaseData testCaseData;
        private object? expectedResult;

        public CombinatorialTestCaseAttribute(params object?[] arguments)
            : this(new TestCaseData(arguments))
        {
        }

        public object? ExpectedResult
        {
            get => this.expectedResult;
            set
            {
                this.expectedResult = value;
                this.HasExpectedResult = true;
            }
        }

        public bool HasExpectedResult { get; private set; }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test? suite)
        {
            if (this.testCaseData.RunState != RunState.Runnable)
            {
                var testCaseParameters = this.CreateParameters(this.testCaseData);
                return new[] { TestCaseBuilder.BuildTestMethod(method, suite, testCaseParameters) };
            }

            try
            {
                var sources = this.MergeArguments(method.GetParameters()).ToArray<IEnumerable>();
                var testMethods = CombinatorialStrategy
                    .GetTestCases(sources)
                    .Select(this.CreateParameters)
                    .Select(parameters => TestCaseBuilder.BuildTestMethod(method, suite, parameters))
                    .ToArray();

                return testMethods.Any()
                    ? testMethods
                    : CreateNoCases(method, suite);
            }
            catch (Exception ex)
            {
                return CreateNotRunnable(method, suite, ex.Message);
            }
        }

        internal static IEnumerable<TestMethod> CreateNoCases(IMethodInfo method, Test? suite) =>
            CreateNotRunnable(method, suite, "No test cases were generated");

        private static IEnumerable<TestMethod> CreateNotRunnable(IMethodInfo method, Test? suite, string skipReason)
        {
            var parameters = new TestCaseParameters
            {
                RunState = RunState.NotRunnable,
            };
            parameters.Properties.Set(PropertyNames.SkipReason, skipReason);
            return new[] { TestCaseBuilder.BuildTestMethod(method, suite, parameters) };
        }

        private IEnumerable<object?[]> MergeArguments(IEnumerable<IParameterInfo> parameters)
        {
            var usedIndex = 0;
            var arguments = this.testCaseData.Arguments;
            foreach (var parameter in parameters)
            {
                if (parameter.GetCustomAttributes<IParameterDataSource>(false).Any())
                {
                    yield return DataProvider.GetDataFor(parameter).Cast<object>().ToArray();
                }
                else if (usedIndex < arguments.Length)
                {
                    yield return new[] { arguments[usedIndex++] };
                }
                else if (parameter.IsOptional)
                {
                    yield return new[] { Type.Missing };
                }
            }

            for (var i = usedIndex; i < arguments.Length; i++)
            {
                yield return new[] { arguments[usedIndex] };
            }
        }

        private TestCaseParameters CreateParameters(ITestCaseData testCaseDataArguments)
        {
            var parameters = new OriginalChangeableTestCaseParameters(
                testCaseDataArguments.Arguments,
                testCaseDataArguments.Arguments.TakeWhile(arg => arg != Type.Missing).ToArray())
            {
                RunState = this.testCaseData.RunState,
                TestName = this.testCaseData.TestName,
            };

            if (this.testCaseData.HasExpectedResult)
            {
                parameters.ExpectedResult = this.testCaseData.ExpectedResult;
            }

            if (this.HasExpectedResult)
            {
                parameters.ExpectedResult = this.ExpectedResult;
            }

            var properties = this.testCaseData.Properties;
            foreach (var name in properties.Keys)
            {
                parameters.Properties[name] = properties[name];
            }

            return parameters;
        }

        private class OriginalChangeableTestCaseParameters : TestCaseParameters
        {
            public OriginalChangeableTestCaseParameters(object?[] args, object?[] originalArgs)
                : base(args)
            {
                this.OriginalArguments = originalArgs;
            }
        }
    }
}