namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [PrimaryConstructor]
    internal sealed partial class FixtureProvider : IFixtureProvider
    {
        public object Fixture { get; }
    }
}