namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection
{
    [PrimaryConstructor]
    internal partial class TestTestFixtureProvider : ITestFixtureProvider
    {
        public object Fixture { get; }
    }
}