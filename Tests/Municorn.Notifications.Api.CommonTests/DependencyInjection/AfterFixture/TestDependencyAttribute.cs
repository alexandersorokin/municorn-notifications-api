using System;
using JetBrains.Annotations;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixture
{
    [AttributeUsage(AttributeTargets.Field)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    internal sealed class TestDependencyAttribute : Attribute
    {
    }
}
