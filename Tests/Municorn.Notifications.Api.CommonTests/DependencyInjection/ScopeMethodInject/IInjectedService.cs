using System;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject
{
    internal interface IInjectedService
    {
        Type? GetServiceType(object? methodFixture, object containerFixture);
    }
}
