namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions
{
    [PrimaryConstructor]
    internal sealed partial class FixtureProvider : IFixtureProvider
    {
        public object Fixture { get; }
    }
}