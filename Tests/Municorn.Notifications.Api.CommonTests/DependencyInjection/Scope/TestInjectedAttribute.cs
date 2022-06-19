using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    internal sealed class TestInjectedAttribute : CombiningStrategyAttribute, ITestBuilder
    {
        private static readonly CombinatorialStrategy CombinatorialStrategy = new();
        private static readonly NUnitTestCaseBuilder TestCaseBuilder = new();

        public TestInjectedAttribute()
            : base(CombinatorialStrategy, new DefaultValueParameterDataSourceProvider())
        {
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test? suite)
        {
            var dataProvider = new ParameterDataSourceProvider();
            var makeParametersOptionalMethodWrapper = new HideInjectedMethodWrapper(method);
            var parameters = makeParametersOptionalMethodWrapper.GetParameters();

            try
            {
                var sources = parameters
                    .Select(parameterInfo => dataProvider
                        .GetDataFor(parameterInfo)
                        .Cast<object>()
                        .ToArray())
                    .ToArray<IEnumerable>();

                return CombinatorialStrategy.GetTestCases(sources).Select(
                    testCaseData =>
                    {
                        var buildTestMethod = TestCaseBuilder.BuildTestMethod(makeParametersOptionalMethodWrapper, suite, (TestCaseParameters)testCaseData);
                        buildTestMethod.Method = new UseContainerMethodWrapper(method, buildTestMethod);
                        return buildTestMethod;
                    });
            }
            catch (InvalidDataSourceException ex)
            {
                var parms = new TestCaseParameters
                {
                    RunState = RunState.NotRunnable,
                };
                parms.Properties.Set(PropertyNames.SkipReason, ex.Message);
                return new[] { TestCaseBuilder.BuildTestMethod(method, suite, parms) };
            }
        }

        private class DefaultValueParameterDataSourceProvider : IParameterDataProvider
        {
            public bool HasDataFor(IParameterInfo parameter) => true;

            public IEnumerable GetDataFor(IParameterInfo parameter) => Enumerable.Empty<object>();
        }
    }
}