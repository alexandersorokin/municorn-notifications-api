using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    public interface IInjectedService
    {
        Type? GetServiceType(object? methodCallTargetFixture, object containerFixture);
    }
}
