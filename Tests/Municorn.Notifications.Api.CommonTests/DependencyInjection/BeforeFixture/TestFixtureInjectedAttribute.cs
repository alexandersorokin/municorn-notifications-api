using System;
using System.Collections.Generic;
using System.Linq;
using Municorn.Notifications.Api.Tests.DependencyInjection.Scope;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.BeforeFixture
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class TestFixtureInjectedAttribute : NUnitAttribute, IFixtureBuilder2
    {
        private readonly TestFixtureAttribute implementation = new();

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo)
        {
            var suites = this.implementation.BuildFrom(new TypeInfoWrapper(typeInfo));
            return AddInjectSupportToTests(suites);
        }

        public IEnumerable<TestSuite> BuildFrom(ITypeInfo typeInfo, IPreFilter filter)
        {
            var suites = this.implementation.BuildFrom(new TypeInfoWrapper(typeInfo), filter);
            return AddInjectSupportToTests(suites);
        }

        private static void PatchTestMethods(IEnumerable<ITest> tests)
        {
            foreach (var test in tests)
            {
                if (test.IsSuite)
                {
                    PatchTestMethods(test.Tests);
                }
                else
                {
                    var testMethod = (TestMethod)test;
                    testMethod.Method = new UseContainerMethodWrapper(testMethod.Method, testMethod);
                }
            }
        }

        private static IEnumerable<TestSuite> AddInjectSupportToTests(IEnumerable<TestSuite> suites)
        {
            var testSuites = suites.ToArray();
            PatchTestMethods(testSuites);
            return testSuites;
        }
    }
}