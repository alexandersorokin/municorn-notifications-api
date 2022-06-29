using System;
using Microsoft.Extensions.DependencyInjection;
using Municorn.Notifications.Api.TestInfrastructure.DependencyInjection.Modules.FieldInjection;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.Modules.FieldInjection
{
    internal abstract class Inject_Field_From_Base_Class : FrameworkServiceProviderFixtureBase
    {
        protected Inject_Field_From_Base_Class(Action<IServiceCollection> configureServices)
            : base(configureServices)
        {
        }

        [field: FieldDependency]
        protected MockService Service { get; } = default!;
    }
}
