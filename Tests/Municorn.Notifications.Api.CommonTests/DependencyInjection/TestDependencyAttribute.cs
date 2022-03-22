using System;

namespace Municorn.Notifications.Api.Tests.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field)]
    internal sealed class TestDependencyAttribute : Attribute
    {
    }
}
