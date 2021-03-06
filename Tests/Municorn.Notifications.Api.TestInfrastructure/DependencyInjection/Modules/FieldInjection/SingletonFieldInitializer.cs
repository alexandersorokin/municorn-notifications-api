using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Framework;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.Abstractions;

namespace Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection
{
    [PrimaryConstructor]
    internal sealed partial class SingletonFieldInitializer : IFixtureOneTimeSetUpService
    {
        private readonly FieldInfoProvider fieldInfoProvider;
        private readonly IServiceProvider serviceProvider;
        private readonly IFixtureProvider fixtureProvider;

        public void Run()
        {
            foreach (var field in this.fieldInfoProvider.Fields.Where(field => field.GetAttributes<IInjectFieldDependency>(true).Any()))
            {
                var value = this.serviceProvider.GetRequiredService(field.FieldType);
                field.SetValue(this.fixtureProvider.Fixture, value);
            }
        }
    }
}
