using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor
{
    [PrimaryConstructor]
    internal partial class SingletonFieldInitializer : IFixtureOneTimeSetUp
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;

        public void Run()
        {
            var fixture = this.fixtureProvider.Fixture;
            var fields = fixture
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(field => field.GetCustomAttribute<TestDependencyAttribute>() != null);

            foreach (var field in fields)
            {
                var value = this.serviceProvider.GetRequiredService(field.FieldType);
                field.SetValue(fixture, value);
            }
        }
    }
}
