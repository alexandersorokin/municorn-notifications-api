namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    internal class InjectedService<TService>
    {
        public override string ToString() => $"Service<{typeof(TService).Name}>";
    }
}
