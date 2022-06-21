using System;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject
{
    public interface IInjectedService
    {
        Type? GetServiceType(object? methodFixture, object containerFixture);
    }
}
