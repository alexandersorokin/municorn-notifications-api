using System;
using NUnit.Framework.Internal;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection
{
    internal static class TestActionSuiteOneTimeTearDownExceptionLogger
    {
        internal static void DoInSafeContext(Action action)
        {
            // workaround https://github.com/nunit/nunit/issues/2938
            try
            {
                action();
            }
            catch (Exception ex)
            {
                TestExecutionContext.CurrentContext.CurrentResult.RecordTearDownException(ex);
            }
        }
    }
}
