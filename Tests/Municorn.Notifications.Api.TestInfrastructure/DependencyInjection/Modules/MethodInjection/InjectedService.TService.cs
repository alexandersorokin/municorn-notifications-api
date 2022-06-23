using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    public class InjectedService<TService> : IInjectedService
    {
        public override string ToString() => $"Service<{typeof(TService).Name}>";

        public Type? GetServiceType(object? methodFixture, object containerFixture) =>
            methodFixture == containerFixture
                ? typeof(TService)
                : null;
    }
}
