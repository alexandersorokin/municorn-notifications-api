using System;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.MethodInjection
{
    public sealed class InjectedService<TService> : IInjectedService
    {
        public override string ToString() => $"Service<{typeof(TService).Name}>";

        public Type? GetServiceType(object? methodCallTargetFixture, object containerFixture) =>
            methodCallTargetFixture == containerFixture
                ? typeof(TService)
                : null;
    }
}
