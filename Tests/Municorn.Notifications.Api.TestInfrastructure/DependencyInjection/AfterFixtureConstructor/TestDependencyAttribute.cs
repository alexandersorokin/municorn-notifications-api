using System;
using JetBrains.Annotations;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.AfterFixtureConstructor
{
    [AttributeUsage(AttributeTargets.Field)]
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    public sealed class TestDependencyAttribute : Attribute
    {
    }
}
