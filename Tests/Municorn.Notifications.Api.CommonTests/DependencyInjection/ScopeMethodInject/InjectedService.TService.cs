namespace Municorn.Notifications.Api.Tests.DependencyInjection.ScopeMethodInject
{
    internal class InjectedService<TService>
    {
        public override string ToString() => $"Service<{typeof(TService).Name}>";
    }
}
