using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AutoMethods;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [PrimaryConstructor]
    internal partial class SingletonFieldInitializer : IFixtureOneTimeSetUp
    {
        private readonly IFixtureProvider fixtureProvider;
        private readonly ServiceProviderAccessor serviceProviderAccessor;

        public void Run()
        {
            var fixture = this.fixtureProvider.Fixture;
            var fields = fixture
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttribute<TestDependencyAttribute>() != null);

            foreach (var field in fields)
            {
                var value = this.serviceProviderAccessor.ServiceProvider.GetRequiredService(field.FieldType);
                field.SetValue(fixture, value);
            }
        }
    }
}
