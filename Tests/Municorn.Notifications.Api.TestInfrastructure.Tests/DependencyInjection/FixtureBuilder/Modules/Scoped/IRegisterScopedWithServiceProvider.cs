using System;

namespace Municorn.Notifications.Api.TestInfrastructure.Tests.DependencyInjection.FixtureBuilder.Modules.Scoped
{
    internal interface IRegisterScopedWithServiceProvider<out TService>
    {
        TService Get(IServiceProvider serviceProvider);
    }
}
