using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    internal sealed class CombinatorialTestCaseAttribute : CombiningStrategyAttribute, ITestBuilder
    {
        private static readonly ParameterDataSourceProvider DataProvider = new();
        private static readonly CombinatorialStrategy CombinatorialStrategy = new();
        private static readonly NUnitTestCaseBuilder TestCaseBuilder = new();

        private readonly object?[] arguments;

        public CombinatorialTestCaseAttribute(params object?[] arguments)
            : base(CombinatorialStrategy, new DefaultValueParameterDataSourceProvider())
        {
            this.arguments = arguments;
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test? suite)
        {
            try
            {
                var sources = this.GetSources(method.GetParameters()).ToArray<IEnumerable>();
                return CombinatorialStrategy
                    .GetTestCases(sources)
                    .Select(
                        testCaseData => TestCaseBuilder.BuildTestMethod(method, suite, (TestCaseParameters)testCaseData))
                    .ToArray();
            }
            catch (Exception ex)
            {
                var parms = new TestCaseParameters
                {
                    RunState = RunState.NotRunnable,
                };
                parms.Properties.Set(PropertyNames.SkipReason, ex.Message);
                return new[] { TestCaseBuilder.BuildTestMethod(method, suite, parms) };
            }
        }

        private IEnumerable<object?[]> GetSources(IEnumerable<IParameterInfo> parameters)
        {
            var usedIndex = 0;
            foreach (var parameterInfo in parameters)
            {
                if (parameterInfo.GetCustomAttributes<IParameterDataSource>(false).Any())
                {
                    yield return DataProvider.GetDataFor(parameterInfo).Cast<object>().ToArray();
                }
                else if (usedIndex < this.arguments.Length)
                {
                    yield return new[] { this.arguments[usedIndex++] };
                }
            }

            for (var i = usedIndex; i < this.arguments.Length; i++)
            {
                yield return new[] { this.arguments[usedIndex] };
            }
        }

        private class DefaultValueParameterDataSourceProvider : IParameterDataProvider
        {
            public bool HasDataFor(IParameterInfo parameter) => true;

            public IEnumerable GetDataFor(IParameterInfo parameter) => Enumerable.Empty<object>();
        }
    }
}