namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class FixtureProvider : IFixtureProvider
    {
        public object Fixture { get; }
    }
}