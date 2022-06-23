using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Scopes.Inject
{
    public interface IInjectedService
    {
        Type? GetServiceType(object? methodFixture, object containerFixture);
    }
}
