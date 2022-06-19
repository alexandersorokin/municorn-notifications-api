using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    internal sealed class TestCaseInjectedAttribute : NUnitAttribute, ITestBuilder
    {
        private readonly TestCaseAttribute implementation;

        public TestCaseInjectedAttribute(params object?[]? arguments)
        {
            this.implementation = new(arguments);
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test? suite)
        {
            var testMethods = this.implementation.BuildFrom(new HideInjectedMethodWrapper(method), suite);
            foreach (var testMethod in testMethods)
            {
                testMethod.Method = new UseContainerMethodWrapper(method, testMethod);
                yield return testMethod;
            }
        }
    }
}