using System;
using NUnit.Framework;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.BeforeFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TestFixtureSourceInjectedAttribute : NUnitAttribute
    {
    }
}
