using System;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class InjectAttribute : Attribute
    {
    }
}
