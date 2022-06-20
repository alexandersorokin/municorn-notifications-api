using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    [AttributeUsage(AttributeTargets.Interface)]
    internal sealed class UseContainerAttribute : NUnitAttribute, ITestAction
    {
        private readonly ConditionalWeakTable<ITest, IMethodInfo> methodInfos = new();

        public void BeforeTest(ITest test)
        {
            var testMethod = (TestMethod)test;
            var methodInfo = testMethod.Method;
            this.methodInfos.Add(test, methodInfo);
            testMethod.Method = new UseContainerMethodWrapper(methodInfo, test);
        }

        public void AfterTest(ITest test)
        {
            var testMethod = (TestMethod)test;
            testMethod.Method = this.methodInfos.GetValue(
                test,
                _ => throw new InvalidOperationException($"Failed to restore MethodInfo for {test.FullName}"));
            if (!this.methodInfos.Remove(test))
            {
                throw new InvalidOperationException($"Failed to clear saved MethodInfo for {test.FullName}");
            }
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
