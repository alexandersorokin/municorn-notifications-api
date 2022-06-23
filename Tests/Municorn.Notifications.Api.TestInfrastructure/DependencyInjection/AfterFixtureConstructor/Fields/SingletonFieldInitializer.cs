using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.AfterFixtureConstructor.Fields
{
    [PrimaryConstructor]
    internal partial class SingletonFieldInitializer : IFixtureOneTimeSetUp
    {
        private readonly FieldInfoProvider fieldInfoProvider;
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;

        public void Run()
        {
            foreach (var field in this.fieldInfoProvider.Fields.Where(field => field.GetCustomAttribute<TestDependencyAttribute>() != null))
            {
                var value = this.serviceProvider.GetRequiredService(field.FieldType);
                field.SetValue(this.fixtureProvider.Fixture, value);
            }
        }
    }
}
