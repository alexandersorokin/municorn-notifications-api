using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.ScopeMethodInject
{
    public interface IInjectedService
    {
        Type? GetServiceType(object? methodFixture, object containerFixture);
    }
}
